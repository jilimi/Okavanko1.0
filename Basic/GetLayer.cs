//if Hudson
using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Rhino.DocObjects;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.GUI;
using CSCECDEC.Okavango.Forms;
using CSCECDEC.Okavango.Params;
using System.Drawing;
using GH_IO.Serialization;

using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;
namespace CSCECDEC.Okavango.Basic
{
    public class GetLayer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Layer class.
        /// </summary>
        int LayerIndex = -1;
        Layer OutputLayer = null; 
        public GetLayer()
          : base("GetLayer", "GetLayer",
              "获取一个图层",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.senary;
            }
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
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
           // pManager.AddGenericParameter("Layer", "L", "图层", GH_ParamAccess.item);
            pManager.AddParameter(new GH_Layer(), "Layer", "L", "An Layer in Rhino", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            this.AppendAddidentMenuItem(menu);
            // return true;
        }
        private void AppendAddidentMenuItem(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Select Layer", Do_SelectALayer,true);
            Menu_AppendItem(menu, "Current Layer", Do_SetCurrentLayer,true);
            Menu_AppendItem(menu, "Clear Data", Do_ClearData,true);
        }

        private void Do_SetCurrentLayer(object sender, EventArgs e)
        {
            this.LayerIndex = Rhino.RhinoDoc.ActiveDoc.Layers.CurrentLayer.Index;
            this.ExpireSolution(true);
        }
        private void Do_ClearData(object sender, EventArgs e)
        {
            this.LayerIndex = -1;
            this.OutputLayer = null;
            this.ExpireSolution(true);
        }

        private void Do_SelectALayer(object sender, EventArgs e)
        {
            Forms.LayerDialog Dialog = new Forms.LayerDialog();
            Dialog.StartPosition = FormStartPosition.CenterParent;
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                this.LayerIndex = Dialog.LayerIndex;
            }
            this.ExpireSolution(true);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //不通过对话框设置
            if (this.LayerIndex == -1)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "请先选取或设置图层");
            }
            else
            {
                OutputLayer = Rhino.RhinoDoc.ActiveDoc.Layers.FindIndex(LayerIndex);
                if (OutputLayer == null)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("索引为：{0}的图层不是有效图层", LayerIndex));
                }
            }
            DA.SetData(0, OutputLayer);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("Layer", this.LayerIndex);
            return base.Write(writer);
        }
        public override bool Read(GH_IReader reader)
        {
            this.LayerIndex = reader.GetInt32("Layer");
            return base.Read(reader);
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
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.GetLayer;
                //Graphic 沒有public的構造函數，不能使用new運算符，衹能通過其他方式創建graphic
                Graphics graphic = Graphics.FromImage((Image)newImage);
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphic.DrawImage(originalImg, 0, 0, newImage.Width, newImage.Height);
                return newImage;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("4305d059-f874-4403-b854-6efa7edc37b5"); }
        }
    }
}
//#endif