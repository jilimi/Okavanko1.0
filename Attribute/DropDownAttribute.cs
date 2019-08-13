using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace CSCECDEC.Plugin.Attribute
{
    class DropDownAttribute:GH_ComponentAttributes
    {
        Hu_AttributeUtil AttributeUtil;
        Action<IGH_Component> Callback;
        IGH_Component Component;
        string Text;

        public DropDownAttribute(GH_Component owner,Action<IGH_Component> Callback,string Text) : base(owner)
        {

            this.Component = owner;
            this.Callback = Callback;
            this.Text = Text;
            this.AttributeUtil = new Hu_AttributeUtil(this.Owner);
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
        }
        protected override void Layout()
        {
            base.Layout();
            Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width + 10, Bounds.Height);
            RectangleF rect = Owner.Params.Output[0].Attributes.Bounds;
            Owner.Params.Output[0].Attributes.Bounds = new RectangleF(rect.X, rect.Y, rect.Width + 10, rect.Height);
           // this.AttributeUtil.ComputeLayout(0)
        }
        private void DrawTrignale(Graphics g,RectangleF RectF)
        {
            Rectangle Rect = GH_Convert.ToRectangle(RectF);
            g.DrawLines(new Pen(Color.Black, 2), new Point[]{ new Point(Rect.X,Rect.Y),new Point(Rect.X+Rect.Width,Rect.Y),new Point(Rect.X+Convert.ToInt32(Rect.Width*0.5),Rect.Y+Rect.Height)});
        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            GH_Capsule Capsule = GH_Capsule.CreateCapsule(this.Bounds, GH_Palette.Normal);

          //  Capsule.AddInputGrip(Bounds.X, Bounds.Y + Bounds.Height / 2);
         //   Capsule.AddOutputGrip(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height / 2);

            if(channel == GH_CanvasChannel.Wires)
            {
                this.RenderComponentCapsule(canvas, graphics);
            }
            if(channel == GH_CanvasChannel.Objects)
            {
                this.DrawTrignale(graphics, new RectangleF(Bounds.X + Bounds.Width - 10, Bounds.Y - 1, Bounds.Width - 3, Bounds.Height - 2));
                base.RenderComponentCapsule(canvas, graphics, true, true, true, true, true, true);
                //base.Render(canvas, graphics, channel);
            }
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if(e.Button == MouseButtons.Right && !this.Owner.Locked)
            {
                ContextMenu RightMenu = new ContextMenu();
                RightMenu.MenuItems.Add(new MenuItem("Hello World", Do_Click));
                RightMenu.Show(sender, GH_Convert.ToPoint(e.CanvasLocation));  
            }
            return base.RespondToMouseDown(sender, e);
        }

        private void Do_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Hello World");
        }

        public void CreateContextMenu()
        {

        }
    }
}
