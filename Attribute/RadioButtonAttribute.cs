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

using CSCECDEC.Plugin.Basic;
using CSCECDEC.Plugin.BIM;
using Grasshopper.GUI;

namespace CSCECDEC.Plugin.Attribute
{
    class RadioButtonAttribute : GH_ComponentAttributes
    {
        RectangleF RadioRect;
        Color RadioColor = Color.Black;
        public bool IsPress = false;

        public Action<IGH_Component> CheckInCallback;
        GH_Component Component;
        string Text;

        public RadioButtonAttribute(GH_Component owner, Action<IGH_Component> _CheckInCallback, string Text) : base(owner)
        {

            this.Component = owner;
            this.CheckInCallback = _CheckInCallback;
            this.Text = Text;
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
        }
        protected override void Layout()
        {
            base.Layout();
            int width = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.Standard);
            Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + 30);
            
        }
        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {

            base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {
                this.RadioRect = new RectangleF(Bounds.Left + 8, Bounds.Bottom -20, 6, 6);
                ObjectDraw.DrawRadioButton(graphics,RadioRect,this.Text,this.RadioColor,this.IsPress,false);
            }
        }
        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                if (RadioRect.Contains(e.CanvasLocation))
                {
                    sender.Refresh();
                }
            }
            return base.RespondToMouseMove(sender, e);
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                if (RadioRect.Contains(e.CanvasLocation))
                {
                    this.CheckInCallback(this.Component);
                    if (!this.IsPress)
                    {
                        this.IsPress = true;
                    }else
                    {
                        this.IsPress = false;
                    }
                    sender.Refresh();
                }
            }
            return base.RespondToMouseDown(sender, e);
        }
    }
}
