using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace CSCECDEC.Plugin.Preview
{
    public class CurveDirPreview : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CurvePreview class.
        /// </summary>
        private List<GH_Curve> CrvList = new List<GH_Curve>();
        private GH_Colour ArrowColor;

        public CurveDirPreview()
          : base("CrvDir", "CurveDir",
              "对曲线方向进行预览",
              GrasshopperPluginInfo.PLUGINNAME, "预览")
        {
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线", GH_ParamAccess.list);
            pManager.AddColourParameter("Color", "C", "颜色", GH_ParamAccess.item, Color.Red);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

        }
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            GH_Colour CurrentArrowColor;
            base.DrawViewportMeshes(args);
            // 下面这句代码比价重要
            if (this.Hidden || this.Locked) return;
            //检测Component有没有被选中
            // this.ExpireSolution(true) 会带来较高的刷新率
            if (this.Attributes.Selected) CurrentArrowColor = new GH_Colour(Color.Green);
            else CurrentArrowColor = ArrowColor; 
            IEnumerable<Line> Lines = CrvList.Select(item => { Curve CrvItem = item.Value; return new Line(CrvItem.PointAtStart, CrvItem.PointAtEnd); });

            args.Display.DrawArrows(Lines, CurrentArrowColor.Value); 
                
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!DA.GetDataList<GH_Curve>(0, CrvList)) return;
            if (!DA.GetData(1, ref ArrowColor)) return;

            this.OnPreviewExpired(true);
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
                Bitmap originalImg = Properties.Resources.CurveDir;
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
            get { return new Guid("7F3149D6-EF3E-4D00-ABF0-457FD2ABAB71"); }
        }
    }
}