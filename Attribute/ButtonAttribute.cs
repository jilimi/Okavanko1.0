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
    public class ButtonAttribute : GH_ComponentAttributes
    {
        RectangleF ButtonRect;
        Color ButtonColor = Color.Black;

        //声明一个委托，并将其作为构造函数参数传递到类中
        public delegate void MouseDownEventCallback(GH_Component Component);

        //实例化委托
        MouseDownEventCallback Callback;
        GH_Component Component;
        string Text;

        public ButtonAttribute(GH_Component owner, MouseDownEventCallback _Callback, string ButtonName) : base(owner) {

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
            Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height+24);
        }

        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            // base.Re(canvas, graphics, true, false, false, true, true, true);
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule ButtonBox = GH_Capsule.CreateCapsule(new RectangleF(Bounds.Left, Bounds.Bottom, Bounds.Width - 6, 40), GH_Palette.Blue, new int[] { 1, 1, 1, 1 }, 2);
                this.ButtonRect = new RectangleF(Bounds.Left + 3, Bounds.Bottom - 24, Bounds.Width - 6, 18);
                GH_Capsule TextBox = GH_Capsule.CreateTextCapsule(this.ButtonRect, this.ButtonRect, GH_Palette.Grey, this.Text);
                TextBox.Render(graphics, this.ButtonColor);
            }
            #region
            /*
            capsule.Palette = GH_Palette.Transparent;
                base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {
                GH_PaletteStyle _Style,Style;
                // capsule.Render(graphics, Style);

                // capsule.AddInputGrip(this.InputGrip.Y);
                //  capsule.AddOutputGrip(this.OutputGrip.Y);

               // if (this.Selected || this.Owner.Locked) _Style = new GH_PaletteStyle(Color.Transparent, Color.Green);
               // else _Style = new GH_PaletteStyle(Color.Transparent, Color.Gray);

               // if(Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error) Style = new GH_PaletteStyle(Color.Transparent, Color.Red);
               // else if(Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning) Style = new GH_PaletteStyle(Color.Transparent, Color.Yellow);
              //  else Style = new GH_PaletteStyle(Color.Transparent, Color.Gray);

               // Rectangle OutterRect = GH_Convert.ToRectangle(Bounds);

              //  GH_CapsuleRenderEngine.RenderInputGrip(graphics, canvas.Viewport.Zoom, this.Owner.Attributes.InputGrip,false);
               // this.RenderIncomingWires(new GH_Painter(canvas), this.Owner.Params, GH_ParamWireDisplay.@default);
              //  graphics.DrawRectangle(new Pen(_Style.Edge, 1), new Rectangle(OutterRect.X + 2, OutterRect.Y + 2, OutterRect.Width - 4, OutterRect.Height - 4));
              //  graphics.DrawRectangle(new Pen(Style.Edge, 1), GH_Convert.ToRectangle(Bounds));
              */
            #endregion
        }
        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {

            if (!Owner.Locked && e.Clicks == 2 && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (ButtonRect.Contains(e.CanvasLocation)){
                    this.Callback(this.Component);
                }    
            }
            return base.RespondToMouseDoubleClick(sender, e);
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                if (ButtonRect.Contains(e.CanvasLocation))
                {
                    this.ButtonColor = Color.LightGray;
                    sender.Refresh();
                }
            }
            return base.RespondToMouseDown(sender, e);
        }
        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                this.ButtonColor = Color.Gray;
                sender.Refresh();
            }
            return base.RespondToMouseUp(sender, e);
        }
    }
}
