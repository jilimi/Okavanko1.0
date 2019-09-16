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
    class Hu_AttributeWithControl : Hu_Attribute
    {
        List<HuControl> ControlList = new List<HuControl>();
        private int ControlNumber = 0;
        HuControl PressControl;
        Timer _timer;
        private int SecondCounter = 0;

        public Hu_AttributeWithControl(GH_Component owner, List<HuControl> ButtonList) : base(owner) {

            this.Component = owner;
            this.ControlList = ButtonList;
            this.ControlNumber = this.ControlList.Count;
            SetUpTimer();
        }
        private void SetUpTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += _timer_Tick;
            _timer.Enabled = true;
            _timer.Start();
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            if (SecondCounter >= int.MaxValue) SecondCounter = 0;
            SecondCounter++;
        }

        private void LayoutControl()
        {
            for(int Index = 0; Index < ControlList.Count; Index++)
            {
                float Bottom = this.Bounds.Bottom;
                float Left = this.Bounds.Left;

                bool IsRadioBox = false;

                if (ControlList[Index].GetType() == typeof(RadioButtonControl))
                {
                    IsRadioBox = true;
                }
                float ControlBoxHeight = Setting.COMPONENTCONTROLBOXHEIGHT;
                float ControlHeight = Setting.COMPONENTCONTROLHEIGHT;
                float ControlWidth = !IsRadioBox ? this.Bounds.Width-8: Setting.COMPONENTCONTROLHEIGHT;
                //Button创建的时候是不知道Bounds的Bounds的，Bounds的需要在运行时才能够知道
                ControlList[Index].Bounds = new RectangleF(Left, Bottom - ControlBoxHeight * (this.ControlNumber - Index), ControlWidth, ControlHeight);
            }
        }
        protected override void Layout()
        {
            base.Layout();
            float Extend_Height = this.ControlNumber * Setting.COMPONENTCONTROLBOXHEIGHT+Setting.HEIGHTFIXED;
            AttributeUtil.ComputeLayout(0, Extend_Height);
            this.LayoutControl();
        }
        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {
                this.ControlList.ForEach(item =>
                {
                    item.OnDraw(graphics);
                });
            }
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                List<HuControl> PressControls = this.ControlList.Where(item => item.Bounds.Contains(e.CanvasLocation)).ToList();
                if (PressControls.Count != 0)
                {
                    this.PressControl = PressControls[0];
                    this.PressControl.IsPress = true;
                    this.PressControl.On_MouseDown(this.PressControl,e);
                    SecondCounter = 0;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }
        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                //同一个地方按下，同一个地方释放
                if (this.PressControl.IsPress)
                {
                    this.PressControl.IsPress = false;
                    this.PressControl.IsClick = !this.PressControl.IsClick;
                    if (SecondCounter < 100)
                    {
                        this.PressControl.On_Click(this.PressControl,e);
                    }else
                    {
                        this.PressControl.On_MouseUp(this.PressControl,e);
                    }
                    sender.Refresh();
                }
            }
            return base.RespondToMouseUp(sender, e);
        }
    }
}
