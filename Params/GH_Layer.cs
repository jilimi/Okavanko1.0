using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;
using Rhino.DocObjects;

using CSCECDEC.Okavango.Types;
using System.Windows.Forms;
using System.Drawing;
using GH_IO.Serialization;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;
namespace CSCECDEC.Okavango.Params
{
    public class GH_Layer : GH_PersistentParam<Types.Hu_Layer>
    {
        public GH_Layer():base(new GH_InstanceDescription("Layer","L","An Component to Hold Rhino Layer", Setting.PARAMS, Setting.PLUGINNAME))
        {
            
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.Menu_AppendWireDisplay(menu);
            GH_ActiveObject.Menu_AppendSeparator(menu);
            base.Menu_AppendReverseParameter(menu);
            base.Menu_AppendFlattenParameter(menu);
            base.Menu_AppendGraftParameter(menu);
            base.Menu_AppendSimplifyParameter(menu);
            GH_ActiveObject.Menu_AppendSeparator(menu);
            base.Menu_AppendPromptOne(menu);
         //   base.Menu_AppendManageCollection(menu);
            GH_ActiveObject.Menu_AppendSeparator(menu);
            base.Menu_AppendExtractParameter(menu);
        }
        /*
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Select Layer", Do_Select_Layer);
            base.AppendAdditionalMenuItems(menu);
        }

        private void Do_Select_Layer(object sender, EventArgs e)
        {
            this.ExpireSolution(true);
        }*/
        protected override GH_GetterResult Prompt_Singular(ref Types.Hu_Layer value)
        {
            
            Forms.LayerDialog Dialog = new Forms.LayerDialog();
            Dialog.StartPosition = FormStartPosition.CenterParent;
            LayerTable LT = Rhino.RhinoDoc.ActiveDoc.Layers;
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                int LayerIndex = Dialog.LayerIndex;
                Rhino.DocObjects.Layer La = LT.FindIndex(LayerIndex);
                if(La == null)
                {
                    value = null;
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "设置图层在工作空间中不存在");

                }else
                {
                    value = new Types.Hu_Layer(La);
                }
                return GH_GetterResult.success;
            }else
            {
                return GH_GetterResult.cancel;
            }

        }
        //按需要使用，
        /*
        public override bool Write(GH_IWriter writer)
        {
            return base.Write(writer);
        }
        public override bool Read(GH_IReader reader)
        {
            return base.Read(reader);
        }*/
        protected override GH_GetterResult Prompt_Plural(ref List<Types.Hu_Layer> values)
        {
            return GH_GetterResult.cancel;
           // throw new NotImplementedException();
        }
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
        /// Initializes a new instance of the GH_Layer class.
        /// </summary>
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("878c2318-5294-41d9-98f9-2ae90c485afc"); }
        }

    }
#if Hudosn
    public class GH_Layer : GH_PersistentParam<Hu_Layer>
    {
        public GH_Layer()
          : base(new GH_InstanceDescription("Layer", "Lay", "A parameter for referencing a Rhino layer", "Params"))
        { }

        public override Guid ComponentGuid
        {
            get { return new Guid("878c2318-5294-41d9-98f9-2ae90c485afc"); }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
        protected override Hu_Layer InstantiateT()
        {
            return new Hu_Layer();
        }

        protected override GH_GetterResult Prompt_Singular(ref Hu_Layer value)
        {
            var doc = Rhino.RhinoDoc.ActiveDoc;
            // is null 属于C# 7 中的功能，Visual Studio 2015中不包含该功能
           // if (doc is null) return GH_GetterResult.cancel;
            if (doc == null) return GH_GetterResult.cancel;
            var form = new Form
            {
                Text = "Pick a layer",
                // Width = Grasshopper.Global_Proc.UiAdjust(200),
                Width = 300,
                Height = 300,
               // Height = Grasshopper.Global_Proc.UiAdjust(300),
                ShowInTaskbar = false,
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterParent
            };

            var list = new ListBox
            {
                MultiColumn = false,
                Dock = DockStyle.Fill,
                SelectionMode = SelectionMode.One
            };
            foreach (var layer in doc.Layers)
                list.Items.Add(layer);

            var button = new Button
            {
                Text = "OK",
                Height = 100,
                Dock = DockStyle.Bottom,
                DialogResult = DialogResult.OK
            };

            form.Controls.Add(list);
            form.Controls.Add(button);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var layer = list.SelectedItem as Layer;
                value = new Hu_Layer(layer);
                return GH_GetterResult.success;
            }
            else
                return GH_GetterResult.cancel;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<Hu_Layer> values)
        {
            // Same as Prompt_Singular, just allow for multiple selection.
            return GH_GetterResult.cancel;
        }
    }
#endif
}