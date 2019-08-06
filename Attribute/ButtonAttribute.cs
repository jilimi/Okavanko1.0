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
    class ButtonAttribute : GH_ComponentAttributes
    {
        string Text;
        RectangleF ButtonRectF;
        public Hu_AttributeUtil AttributeUtil;
        //声明一个委托，并将其作为构造函数参数传递到类中
        //实例化委托
        public Action<IGH_Component> Callback;
        GH_Component Component;
        Color ButtonColor = Color.Gray;
        public ButtonAttribute(GH_Component owner, Action<IGH_Component> _Callback, string ButtonName) : base(owner) {

            this.Component = owner;
            this.Callback = _Callback;
            this.Text = ButtonName;
            this.AttributeUtil = new Hu_AttributeUtil(this.Owner);
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
        }
        protected override void Layout()
        {
            base.Layout();
            AttributeUtil.ComputeLayout(24);
        }
        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            // base.Re(canvas, graphics, true, false, false, true, true, true);
            if(channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }
            if (channel == GH_CanvasChannel.Objects)
            {
                AttributeUtil.CompoundRender(graphics, canvas);
                float W_Extend = GrasshopperPluginInfo.W_EXTEND;
                this.ButtonRectF = new RectangleF(Bounds.Left, Bounds.Bottom-24, Bounds.Width, 18);
                GH_Capsule TextBox = GH_Capsule.CreateTextCapsule(ButtonRectF,ButtonRectF,GH_Palette.Normal,this.Text,GH_FontServer.Standard,GH_Orientation.horizontal_center,1,0);
                TextBox.Render(graphics, Color.Red);
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
                if (ButtonRectF.Contains(e.CanvasLocation)){
                    this.Callback(this.Owner);
                }    
            }
            return base.RespondToMouseDoubleClick(sender, e);
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (!Owner.Locked && e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                if (ButtonRectF.Contains(e.CanvasLocation))
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
