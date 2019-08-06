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
        //声明一个委托，并将其作为构造函数参数传递到类中
        public delegate void MouseDownEventCallback(GH_Component Component);

        //实例化委托
        MouseDownEventCallback Callback;
        GH_Component Component;
        string Text;

        public RadioButtonAttribute(GH_Component owner, MouseDownEventCallback _Callback, string ButtonName) : base(owner)
        {

            this.Component = owner;
            this.Callback = _Callback;
            this.Text = ButtonName;
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
            // base.Re(canvas, graphics, true, false, false, true, true, true);
            base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {
                this.RadioRect = new RectangleF(Bounds.Left + 8, Bounds.Bottom -20, 6, 6);
                this.DrawRadio(graphics,this.RadioRect, Color.Black,this.Text, this.IsPress);
            }
        }
        public void DrawRadio(Graphics g,RectangleF Rect,Color Color,string Text, bool IsPress)
        {

            Point TextLocation = GH_Convert.ToRectangle(Rect).Location;
            TextLocation.Offset(10, -4);
            g.DrawString(Text, GH_FontServer.Standard, Brushes.Black,TextLocation);

            if (IsPress)
            {
                g.DrawEllipse(new Pen(Color, 1), Rect);
                Rect.Inflate(-4, -4);
                g.DrawEllipse(new Pen(Color, 2), Rect);
            }else
            {
                g.DrawEllipse(new Pen(Color, 1), Rect);
                Rect.Inflate(-4, -4);
                g.DrawEllipse(new Pen(Color.Gray, 2), Rect);
            }
        }
        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                if (RadioRect.Contains(e.CanvasLocation))
                {
                  //  sender.Cursor = System.Windows.Forms.Cursors.Hand;
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
                    this.Callback(this.Component);
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
