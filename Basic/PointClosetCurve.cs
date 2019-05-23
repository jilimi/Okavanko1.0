using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.Basic
{
    public class PointClosetCurve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PointClosetCurve class.
        /// </summary>
        public PointClosetCurve()
          : base("PointClosetCurve", "PointClosetCurve",
              "找出距离点最近的N根曲线",
             GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "曲线集合", GH_ParamAccess.list);
            pManager.AddPointParameter("Point", "P", "坐标点", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Num", "N", "需要查找的数目", GH_ParamAccess.item,1);
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.senary;
            }
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Result", "R", "计算结果", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d Pt = default(Point3d);
            List<Curve> Crvs = new List<Curve>();
            int Num = 1;
            Dictionary<double, Curve> TempDict = new Dictionary<double, Curve>();

            if (!DA.GetDataList<Curve>(0, Crvs)) return;
            if (!DA.GetData(1,ref Pt)) return;
            if (!DA.GetData(2,ref Num)) return;

            double Param = 0;

            foreach(var Crv in Crvs)
            {
                if(Crv.ClosestPoint(Pt,out Param))
                {
                    Point3d TempPt = Crv.PointAt(Param);
                    double Dist = Pt.DistanceTo(TempPt);
                    TempDict.Add(Dist, Crv);
                }
            }
            var ResultDict = TempDict.OrderByDescending(item => -item.Key).ToDictionary(item => item.Key, item => item.Value);
            List<Curve> ResultCrvs = ResultDict.Values.ToList().GetRange(0, Num);

            DA.SetDataList(0, ResultCrvs);
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
                Bitmap originalImg = Properties.Resources.FindClosetCrvs;
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
            get { return new Guid("64040c71-fff8-4ca1-a7f1-efd435b4606c"); }
        }
    }
}