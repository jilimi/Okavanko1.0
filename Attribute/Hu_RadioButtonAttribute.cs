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
    class Hu_RadioButtonAttribute : Hu_Attribute
    {
        List<RadioButtonControl> RadioButtonList = new List<RadioButtonControl>();
        RadioButtonControl RadioButton = null;
        private int ControlNumber = 0;
        //构造函数不能使用obsolete方法
        public Hu_RadioButtonAttribute(GH_Component owner, List<RadioButtonControl> RadioButtonList) : base(owner)
        {
            this.Component = owner;
            this.RadioButtonList = RadioButtonList;
            this.ControlNumber = RadioButtonList.Count;

            this.LayoutRadioButton();
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

                RadioButtonList[Index].Bounds = new RectangleF(Left, Bottom - ControlBoxHeight * (this.ControlNumber - Index), ControlWidth, ControlHeight);
            }
        }
        protected override void Layout()
        {
            base.Layout();
            float Extend_Height = this.ControlNumber * Setting.COMPONENTCONTROLBOXHEIGHT+8;
            AttributeUtil.ComputeLayout(0, Extend_Height);
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
                List<RadioButtonControl> RadioButtons = this.RadioButtonList.Where(item => item.Bounds.Contains(e.CanvasLocation)).ToList();
                if (RadioButtons.Count != 0)
                {
                    this.RadioButton = RadioButtons[0];
                    this.RadioButton.IsMouseDown = true;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }
        //sender其实就是canvas
        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                List<RadioButtonControl> RadioButtons = this.RadioButtonList.Where(item => item.Bounds.Contains(e.CanvasLocation)).ToList();
                if (RadioButtons.Count != 0)
                {
                    this.RadioButton.IsMouseDown = false;
                    if (this.RadioButton.Id.Equals(RadioButtons[0].Id)){
                        if (this.RadioButton.IsPress) this.RadioButton.IsPress = false;
                        else this.RadioButton.IsPress = true;
                    }
                    this.RadioButton.On_Click(this.Owner,e);
                }
                sender.Refresh();
            }
            return base.RespondToMouseUp(sender, e);
        }
    }
}
