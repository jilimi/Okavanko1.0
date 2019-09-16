using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

using CSCECDEC.Okavango.Control;

namespace CSCECDEC.Okavango.Attribute
{
    class Hu_DropDownAttribute : Hu_Attribute
    {
        List<string> ContextMenuName = new List<string>();
        public DropDownControl DropDownControl;
       // public AttributeUtil AttributeUtil;
        public Hu_DropDownAttribute(GH_Component owner,List<string> MenuNames,DropDownControl DropDown) : base(owner)
        {

            this.ContextMenuName = MenuNames;
            this.DropDownControl = DropDown;
            this.Component = owner;
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
            this.DropDownControl.ConstructContextMenu(this.ContextMenuName);
        }

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            this.DropDownControl.ConstructContextMenu(this.ContextMenuName);
        }

        protected override void Layout()
        {
            int FontWidth = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.Standard);
            //10 is meanless
            float ComponentWidth = FontWidth + 2 * Setting.DROPDOWNCOMPONENTPARAMHEIGHT + Setting.DROPDOWNTRIANGLEHEIGHT+10;
            SizeF BoundsSize = new SizeF(ComponentWidth, Setting.DROPDOWNCOMPONENTHEIGHT);
            this.Owner.Attributes.Bounds = new RectangleF(Pivot,BoundsSize);
            base.AttributeUtil.ComputeFixPramGridBounds(Setting.DROPDOWNCOMPONENTPARAMHEIGHT);
        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if(channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }
            if(channel == GH_CanvasChannel.Objects)
            {
                float ParamWidth = Setting.DROPDOWNCOMPONENTPARAMHEIGHT;
                RectangleF ContentBox = new RectangleF(Bounds.Left + ParamWidth, Bounds.Top, this.Owner.Attributes.Bounds.Width - 2 * ParamWidth - Setting.DROPDOWNTRIANGLEHEIGHT - 10, Setting.DROPDOWNCOMPONENTHEIGHT);

                base.AttributeUtil.RenderComponentBounds(graphics);
                base.AttributeUtil.RenderParamsGripPoints1(graphics, canvas);
                base.AttributeUtil.DrawHorizonComponentName(ContentBox,graphics, false);

                this.DropDownControl.OnDraw(graphics);
                this.DropDownControl.DrawLine(graphics);
            }
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if(e.Button == MouseButtons.Left && !this.Owner.Locked)
            {
                RectangleF ControlRect = this.DropDownControl.GetBounds();
                if (ControlRect.Contains(e.CanvasLocation))
                {
                    this.DropDownControl.On_MouseDown(sender,e);
                }
            }
            return GH_ObjectResponse.Ignore;
        }
     }
}
