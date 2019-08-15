using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI.Canvas;
using System.Drawing;

using CSCECDEC.Okavango.Basic;
using CSCECDEC.Okavango.BIM;
using CSCECDEC.Okavango.Control;

namespace CSCECDEC.Okavango.Attribute
{
    class RadioButtonAttribute : Hu_Attribute
    {
        List<RadioButtonControl> RadioButtonList = new List<RadioButtonControl>();
        RadioButtonControl PressRadioButton = null;
        //构造函数不能使用obsolete方法
        public RadioButtonAttribute(GH_Component owner, List<RadioButtonControl> RadioButtonList) : base(owner)
        {
            this.Component = owner;
            this.RadioButtonList = RadioButtonList;
            base.InnerControlNumber = RadioButtonList.Count;
        }
        private void LayoutRadioButton()
        {
            for (int Index = 0; Index < RadioButtonList.Count; Index++)
            {
                float Bottom = this.Bounds.Bottom;
                float Left = this.Bounds.Left;

                float ControlBoxHeight = Setting.COMPONENTCONTROLBOXHEIGHT;
                float ControlHeight = Setting.COMPONENTCONTROLHEIGHT;
                float ControlWidth = this.Bounds.Width;

                RadioButtonList[Index].Bounds = new RectangleF(Left, Bottom - ControlBoxHeight * (base.InnerControlNumber - Index), ControlWidth, ControlHeight);
            }
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
        }
        protected override void Layout()
        {
            base.Layout();
            this.LayoutRadioButton();
        }
        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {

            base.Render(canvas, graphics, channel); 
            if (channel == GH_CanvasChannel.Objects)
            {
                this.RadioButtonList.ForEach(item =>
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
                List<RadioButtonControl> PressRadioButtons = this.RadioButtonList.Where(item => item.Bounds.Contains(e.CanvasLocation)).ToList();
                if (PressRadioButtons.Count != 0)
                {
                    this.PressRadioButton = PressRadioButtons[0];
                    this.PressRadioButton.IsMouseDown = true;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }
        //sender其实就是canvas
        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                List<RadioButtonControl> PressRadioButtons = this.RadioButtonList.Where(item => item.Bounds.Contains(e.CanvasLocation)).ToList();
                if (PressRadioButtons.Count != 0)
                {
                    this.PressRadioButton.IsMouseDown = false;
                    if (this.PressRadioButton.Id.Equals(PressRadioButtons[0].Id)){
                        if (this.PressRadioButton.IsPress) this.PressRadioButton.IsPress = false;
                        else this.PressRadioButton.IsPress = true;
                    }
                    this.PressRadioButton.On_Click(this.Owner);
                }
                sender.Refresh();
            }
            return base.RespondToMouseUp(sender, e);
        }
    }
}
