using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Collections;

using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.BIM
{
    public class UnArchives : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the UnArchives class.
        /// </summary>
        public UnArchives()
          : base("UnArchivesData", "UnArchivesData",
              "对Archives执行UnArchives操作",
              Setting.PLUGINNAME, Setting.BIMCATATORY)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Archives", "A", "Archives对象", GH_ParamAccess.item);
        }
        public override void CreateAttributes()
        {
            //   base.CustomAttributes(this,3);
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new Grasshopper.Kernel.Attributes.GH_ComponentAttributes(this);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "解压缩后的数据", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ArchivableDictionary Archives = new ArchivableDictionary();

            if (!DA.GetData(0, ref Archives)) return;
            DA.SetDataList(0, Archives.Values);
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
            get { return new Guid("ba97bdc8-f300-4c7b-ae8c-858a2229e2d5"); }
        }
    }
}