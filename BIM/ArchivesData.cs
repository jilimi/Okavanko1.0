using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Collections;
using CSCECDEC.Okavango.Attribute;

using Grasshopper.Kernel.Data;

namespace CSCECDEC.Okavango.BIM
{
    public class ArchivesData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ArchivalDictionary class.
        /// </summary>
        public ArchivesData()
          : base("ArchivesData", "ArchivesData",
              "构建一个ArchivableDictionary",
              Setting.PLUGINNAME, Setting.BIMCATATORY)
        {
        }
        public override void CreateAttributes()
        {
            //   base.CustomAttributes(this,3);
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new Grasshopper.Kernel.Attributes.GH_ComponentAttributes(this);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Input", "I", "需要被Archives的数据", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Archives", "A", "构建好的Archives", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //动态绑定
            List<dynamic> Inputs = new List<dynamic>();
            if (!DA.GetDataList(0, Inputs)) return;
            ArchivableDictionary Archives = new ArchivableDictionary();
            for(int Index = 0; Index<Inputs.Count; Index++)
            {
                string Key = Guid.NewGuid().ToString();
                Archives.Set(Key, Inputs[Index].Value);
            }
            DA.SetData(0, Archives);
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
            get { return new Guid("4e0b6b6a-15ec-406a-8271-00b94dba495d"); }
        }
    }
}