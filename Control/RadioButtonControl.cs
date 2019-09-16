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
    class RadioButtonControl:HuControl
    {
        public string Text;
        public IGH_Component Owner;
        public Action<IGH_Component> ClickCallback;
        public Guid Id { get; }

        public RadioButtonControl(IGH_Component Owner,string Text) : base()
        {
            this.Text = Text;
            this.Owner = Owner;
            this.Id = Guid.NewGuid();
        }
        public override void On_Click(object sender, GH_CanvasMouseEvent e)
        {
            if(this.ClickCallback != null)
            {
                this.ClickCallback(Owner);
            }
        }
        public override RectangleF GetBounds()
        {
            return base.Bounds;
        }
        public override void OnDraw(Graphics g)
        {
            Pen FillPen,TextPen;
            if (this.IsClick) FillPen = new Pen(Setting.HOWERCOLOR);
            else FillPen = new Pen(Setting.BORDERCOLOR);
            TextPen = new Pen(Setting.TEXTCOLOR);


            StringFormat Format = new StringFormat();
            Format.Alignment = StringAlignment.Near;
            Format.LineAlignment = StringAlignment.Center;

            RectangleF TextBounds = new RectangleF(Bounds.Right, Bounds.Top, Owner.Attributes.Bounds.Width - 18, 18);
            RectangleF EllipseBounds = new RectangleF(Bounds.Left + 4, Bounds.Top + 4, Bounds.Width-8, Bounds.Height-8);
            RectangleF InnerEllipseBounds = new RectangleF(Bounds.Left + 6, Bounds.Top + 6, Bounds.Width - 12, Bounds.Height - 12);

            g.DrawString(Text, GH_FontServer.Standard, TextPen.Brush, TextBounds,Format);

            if (this.IsClick) g.FillEllipse(FillPen.Brush, InnerEllipseBounds);
            g.DrawEllipse(FillPen, EllipseBounds);
            Format.Dispose();
        }

    }
}
