using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Grasshopper.Kernel.Attributes;
using CSCECDEC.Plugin.Attribute;

namespace CSCECDEC.Plugin.Basic
{
    public class DropDown : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DropDown class.
        /// </summary>
        public DropDown()
          : base("DropDown", "DropDown",
              "下拉列表",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override void CreateAttributes()
        {
            this.m_attributes = new DropDownAttribute(this);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Sign", "S", "聚类的标志，该数值只能输入x（x 轴）,y（y轴）,z(Z 轴),d(距离)等四个数值，输入其他数值则默认为d", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Sign", "S", "聚类的标志，该数值只能输入x（x 轴）,y（y轴）,z(Z 轴),d(距离)等四个数值，输入其他数值则默认为d", GH_ParamAccess.item);
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
            get { return new Guid("28ae08c1-c432-4bc3-a439-d70a41375fe2"); }
        }
    }
}