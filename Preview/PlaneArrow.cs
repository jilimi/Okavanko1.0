using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace CSCECDEC.Plugin.Preview
{
    public class PlaneArrow : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PlaneArrow class.
        /// </summary>
        public PlaneArrow()
          : base("PlaneArrow", "PlaneArrow",
              "显示工作平面的坐标轴",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PREVIEWCATATORY)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
            get { return new Guid("02b6308d-d515-4cc6-a56e-87b8fd3ddef6"); }
        }
    }
}