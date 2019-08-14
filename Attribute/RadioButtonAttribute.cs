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
using Grasshopper.GUI;
using System.Windows.Forms;

namespace CSCECDEC.Okavango.Attribute
{
    class RadioButtonAttribute : GH_ComponentAttributes
    {
        RectangleF ButtonRectF;
        public Hu_AttributeUtil AttributeUtil;
        GH_Component Component;
        List<RadioButtonControl> RadioButtonList = new List<RadioButtonControl>();
        RadioButtonControl PressRadioButton = null;
        //构造函数不能使用obsolete方法
        public RadioButtonAttribute(GH_Component owner, List<RadioButtonControl> RadioButtonList) : base(owner)
        {

            this.Component = owner;
            this.RadioButtonList = RadioButtonList;
            this.AttributeUtil = new Hu_AttributeUtil(this.Owner);
        }
        private void LayoutRadioButton()
        {
            for (int Index = 0; Index < RadioButtonList.Count; Index++)
            {
                float Bottom = this.Bounds.Bottom;
                float Left = this.Bounds.Left;

                float Height = 18;
                float Width = 18;
                RadioButtonList[Index].Bounds = new RectangleF(Left, Bottom - 24 * (this.RadioButtonList.Count - Index), Width, Height);
            }
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
        }
        protected override void Layout()
        {
            base.Layout();
            AttributeUtil.ComputeLayout(24 * this.RadioButtonList.Count);
            this.LayoutRadioButton();
        }
        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Wires)
            {
                AttributeUtil.RenderBounds(graphics);
                base.Render(canvas, graphics, channel);
            }
            if (channel == GH_CanvasChannel.Objects)
            {
                this.ButtonRectF = new RectangleF(Bounds.Left, Bounds.Bottom - 24, Bounds.Width, 18);
                AttributeUtil.CompoundRender(graphics, canvas);
                this.RadioButtonList.ForEach(item =>
                {
                    item.OnDraw(graphics);
                });
            }
        }
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
