#if Hudson
using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace CSCECDEC.Okavango.Basic
{
    public class RadiDimention : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RadiDimention class.
        /// </summary>
        public RadiDimention()
          : base("RadiDimention", "RadiDimention",
              "对弧长进行标注，该标注可以与CAD中的弧长标注进行无缝对接",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
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
            get { return new Guid("7b3126ea-4cb4-47f4-9a64-9ac481de0167"); }
        }
    }
}
#endif