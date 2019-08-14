/*
using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;
using Rhino.Display;
using System.Windows.Forms;
using GH_IO.Serialization;

namespace CSCECDEC.Okavango.Preview
{
    public class PointPreview : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CurvePreview class.
        /// </summary>
        private List<GH_Point> PointList = new List<GH_Point>();
        private GH_Colour PointColor;
        public GH_Integer PointSize;
        private PointStyle PtStyle = PointStyle.Simple;
        public PointPreview()
          : base("PointPreview", "PtPreview",
              "对点进行预览",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PREVIEWCATATORY)
        {
          //  this.AppendAddidentMenuItem(this.con);
        }
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
           this.AppendAddidentMenuItem(menu);
           // return true;
        }
        private void AppendAddidentMenuItem(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Simple Style", ChangePointType,true,(int)this.PtStyle==0?true:false);
            Menu_AppendItem(menu, "Control Style", ChangePointType, true,(int)this.PtStyle == 1 ? true : false);
            Menu_AppendItem(menu, "Active Style", ChangePointType,true, (int)this.PtStyle == 2 ? true : false);
            Menu_AppendItem(menu, "X Style", ChangePointType,true, (int)this.PtStyle == 3 ? true : false);
        }
        private void ChangePointType(object sender, EventArgs e)
        {
            ToolStripMenuItem Menu = sender as ToolStripMenuItem;
            string Text = Menu.Text;
            
            switch (Text)
            {
                case "Simple Style":
                    this.PtStyle = PointStyle.Simple;
                    Menu.Checked = true;
                    break;
                case "Control Style":
                    this.PtStyle = PointStyle.ControlPoint;
                    Menu.Checked = true;
                    break;
                case "Active Style":
                    this.PtStyle = PointStyle.ActivePoint;
                    Menu.Checked = true;
                    break;
                case "X Style":
                    this.PtStyle = PointStyle.X;
                    Menu.Checked = true;
                    break;
                default:
                    this.PtStyle = PointStyle.Simple;
                    Menu.Checked = true;
                    break;
            }
            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }
        
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "点", GH_ParamAccess.list);
            pManager.AddColourParameter("Color", "C", "颜色", GH_ParamAccess.item,Color.Red);
            pManager.AddIntegerParameter("Size", "S", "点大小", GH_ParamAccess.item,3);

            this.OnPreviewExpired(true);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("PointStyle", (int)this.PtStyle);
            return base.Write(writer);
        }
        public override bool Read(GH_IReader reader)
        {
            this.PtStyle = (PointStyle)reader.GetInt32("PointStyle");
            return base.Read(reader);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
           
        }
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {

            GH_Colour CurrentPtColor;
            base.DrawViewportMeshes(args);
            int Size = PointSize.Value < 0 ? Math.Abs(PointSize.Value) : PointSize.Value == 0 ? 2 : PointSize.Value;
            //这行代码非常重要
            if (this.Hidden || this.Locked) return;

            if (this.Attributes.Selected) CurrentPtColor = new GH_Colour(Color.Green);
            else CurrentPtColor = PointColor;

            for (int Index = 0; Index < PointList.Count; Index++)
            {
                GH_Point PointItem = PointList[Index];
                args.Display.DrawPoint(PointItem.Value,this.PtStyle, PointSize.Value, CurrentPtColor.Value);
            }
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (PointList.Count == 0) PointList.Clear();
            if (!DA.GetDataList<GH_Point>(0, PointList)) return;
            if (!DA.GetData(1, ref PointColor)) return;
            if (!DA.GetData(2, ref PointSize)) return;

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
                Bitmap originalImg = Properties.Resources.PointPreview;
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
            get { return new Guid("5948B1EF-44AA-48D6-A6F1-33C2ED1B7EED"); }
        }
    }
}
*/