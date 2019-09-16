using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Data;
using Grasshopper.GUI;

using Rhino.Geometry;
using Rhino.DocObjects.Tables;
using Rhino.DocObjects;

using CSCECDEC.Okavango.Types;
using System.Windows.Forms;
using System.Drawing;
using GH_IO.Serialization;
using CSCECDEC.Okavango.Attribute;


namespace CSCECDEC.Okavango.Params
{
    public class GH_Font : GH_PersistentParam<Types.Hu_Font>
    {
        public GH_Font() : base(new GH_InstanceDescription("Font", "F", "An Component to Hold Font Information", Setting.PARAMS, Setting.PLUGINNAME))
        {

        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
        public override void CreateAttributes()
        {
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_ParamAttribute(this);
            else m_attributes = new GH_FloatingParamAttributes(this);

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
            GH_ActiveObject.Menu_AppendSeparator(menu);
            base.Menu_AppendExtractParameter(menu);
        }
        protected override GH_GetterResult Prompt_Singular(ref Types.Hu_Font value)
        {
            System.Drawing.Font Font = GH_FontPicker.ShowFontPickerWindow(GH_FontServer.Standard);
            if (Font == null) return GH_GetterResult.cancel;
            value = new Hu_Font(Font);
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<Types.Hu_Font> values)
        {
            return GH_GetterResult.cancel;
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.GetLayer;
                Graphics graphic = Graphics.FromImage((Image)newImage);
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphic.DrawImage(originalImg, 0, 0, newImage.Width, newImage.Height);
                return newImage;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{AFADB1B1-DD68-4E02-A4A0-CA9798DEE4AB}"); }
        }
    }
}
