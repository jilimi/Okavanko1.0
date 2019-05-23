using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.Hu
{
    public class Hu_SortSrfByAxis : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Hu_SortSrfByAxis class.
        /// </summary>
        public Hu_SortSrfByAxis()
          : base("Hu_SortSrfByAxis", "Hu_SortSrfByAxis",
              "按坐标轴对面进行排序",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PERSONAL)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "待排序的Brep面", GH_ParamAccess.list);
            pManager.AddTextParameter("Sign", "S", "排序方法，只能取'x','y','z'", GH_ParamAccess.item,"x");

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Result", "R", "排序结果", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Brep> BrepList = new List<Brep>();
            string Text = "x";

            if (!DA.GetDataList<Brep>(0, BrepList)) return;
            if (!DA.GetData(0, ref Text)) return;

            int IndexSign = Text.ToLower() == "x" ?
                          0 : Text.ToLower() == "y" ?
                          1 : Text.ToLower() == "z" ?
                          2 : 0;

            var TempBrep = BrepList.OrderByDescending(item => { return AreaMassProperties.Compute(item).Centroid[IndexSign]; }).ToList();

            DA.SetDataList(0, TempBrep);
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
                Bitmap originalImg = Properties.Resources.SortSrfByAxis;
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
            get { return new Guid("73e04d7f-b2f1-4a70-8c9c-c95c797b9fb0"); }
        }
    }
}