using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace CSCECDEC.Okavango.Preview
{
    public class CurvePreview : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CurvePreview class.
        /// </summary>
        private List<GH_Curve> CrvList = new List<GH_Curve>();
        private GH_Colour CrvColor;
        public GH_Integer CrvThickness;
        public CurvePreview()
          : base("CrvPreview", "CurvePreview",
              "对曲线进行预览",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PREVIEWCATATORY)
        {
        }
        protected override void BeforeSolveInstance()
        {
            this.ExpirePreview(true);
           // this.ExpireSolution(true);
           // base.BeforeSolveInstance();
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线", GH_ParamAccess.list);
            pManager.AddColourParameter("Color", "C", "颜色", GH_ParamAccess.item,Color.Red);
            pManager.AddIntegerParameter("Width", "W", "线宽", GH_ParamAccess.item,3);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
           
        }
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            GH_Colour CurrentCrvColor;
            base.DrawViewportMeshes(args);
            int Thickness = CrvThickness.Value < 0 ? Math.Abs(CrvThickness.Value) : CrvThickness.Value == 0 ? 2 : CrvThickness.Value;
            if (this.Hidden || this.Locked) return;

            if (this.Attributes.Selected) CurrentCrvColor = new GH_Colour(Color.Green);
            else CurrentCrvColor = CrvColor;

            for (int Index = 0; Index < CrvList.Count; Index++)
            {
                GH_Curve Crv = CrvList[Index];
                args.Display.DrawCurve(Crv.Value, CurrentCrvColor.Value,Thickness);
            }
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (CrvList.Count != 0) CrvList.Clear();
            if (!DA.GetDataList<GH_Curve>(0, CrvList)) return;
            if (!DA.GetData(1, ref CrvColor)) return;
            if (!DA.GetData(2, ref CrvThickness)) return;

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
                Bitmap originalImg = Properties.Resources.PreviewCurve;
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
            get { return new Guid("d440eb0e-392f-460b-88e7-6f08666b6e45"); }
        }
    }
}