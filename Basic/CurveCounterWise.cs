using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace CSCECDEC.Okavango.Basic
{
    public class CurveCounterWise : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CurveCounterWise class.
        /// </summary>
        public CurveCounterWise()
          : base("CurveCounterWise", "CurveCounterWise",
              "检测闭合曲线的朝向，0表示为止，1表示顺时针，-1表示逆时针",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
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
            pManager.AddCurveParameter("Curve", "C", "闭合曲线", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "工作平面", GH_ParamAccess.item,Plane.WorldXY);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Dir", "D", "0表示为止，1表示顺时针，-1表示逆时针", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve Crv = default(Curve);
            Plane Pl = default(Plane);

            if (!DA.GetData(0, ref Crv)) return;
            if (!DA.GetData(1, ref Pl)) return;

            if (!Crv.IsClosed)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Curve 为未闭合的曲线");
                return;
            }

            int Orientation = -(int)Crv.ClosedCurveOrientation(Pl);

            DA.SetData(0, Orientation);

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
                Bitmap originalImg = Properties.Resources.CrvCounterWise;
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
            get { return new Guid("f7f9bde6-5373-4661-8205-1991cbfcc20f"); }
        }
    }
}