#if HudsonCompile
using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace CSCECDEC.Okavango.CutMaterial
{
    public class Optimization : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Optimization class.
        /// </summary>
        public Optimization()
          : base("Optimization", "Optimization",
              "对长度进行切割优化,有局部最优解和全局最优解",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.CUTDOWNCATATORY)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("DataList", "D", "待优化的原始长度数据", GH_ParamAccess.list);
            pManager.AddTextParameter("Data", "D", "定尺数据,数据与数据之间用逗号隔开", GH_ParamAccess.item);
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
        //
        private List<double> TODO()
        {
            throw new NotImplementedException();
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
            get { return new Guid("f30b1aa6-ddf0-450b-afdd-1411b89add98"); }
        }
    }
}
# endif