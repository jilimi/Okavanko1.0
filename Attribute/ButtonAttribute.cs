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
    class ButtonAttribute : GH_ComponentAttributes
    {
        RectangleF ButtonRectF;
        public Hu_AttributeUtil AttributeUtil;
        //声明一个委托，并将其作为构造函数参数传递到类中
        //实例化委托
        GH_Component Component;
        //
        List<ButtonControl> ButtonList = new List<ButtonControl>();
        ButtonControl Button = null;
        ButtonControl PressButton = null;
        //构造函数不能使用obsolete方法
        public ButtonAttribute(GH_Component owner, List<ButtonControl> ButtonList) : base(owner) {

            this.Component = owner;
            this.ButtonList = ButtonList;
            this.AttributeUtil = new Hu_AttributeUtil(this.Owner);
        }
        private void LayoutButton()
        {
            for(int Index = 0; Index < ButtonList.Count; Index++)
            {
                float Bottom = this.Bounds.Bottom;
                float Left = this.Bounds.Left;

                float Height = 18;
                float Width = this.Bounds.Width;

                ButtonList[Index].Bounds = new RectangleF(Left, Bottom - 24 *(this.ButtonList.Count- Index), Width, Height);
            }
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
        }
        protected override void Layout()
        {
            base.Layout();
            AttributeUtil.ComputeLayout(24*this.ButtonList.Count);
            this.LayoutButton();
        }
        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            if(channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }
            if (channel == GH_CanvasChannel.Objects)
            {
                this.ButtonRectF = new RectangleF(Bounds.Left, Bounds.Bottom - 24, Bounds.Width, 18);
                AttributeUtil.RenderBounds(graphics);
                //ComponentRender is use to render grip point parameter name and componentName
                AttributeUtil.CompoundRender(graphics, canvas);
                this.ButtonList.ForEach(item =>
                {
                    item.OnDraw(graphics);
                });
            }
        }
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
        //sender其实就是canvas
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
