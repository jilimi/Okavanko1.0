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
using System.Windows.Forms;

namespace CSCECDEC.Plugin.Attribute
{   
    class ButtonAttribute : GH_ComponentAttributes
    {
        string Text;
        RectangleF ButtonRectF;
        public Hu_AttributeUtil AttributeUtil;
        //声明一个委托，并将其作为构造函数参数传递到类中
        //实例化委托
        GH_Component Component;
        //
        Color ButtonColor = Color.FromArgb(255, 149, 151, 153);
        List<ButtonControl> ButtonList = new List<ButtonControl>();
        ButtonControl Button = null;
        ButtonControl PressButton = null;

        public ButtonAttribute(GH_Component owner, List<ButtonControl> ButtonList , string Text) : base(owner) {

            this.Component = owner;
            this.ButtonList = ButtonList;
            this.Text = Text;
            this.AttributeUtil = new Hu_AttributeUtil(this.Owner);
        }
        [Obsolete]
        public ButtonAttribute(GH_Component owner, ButtonControl Button, string Text) : base(owner)
        {

            this.Component = owner;
            this.Button = Button;
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
            AttributeUtil.ComputeLayout(24*this.ButtonList.Count);
        }
        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            // base.Re(canvas, graphics, true, false, false, true, true, true);
            if(channel == GH_CanvasChannel.Wires)
            {
                AttributeUtil.RenderBounds(graphics);
                base.Render(canvas, graphics, channel);
            }
            if (channel == GH_CanvasChannel.Objects)
            {
                this.ButtonRectF = new RectangleF(Bounds.Left, Bounds.Bottom - 24, Bounds.Width, 18);
                AttributeUtil.CompoundRender(graphics, canvas);
                ObjectDraw.DrawButton(graphics, this.ButtonRectF, this.Text, this.ButtonColor, false);
                this.ButtonList.ForEach(item =>
                {
                    item.OnDraw(graphics);
                });
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
       public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                List<ButtonControl> PressButtons = this.ButtonList.Where(item => item.Bounds.Contains(e.CanvasLocation)).ToList();
                if (PressButtons.Count != 0)
                {
                    this.PressButton = PressButtons[0];
                    this.PressButton.IsPress = true;
                    this.PressButton.PressInCallback(this.Owner);
                }
            }
            return base.RespondToMouseDown(sender, e);
        }
        //sender其实就是canvas
        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                if (this.PressButton.IsPress)
                {
                    this.PressButton.PressOutCallback(this.Owner);
                    this.ButtonColor = Color.FromArgb(255, 149, 151, 153);
                    this.PressButton.IsPress = false;
                    sender.Refresh();
                }
            }
            return base.RespondToMouseUp(sender, e);
        }
    }
}
