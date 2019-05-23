using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace CSCECDEC.Plugin.Basic
{
    public class CrvsClosetPoint : GH_Component,IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the CrvsClosetPointBetween2Crv class.
        /// </summary>
        /// 
        private Rhino.Geometry.Point Pt1,Pt2;
        public CrvsClosetPoint()
          : base("CrvsClosetPoint", "CrvsClosetPoint",
              "求解两根线之间最短距离的最大值",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve1", "C1", "目标曲线1", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve2", "C2", "目标曲线2", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Integer", "I", "计算精度数值，10 ~100000", GH_ParamAccess.item,1000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("T1", "T1", "曲线1的T值", GH_ParamAccess.item);
            pManager.AddNumberParameter("T2", "T2", "曲线2的T值", GH_ParamAccess.item);
            pManager.AddNumberParameter("Distance", "D", "距离", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //Crv1 is Base Curve;
            //Crv2 is Dest Curve;

            Curve Crv1 = default(Curve), Crv2 = default(Curve);
            int Precise = 10000;
            double Param1 = 0, Param2 = 0, TempParam2 = 0,Distance = 0 ;

            if (!DA.GetData(0, ref Crv1))
            {
                //以下内容都是必要的，否则运行会失效
                Pt1 = null;
                Pt2 = null;
                this.ExpirePreview(true);
                return;
            }
            if (!DA.GetData(1, ref Crv2))
            {

                Pt1 = null;
                Pt2 = null;
                
                this.ExpirePreview(true);
                return;
            }
            if (!DA.GetData(2, ref Precise)) return;

            const int MIN = 10;
            const int MAX = 1000000;


            if(Crv1.Domain.Max > 1 || Crv2.Domain.Max > 1)
            {
                this.AddRuntimeMessage(Grasshopper.Kernel.GH_RuntimeMessageLevel.Error, "Curve Domain is Large than 1,You must reparameterize the cure first");
                return;
            }

            if (Precise < MIN || Precise > MAX) Precise = 10000;

            double InCrement = 1.0 / Precise;
            for (double i = 0; i < 1; i += InCrement)
            {
                Point3d Pt1 = Crv1.PointAt(i);
                Crv2.ClosestPoint(Pt1, out TempParam2);
                double d = Crv2.PointAt(TempParam2).DistanceTo(Pt1);
                if (Distance <= d)
                {
                    Distance = d;
                    Param1 = i;
                    Param2 = TempParam2;
                }
            }

            Pt1 = new Rhino.Geometry.Point(Crv1.PointAt(Param1));
            Pt2 = new Rhino.Geometry.Point(Crv2.PointAt(Param2));

            DA.SetData(0, Param1);
            DA.SetData(1, Param2);
            DA.SetData(2, Distance);

        }
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            System.Drawing.Color PtColor = System.Drawing.Color.Red;

            if (this.Attributes.Selected) PtColor = System.Drawing.Color.Green;
            args.Display.DrawPoint(Pt1.Location,PtColor);
            args.Display.DrawPoint(Pt2.Location,PtColor);
            base.DrawViewportWires(args);
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
                Bitmap originalImg = Properties.Resources.CrvClosetPt;
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
            get { return new Guid("6b6f3852-f586-4785-bfd8-a74606f41f4d"); }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Pt1 = null;
                    Pt2 = null;
                    Dispose(true);
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Pt1 = null;
            Pt2 = null; 
            Dispose(true);
        }
        #endregion
    }
}