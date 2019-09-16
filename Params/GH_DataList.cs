using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;

using CSCECDEC.Okavango.Attribute;
using CSCECDEC.Okavango.Control;

namespace CSCECDEC.Okavango.Params
{
    public class GH_DataList : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_DataList class.
        /// </summary>
        List<string> MenuNames = new List<string>();
        string OutputText = "";
        public GH_DataList()
          : base("DataList", "DataList",
              "数据列表",
              Setting.PARAMS, Setting.PLUGINNAME)
        {
            this.Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            this.ExpireSolution(true);
        }

        public override void CreateAttributes()
        {
            DropDownControl DropDown = new DropDownControl(PressIn, this);
            Hu_DropDownAttribute Attr_DropDown = new Hu_DropDownAttribute(this, MenuNames,DropDown);
            Normal_DropDownAttribute Attr_DropDownNormal = new Normal_DropDownAttribute(this, MenuNames, DropDown);

            if (Properties.Settings.Default.Is_Hu_Attribute)
            {
                m_attributes = Attr_DropDown;
            }else
            {
                m_attributes = Attr_DropDownNormal;
            }
        }

        private void PressIn(ToolStripMenuItem MenuItem)
        {
            string Text = MenuItem.Text;
            string TempText = Text.Length > 8 ? Text.Substring(0, 8) + "..." : Text;
            this.NickName = TempText;
            OutputText = Text;
            this.ExpireSolution(true);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Input", "I", "输入的列表", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output", "O", "输出的列表", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_String> TempTree = new GH_Structure<GH_String>();
            List<string> TempList = new List<string>();

            if (MenuNames.Count != 0) MenuNames.Clear();
            if (!DA.GetDataTree(0, out TempTree)) return;
            TempList = TempTree.Branches[0].Select(item=>item.Value).ToList();
            for(int Index = 0; Index < TempList.Count; Index++)
            {
                this.MenuNames.Add(TempList[Index]);
            }
            DA.SetData(0, this.OutputText);
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
            get { return new Guid("3a86dc64-d3b0-4bee-9306-2e3501607e56"); }
        }
    }
}