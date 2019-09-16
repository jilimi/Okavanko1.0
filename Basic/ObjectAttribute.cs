using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.DocObjects;

using CSCECDEC.Okavango.Params;
using CSCECDEC.Okavango.Types;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class ObjectAttribute : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ConstructAttribute class.
        /// </summary>
        public ObjectAttribute()
          : base("Attribute", "Attribute",
              "构建一个属性类",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
            
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GH_Layer(),"Layer","L","图层",GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "物体颜色", GH_ParamAccess.item);
        }
        public override void CreateAttributes()
        {
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new GH_ComponentAttributes(this);

        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
          //  pManager.AddParameter(new GH_ObjectAttribute(), "Attribute", "A", "物体的属性", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Layer Layer = default(Layer);
            Color Color = default(Color);
          //  Hu_ObjectAttribute Attr = new Hu_ObjectAttribute();

            if (!DA.GetData(0, ref Layer)) return;
            if (!DA.GetData(1, ref Color)) return;

          //  Attr = new Hu_ObjectAttribute(Layer, Color);

          //  DA.SetData(0, Attr);
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
            get { return new Guid("c10f2988-1a8e-4cf3-92be-6486413939b8"); }
        }
    }
}