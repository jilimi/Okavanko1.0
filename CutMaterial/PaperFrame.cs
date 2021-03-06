using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;

using Rhino.Geometry;
using Rhino.FileIO;
using Rhino.DocObjects;
using System.Drawing;
using GH_IO.Serialization;

using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;
namespace CSCECDEC.Okavango.CutMaterial
{
    public struct PaperSize
    {
        public int Width, Height;
        public PaperSize(int Width,int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
    }
    public class PaperFrame : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DrawFrame class.
        /// </summary>
        /// 0 => A0图框
        /// 1 => A1图框
        /// 2 => A2图框
        /// 3 => A3图框
        private int DrawingFrameType = 0;
       // readonly string FrameFileName = "FrameFile_12345678.3dm";
        //注意，是这样初始化字典的
        public PaperFrame()
          : base("DrawFrame", "DrawFrame",
              "图框",
              Setting.PLUGINNAME, Setting.CUTDOWNCATATORY)
        {
            this.Message = "A0";
        }
        public override void RemovedFromDocument(GH_Document document)
        {
            
        }
        public override void CreateAttributes()
        {
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new GH_ComponentAttributes(this);

        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Scale", "S", "放大倍数",GH_ParamAccess.item,1);
            pManager.AddBooleanParameter("Orient", "O", "纸张的放置方向，false代表竖直放置，true代表水平放置",GH_ParamAccess.item,false);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "图框", GH_ParamAccess.item);
           
            
        }
        protected override void BeforeSolveInstance()
        {
          //  base.BeforeSolveInstance();
        }
        protected override void AfterSolveInstance()
        {
            this.ExpirePreview(true);
            base.AfterSolveInstance();
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool Oratation = false;
            double Scale = 1;
            Transform Trans = new Transform();

            if (!DA.GetData(0, ref Scale)) return;
            if (!DA.GetData(1, ref Oratation)) return;

            if (Scale == 0) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Scale 不能为0");

            Scale = Math.Abs(Scale);
            PaperSize Paper = this.GetPaperSize(this.DrawingFrameType);

            Rectangle3d Rect = new Rectangle3d(Plane.WorldXY, Paper.Width, Paper.Height);
            Trans = Transform.Scale(Rect.Center,Scale);
            Matrix Matrix = new Matrix(Trans);
            if (Oratation)
            {
                //矩阵的变换顺序
               Trans  = Transform.Multiply(Trans,Transform.Rotation(Math.PI / 2, Rect.Center));
            }
            Rect.Transform(Trans);
            DA.SetData(0, Rect);
        }
        private PaperSize GetPaperSize(int Paper)
        {

            PaperSize A0_Paper = new PaperSize(841, 1189);
            PaperSize A1_Paper = new PaperSize(594, 841);
            PaperSize A2_Paper = new PaperSize(420, 594);
            PaperSize A3_Paper = new PaperSize(297, 420);
            PaperSize A4_Paper = new PaperSize(210, 297);
            PaperSize AS_Paper = new PaperSize(200, 200);

            return Paper == 0 ? A0_Paper
                          : Paper == 1 ? A1_Paper
                          : Paper == 2 ? A2_Paper
                          : Paper == 3 ? A3_Paper
                          : Paper == 4 ? A4_Paper
                          : AS_Paper;
        }
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            this.AppendAddidentMenuItem(menu);
        }
        private void AppendAddidentMenuItem(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "A0图框", Do_Add_PaperFrame,true,this.DrawingFrameType==0?true:false);
            Menu_AppendItem(menu, "A1图框", Do_Add_PaperFrame,true,this.DrawingFrameType == 1 ? true : false);
            Menu_AppendItem(menu, "A2图框", Do_Add_PaperFrame,true,this.DrawingFrameType == 2 ? true : false);
            Menu_AppendItem(menu, "A3图框", Do_Add_PaperFrame,true,this.DrawingFrameType == 3 ? true : false);
            Menu_AppendItem(menu, "A4图框", Do_Add_PaperFrame,true,this.DrawingFrameType == 4 ? true : false);
            Menu_AppendItem(menu, "矩形图框", Do_Add_PaperFrame,true,this.DrawingFrameType == 5 ? true : false);

        }

        private void Do_Add_PaperFrame(object sender, EventArgs e)
        {
            ToolStripMenuItem Menu = sender as ToolStripMenuItem;
            string Text = Menu.Text;

            switch (Text)
            {
                case "A0图框":
                    this.DrawingFrameType = 0;
                    Menu.Checked = true;
                    this.Message = "A0图框";
                    break;
                case "A1图框":
                    this.DrawingFrameType = 1;
                    Menu.Checked = true;
                    this.Message = "A1图框";
                    break;
                case "A2图框":
                    this.DrawingFrameType = 2;
                    Menu.Checked = true;
                    this.Message = "A2图框";
                    break;
                case "A3图框":
                    this.DrawingFrameType = 3;
                    Menu.Checked = true;
                    this.Message = "A3图框";
                    break;
                case "A4图框":
                    this.DrawingFrameType = 4;
                    Menu.Checked = true;
                    this.Message = "A4图框";
                    break;
                case "矩形图框":
                    this.DrawingFrameType = 5;
                    Menu.Checked = true;
                    this.Message = "矩形图框";
                    break;
                default:
                    this.DrawingFrameType = 0;
                    Menu.Checked = true;
                    this.Message = "A0图框";
                    break;

            }
            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("PaperType", this.DrawingFrameType);
            return base.Write(writer);
        }
        public override bool Read(GH_IReader reader)
        {
            this.DrawingFrameType = reader.GetInt32("PaperType");
            return base.Read(reader);
        }
        /*
        private List<GeometryBase> ExactDrawingFrame(int DrawingType)
        {
            byte[] Rhino_Frame = Properties.Resources.Frame;
            string APPDataFolder = Grasshopper.Folders.AppDataFolder;
            string Type = DrawingType == 0 ? "A0"
                          : DrawingType == 1 ? "A1"
                          : DrawingType == 2 ? "A2"
                          : DrawingType == 3 ? "A3"
                          : DrawingType == 4 ? "A4"
                          : DrawingType == 5 ? "矩形图框"
                          : "A0";
            //如何将二进制的文件写入到3dm文件中
            string FilePath = APPDataFolder + "/" + "Temp_" + FrameFileName;
            if (!File.Exists(FilePath))
            {
                File.WriteAllBytes(FilePath,Rhino_Frame);
            }

            File3dm _3dmFile = File3dm.Read(FilePath);
            File3dmLayerTable Layer_Table = _3dmFile.AllLayers;

            string LayerName = "Frame_" + Type;
            Layer Dest_Layer = FindLayerByName(Layer_Table.ToList(),LayerName);
            if(Dest_Layer == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "不存在相应图框");
                return null;
            }

            List<File3dmObject> m_File3dmObject = _3dmFile.Objects.FindByLayer(Dest_Layer).ToList();
            return m_File3dmObject.Select(item => item.Geometry).ToList();
        }*/
        private Layer FindLayerByName(List<Layer> l_Layer,string Name)
        {
            List<Layer> LayerList = l_Layer.Where(item => { return item.Name == Name; }).ToList();

            if (LayerList.Count == 0) return null;
            else return LayerList[0];
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.PaperFrame;
                //Graphic 沒有public的構造函數，不能使用new運算符，衹能通過其他方式創建graphic
                Graphics graphic = Graphics.FromImage((Image)newImage);
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphic.DrawImage(originalImg, 0, 0, newImage.Width, newImage.Height);
                return newImage;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a8d9aa95-33cc-4c61-b0ec-e97dcbb9c8a9"); }
        }
    }
}