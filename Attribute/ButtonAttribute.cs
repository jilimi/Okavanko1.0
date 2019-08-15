using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI.Canvas;
using System.Drawing;

using CSCECDEC.Okavango.Basic;
using CSCECDEC.Okavango.BIM;
using CSCECDEC.Okavango.Control;

using Grasshopper.GUI;
using System.Windows.Forms;

namespace CSCECDEC.Okavango.Attribute
{   
    class ButtonAttribute : Hu_Attribute
    {
        List<ButtonControl> ButtonList = new List<ButtonControl>();
        ButtonControl PressButton = null;

        public ButtonAttribute(GH_Component owner, List<ButtonControl> ButtonList) : base(owner) {

            this.Component = owner;
            this.ButtonList = ButtonList;
            base.InnerControlNumber = this.ButtonList.Count;
        }
        private void LayoutButton()
        {
            for(int Index = 0; Index < ButtonList.Count; Index++)
            {
                float Bottom = this.Bounds.Bottom;
                float Left = this.Bounds.Left;

                float ControlBoxHeight = Setting.COMPONENTCONTROLBOXHEIGHT;
                float ControlHeight = Setting.COMPONENTCONTROLHEIGHT;
                float ControlWidth = this.Bounds.Width;
                //Button创建的时候是不知道Bounds的Bounds的，Bounds的需要在运行时才能够知道
                ButtonList[Index].Bounds = new RectangleF(Left, Bottom - ControlBoxHeight * (base.InnerControlNumber- Index), ControlWidth, ControlHeight);
            }
        }
        protected override void Layout()
        {
            base.Layout();
            this.LayoutButton();
        }
        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {
                this.ButtonList.ForEach(item =>
                {
                    item.OnDraw(graphics);
                });
            }
        }
        //Event Process
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                List<ButtonControl> PressButtons = this.ButtonList.Where(item => item.Bounds.Contains(e.CanvasLocation)).ToList();
                if (PressButtons.Count != 0)
                {
                    this.PressButton = PressButtons[0];
                    this.PressButton.IsPress = true;
                    this.PressButton.On_MouseDown(this.Owner);
                }
            }
            return base.RespondToMouseDown(sender, e);
        }
        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (this.PressButton.IsPress)
            {
                this.PressButton.IsPress = false;
                sender.Refresh();
            }
            return base.RespondToMouseMove(sender, e);
        }
        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                if (this.PressButton.IsPress)
                {
                    this.PressButton.On_MouseUp(this.Owner);
                    this.PressButton.IsPress = false;
                    sender.Refresh();
                }
            }
            return base.RespondToMouseUp(sender, e);
        }
    }
}
