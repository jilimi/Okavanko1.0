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
    class Normal_DropDownAttribute:GH_ComponentAttributes
    {
        //the size of the component
        int ParamWidth = 10;
        int TriangleSize = 10;
        List<string> ContextMenuName = new List<string>();
        public DropDownControl DropDownControl;
        //将Parameter固定为12*20的方块大小
        public Normal_DropDownAttribute(GH_Component owner,List<string> MenuNames,DropDownControl DropDown):base(owner)
        {

            this.ContextMenuName = MenuNames;
            this.DropDownControl = DropDown;
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
            this.DropDownControl.ConstructContextMenu(this.ContextMenuName);
        }
        protected override void Layout()
        {
            
            //Fixed the Component Size;
            int FontWidth = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.Standard);
            //10 is meanless
            float ComponentWidth = FontWidth + 2 * ParamWidth + TriangleSize+10;
            SizeF BoundsSize = new SizeF(ComponentWidth, Setting.DROPDOWNCOMPONENTHEIGHT);
            //Pivot非常的重要
            this.Owner.Attributes.Bounds = new RectangleF(Pivot,BoundsSize);
            this.ComputeParamGridBounds();
        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if(channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }
            if(channel == GH_CanvasChannel.Objects)
            {
                this.RenderComponentBounds(graphics);
                this.DrawComponentName(graphics, false);
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
        private void ComputeParamGridBounds()
        {
            List<IGH_Param> Inputs = Owner.Params.Input;
            List<IGH_Param> Outputs = Owner.Params.Output;

            float ParamHeight = Setting.DROPDOWNCOMPONENTHEIGHT;
            RectangleF Bounds = this.Owner.Attributes.Bounds;

            Inputs[0].Attributes.Bounds = new RectangleF(Bounds.Location, new SizeF(ParamWidth, ParamHeight));

            float O_Pos_X = Bounds.Right - ParamWidth;
            float O_Pos_Y = Bounds.Top;

            Outputs[0].Attributes.Bounds = new RectangleF(O_Pos_X, O_Pos_Y, ParamWidth, ParamHeight);
        }
        private void RenderComponentBounds(Graphics graphics)
        {
              //点击等效果到这里修改
            GH_Palette ComponentColor = GH_Palette.Normal;
            bool IsLock = false;
            bool IsSelected = false;

            if (this.Owner.Locked) IsLock = true;
            if (this.Owner.Attributes.Selected) IsSelected = true;
            if (this.Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error) ComponentColor = GH_Palette.Warning;
            if (this.Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning) ComponentColor = GH_Palette.Warning;
            if (this.Owner.Locked) ComponentColor = GH_Palette.Locked;

            RectangleF Bounds = this.Owner.Attributes.Bounds;
            GH_Capsule Capsule = GH_Capsule.CreateCapsule(Bounds, ComponentColor);

            Capsule.AddInputGrip(Bounds.Left, Bounds.Top + Setting.DROPDOWNCOMPONENTHEIGHT / 2);
            Capsule.AddOutputGrip(Bounds.Right,Bounds.Top + Setting.DROPDOWNCOMPONENTHEIGHT / 2);

            Capsule.RenderEngine.RenderGrips(graphics);
            Capsule.Render(graphics, IsSelected, IsLock, false);
            Capsule.Dispose();
        }
        private void DrawComponentName(Graphics g, bool IsFullName)
        {
            Pen _Pen = new Pen(Color.Black, 1);
            Brush _Brush = _Pen.Brush;
            string Text = IsFullName ? this.Owner.Name : this.Owner.NickName;
            // 12+2
            //6 is the gape between triangle and contentbox
            RectangleF ContentBox = new RectangleF(Bounds.Left+ParamWidth,Bounds.Top,this.Owner.Attributes.Bounds.Width-2*ParamWidth- TriangleSize-10, Setting.DROPDOWNCOMPONENTHEIGHT);

            StringFormat Format = new StringFormat();
            Format.LineAlignment = StringAlignment.Center;
            Format.Alignment = StringAlignment.Center;

            g.DrawString(Text, GH_FontServer.Standard, _Brush, ContentBox, Format);
            Format.Dispose();
        }
    }
}
