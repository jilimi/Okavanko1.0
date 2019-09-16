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
    class Normal_RadioButtonAttribute : NormalAttributeWithControl
    {
        ButtonControl PressButton = null;
        public Normal_RadioButtonAttribute(GH_Component owner, List<HuControl> ControlList) : base(owner,ControlList)
        {
            //这里会自动调用基类的构造函数
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                List<HuControl> PressButtons = base.ControlList.Where(item => item.Bounds.Contains(e.CanvasLocation)).ToList();
                if (PressButtons.Count != 0)
                {
                    this.PressButton = PressButtons[0] as ButtonControl;
                    this.PressButton.IsPress = true;
                    this.PressButton.On_MouseDown(this.Owner, e);
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
                    this.PressButton.On_MouseUp(this.Owner, e);
                    this.PressButton.IsPress = false;
                    sender.Refresh();
                }
            }
            return base.RespondToMouseUp(sender, e);
        }
    }
}
