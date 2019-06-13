#if Personal
using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.Hu
{
    public class Hu_SortCrv : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Hu_SortCrv class.
        /// </summary>
        public Hu_SortCrv()
          : base("Hu_SortCrv", "Hu_SortCrv",
              "Sort Curve By Axis Component Or Length",
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
            pManager.AddCurveParameter("Crvs", "C", "Curve Object", GH_ParamAccess.list);
            pManager.AddTextParameter("Sign", "S", "如何排序，只能是‘x’，‘y’，‘z’,‘l’(按长度进行排序)", GH_ParamAccess.item,",");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Result", "S", "已经排好序的曲线", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Sign = "x";
            List<Curve> Crvs = new List<Curve>();
            List<Curve> OutputCrv = new List<Curve>();

            if (!DA.GetDataList<Curve>(0, Crvs)) return;
            if (!DA.GetData(1, ref Sign)) return;

            int SortSign = Sign.ToLower() == "x" ?
                         0 : Sign.ToLower() == "y" ?
                         1 : Sign.ToLower() == "z" ?
                         2 : Sign.ToLower() == "l"?
                         -1:0;

            if (SortSign != -1)
            {
                OutputCrv = Crvs.OrderByDescending(item => { Point3d CPt = (item.PointAtStart+item.PointAtEnd)/2;return CPt[SortSign]; }).ToList();
            }else
            {
                OutputCrv = Crvs.OrderByDescending(item => { return item.GetLength(); }).ToList();
            }
            DA.SetDataList(0, OutputCrv);
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
                Bitmap originalImg = Properties.Resources.SortCrvByAxis;
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
            get { return new Guid("daf2a6dd-261b-40ed-9f17-807589e45f28"); }
        }
    }
}
#endif