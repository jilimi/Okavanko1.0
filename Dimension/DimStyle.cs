using System;
using System.Linq;
using System.Collections.Generic;
using CSCECDEC.Okavango.Attribute;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Windows.Forms;
using Grasshopper.Kernel.Attributes;

namespace CSCECDEC.Okavango.Dimension
{
    public class DimStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public DimStyle()
          : base("DimStyle", "DimStyle",
              "获取当前系统中的标注样式,当在系统中添加完样式后，请右键刷新",
              Setting.PLUGINNAME, Setting.DIMENSIONCATATORY)
        {
        }
        public override void CreateAttributes()
        {
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new GH_ComponentAttributes(this);

        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //TODO
        }
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Reflesh", Do_Reflesh, true,false);
            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void Do_Reflesh(object sender, EventArgs e)
        {
            this.ExpireSolution(true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Styles", "S", "标注样式的名称", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.DocObjects.Tables.DimStyleTable TB = Rhino.RhinoDoc.ActiveDoc.DimStyles;
            List<String> StyleIds = TB.Select(item => item.Name).ToList();
            DA.SetDataList(0, StyleIds);
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
            get { return new Guid("a4432439-1d91-49ef-93cb-1ac503071937"); }
        }
    }
}