using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;

using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.GUI;

namespace CSCECDEC.Okavango.Control
{
    class ButtonControl:HuControl
    {
        public new string Text;
        public Guid id { get; }
        public IGH_Component Owner;

        public Action<IGH_Component> MouseDownCallback;
        public Action<IGH_Component> MouseUpCallback;
        public Action<IGH_Component> MouseClickCallback;

        public ButtonControl(IGH_Component Owner,string Text):base()
        {
            this.Owner = Owner;
            this.Text = Text;
            this.id = Guid.NewGuid();
        }
        public override void On_MouseDown(object sender, GH_CanvasMouseEvent e)
        {
            if(MouseDownCallback != null)
            {
                this.MouseDownCallback(this.Owner);
            }
        }
        public override void On_MouseUp(object sender, GH_CanvasMouseEvent e)
        {
            if(MouseUpCallback != null)
            {
                this.MouseUpCallback(this.Owner);
            }
        }
        public override void On_Click(object sender, GH_CanvasMouseEvent e)
        {
            if(MouseClickCallback != null)
            {
                this.MouseClickCallback(this.Owner);
            }
        }
        public override RectangleF GetBounds()
        {
            return base.Bounds;
        }
        public override void OnDraw(Graphics g)
        {
            Pen FillPen;
            if (this.IsPress) FillPen = new Pen(Setting.BUTTONPRESSCOLOR);
            else FillPen = new Pen(Setting.BUTTONUNPRESSCOLOR);
            if (this.Owner.Locked) FillPen = new Pen(Setting.COMPONENTLOCKTEXTCOLOR);
            Pen TextPen = new Pen(Setting.TEXTCOLOR);
            Pen BorderPen = new Pen(Setting.BORDERCOLOR);

            StringFormat Format = new StringFormat();
            Format.Alignment = StringAlignment.Center;
            Format.LineAlignment = StringAlignment.Center;

            GraphicsPath ButtonPath = GH_CapsuleRenderEngine.CreateRoundedRectangle(Bounds, 2);
            g.FillPath(FillPen.Brush, ButtonPath);
            g.DrawPath(BorderPen, ButtonPath);
            g.DrawString(Text, GH_FontServer.Standard, TextPen.Brush, Bounds, Format);

            Format.Dispose();
        }
    }
}
