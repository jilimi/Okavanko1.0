#if Personal
using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Okavango.Hu
{
    public class Hu_SortPts : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Hu_SortPts class.
        /// </summary>
        public Hu_SortPts()
          : base("Hu_SortPts", "Hu_SortPts",
              "对点进行排序",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PERSONAL)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Ps", "待排序的点", GH_ParamAccess.list);
            pManager.AddTextParameter("String", "S", "如何排序，只能是‘x’，‘y’，‘z’", GH_ParamAccess.item,"x");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Result", "R", "已经排好序的点集", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            // 这里GH_Point是Point3d的Wrapper，不是Point的Wrapper，需要注意
            //
            //
            string Sign = "x";
            List<Point3d> Pts = new List<Point3d>();

            if (!DA.GetDataList<Point3d>(0, Pts)) return;
            if (!DA.GetData(1, ref Sign)) return;

            int SortSign = Sign.ToLower() == "x"?
                          0:Sign.ToLower() == "y"?
                          1:Sign.ToLower() == "z"?
                          2:0;

            var ResultPts = Pts.OrderByDescending(Pt => { return Pt[SortSign]; }).ToList();
            DA.SetDataList(0, ResultPts);
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
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.SortPoints;
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
            get { return new Guid("0b052334-f13a-4a1c-811f-9af6ababbc73"); }
        }
    }
}
#endif