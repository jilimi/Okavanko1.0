using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;

using Rhino.Geometry;
using Rhino.FileIO;
using Rhino.DocObjects;


namespace CSCECDEC.Plugin.CutMaterial
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
    public class DrawFrame : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DrawFrame class.
        /// </summary>
        /// 0 => 白图框
        /// 1 => A1图框
        /// 2 => A2图框
        /// 3 => A3图框
        private int DrawingFrameType = 0;
        readonly string FrameFileName = "FrameFile_12345678.3dm";
        //注意，是这样初始化字典的
        public DrawFrame()
          : base("DrawFrame", "DrawFrame",
              "图框",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.CUTDOWNCATATORY)
        {
            this.Message = "白图框";
        }
        public override void RemovedFromDocument(GH_Document document)
        {
            
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Scale", "S", "放大倍数",GH_ParamAccess.item,1);
            pManager.AddBooleanParameter("Orientation", "O", "纸张的放置方向，false代表竖直放置，true代表水平放置",GH_ParamAccess.item,false);
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
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool Oratation = false;
            double Scale = 1;
            int Width = 0, Height = 0;

            if (!DA.GetData(0, ref Scale)) return;
            if (!DA.GetData(1, ref Oratation)) return;

            Scale = Math.Abs(Scale);
            Transform Trans;

            PaperSize Paper = this.GetPaperSize(this.DrawingFrameType);

            Rectangle3d Rect = new Rectangle3d(Plane.WorldXY, Paper.Width*Scale, Paper.Height*Scale);

            if (Oratation)
            {
                Trans = Transform.Rotation(Math.PI / 2, Rect.Center);
                Rect.Transform(Trans);
            }
           // List<GeometryBase> Output_Geoms = new List<GeometryBase>();
           // Output_Geoms = this.ExactDrawingFrame(this.DrawingFrameType);
           DA.SetData(0, Rect);
        }
        private PaperSize GetPaperSize(int Paper)
        {

            PaperSize A0_Paper = new PaperSize(841, 1189);
            PaperSize A1_Paper = new PaperSize(594, 841);
            PaperSize A2_Paper = new PaperSize(420, 594);
            PaperSize A3_Paper = new PaperSize(297, 420);
            PaperSize A4_Paper = new PaperSize(210, 297);

            return Paper == 0 ? A0_Paper
                          : Paper == 1 ? A1_Paper
                          : Paper == 2 ? A2_Paper
                          : Paper == 3 ? A3_Paper
                          : A4_Paper;
        }
        public override bool AppendMenuItems(ToolStripDropDown menu)
        {

            base.Menu_AppendObjectName(menu);
            base.Menu_AppendEnableItem(menu);
            base.Menu_AppendPreviewItem(menu);
            base.Menu_AppendObjectHelp(menu);
            GH_DocumentObject.Menu_AppendSeparator(menu);
            this.AppendAddidentMenuItem(menu);
            return true;
        }
        private void AppendAddidentMenuItem(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "白图框", Add_Plain_DrawingFrame);
            Menu_AppendItem(menu, "A1图框", Add_A1_DrawingFrame);
            Menu_AppendItem(menu, "A2图框", Add_A2_DrawingFrame);
            Menu_AppendItem(menu, "A3图框", Add_A3_DrawingFrame);
        }

        private void Add_Plain_DrawingFrame(object sender, EventArgs e)
        {
            this.DrawingFrameType = 0;
            this.Message = "白图框";
             
            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }

        private void Add_A3_DrawingFrame(object sender, EventArgs e)
        {
            this.DrawingFrameType = 3;
            this.Message = "A3图框";

            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }

        private void Add_A1_DrawingFrame(object sender, EventArgs e)
        {
            this.DrawingFrameType = 1;
            this.Message = "A1图框";

            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }

        private void Add_A2_DrawingFrame(object sender, EventArgs e)
        {
            this.DrawingFrameType = 2;
            this.Message = "A2图框";

            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }
        private List<GeometryBase> ExactDrawingFrame(int DrawingType)
        {
            byte[] Rhino_Frame = Properties.Resources.Frame;
            string APPDataFolder = Grasshopper.Folders.AppDataFolder;
            string Type = DrawingType == 0 ? "Plain"
                          : DrawingType == 1 ? "A1"
                          : DrawingType == 2 ? "A2"
                          : DrawingType == 3 ? "A3"
                          : "A1";
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
        }
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
                return null;
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