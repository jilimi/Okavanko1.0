using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.DocObjects;
using CSCECDEC.Okavango.Params;
using CSCECDEC.Okavango.Types;

namespace CSCECDEC.Okavango.Workflow
{
    public class CreateAttribute : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateAttribute class.
        /// </summary>
        bool ColorFromLayer = true;
        public CreateAttribute()
          : base("CreateAttribute", "CreateAttribute",
              "构建物体的属性",
              Setting.PLUGINNAME, Setting.WORKFLOWCATATORY)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GH_Layer());
            pManager.AddColourParameter("Color", "Color", "物体的颜色", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Attribute", "Attribute", "物体的属性", GH_ParamAccess.item);
        }
        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Color From Layer", Do_ChangeColorSource, this.ColorFromLayer);
        }

        private void Do_ChangeColorSource(object sender, EventArgs e)
        {
            this.ColorFromLayer = !this.ColorFromLayer;
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Hu_Layer Layer = default(Hu_Layer);
            Color ObjectColor = new Color();

            if (!DA.GetData(0, ref Layer)) return;
            if (!this.ColorFromLayer)
            {
                if (!DA.GetData(1, ref ObjectColor)) return;
            }
            ObjectAttributes Attr = new ObjectAttributes();
            Attr.LayerIndex = Layer.Value.Index;
            if(this.ColorFromLayer)
            {
                Attr.ColorSource = ObjectColorSource.ColorFromLayer;
            }else
            {
                Attr.ColorSource = ObjectColorSource.ColorFromObject;
                Attr.ObjectColor = ObjectColor;
            }
            DA.SetData(0, Attr);
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
            get { return new Guid("df58b194-1f96-46fe-b147-499bf2dcbfed"); }
        }
    }
}