using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

using Grasshopper;

namespace CSCECDEC.Okavango.Attribute
{
    class DropDownAttribute:GH_ComponentAttributes
    {
        Hu_AttributeUtil AttributeUtil;
        //
        IGH_Component Component;
        string Text;
        int ParamWidth = 12;
        int ComponentHeight = 20;
        int ComponentWidth = 0;
        int TriangleSize = 10;
        List<string> ContextMenuName = new List<string>();
        RectangleF TrangleRect;
        ToolStripDropDown MainMenu = null;

        string CurrentText = "";

        //将Parameter固定为12*20的方块大小
        public DropDownAttribute(GH_Component owner,List<string> MenuNames) : base(owner)
        {

            this.Component = owner;
            this.ContextMenuName = MenuNames;
            this.AttributeUtil = new Hu_AttributeUtil(owner);
            //this.Text = Text;
        }
        public void ConstructContextMenu(List<string> MenuNames)
        {

            this.MainMenu = new ToolStripDropDown();
            MainMenu.Width = 245;
            MainMenu.ForeColor = Color.Coral;
            for(int Index = 0; Index < MenuNames.Count; Index++)
            {
                ToolStripMenuItem Item = new ToolStripMenuItem(MenuNames[Index], null, OnClick);
                MainMenu.Items.Add(Item);

            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            this.CurrentText = item.Text;
            this.Owner.NickName = this.CurrentText;
            Rhino.RhinoApp.WriteLine(this.CurrentText);
        }

        public override void ExpireLayout()
        {
            base.ExpireLayout();
            this.ConstructContextMenu(this.ContextMenuName);
        }
        protected override void Layout()
        {
            //Fixed the Component Size;
            int FontWidth = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.Standard);
            //4 is meanless
            this.ComponentWidth = FontWidth + 2 * ParamWidth + TriangleSize+10;
            SizeF BoundsSize = new SizeF(ComponentWidth, ComponentHeight);
            //Pivot非常的重要
            this.Component.Attributes.Bounds = new RectangleF(Pivot,BoundsSize);
            this.ComputeParamGridBounds();
        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if(channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }
            if(channel == GH_CanvasChannel.Objects)
            {
                this.RenderBounds(graphics);
                this.DrawTriangle(graphics);
                this.CompoundRender(graphics,canvas);
            }
        }
        private void CompoundRender(Graphics g, GH_Canvas canvas)
        {
            this.RenderParamsGripPoints(g, canvas);
            if (Grasshopper.CentralSettings.CanvasObjectIcons)
            {
                this.DrawComponentName(g, false);
            }
            else
            {
               this.DrawComponentName(g, false);
            }
        }
        private void RenderParamsGripPoints(Graphics g, GH_Canvas canvas)
        {
            //render input params icon
            int InputCount = this.Component.Params.Input.Count;
            int OutputCount = this.Component.Params.Output.Count;

            List<IGH_Param> InputParam = this.Component.Params.Input;
            List<IGH_Param> OutputParam = this.Component.Params.Output;

            if (InputCount != 1 || OutputCount != 1)
            {
                System.Windows.Forms.MessageBox.Show("无法创建 DropDown Attribute");
                return;
            }
            // OK
            RectangleF Input_ParamBounds = new RectangleF(Owner.Attributes.Bounds.Location,new SizeF(ParamWidth, 20));
            PointF Input_GridLocation = new PointF(Input_ParamBounds.X, Input_ParamBounds.Y + Input_ParamBounds.Height / 2);
            GH_CapsuleRenderEngine.RenderInputGrip(g, canvas.Viewport.Zoom, Input_GridLocation, true);

            RectangleF Output_ParamBounds = OutputParam[0].Attributes.Bounds;
            PointF Output_GridLocation = new PointF(Output_ParamBounds.X + ParamWidth, Output_ParamBounds.Y + ComponentHeight / 2);
            GH_CapsuleRenderEngine.RenderInputGrip(g, canvas.Viewport.Zoom, Output_GridLocation, true);
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if(e.Button == MouseButtons.Left && !this.Owner.Locked)
            {
                if (this.TrangleRect.Contains(e.CanvasLocation))
                {
                    this.MainMenu.Show(sender,GH_Convert.ToPoint(e.ControlLocation));
                }
               // ContextMenu RightMenu = new ContextMenu();
            }
            return GH_ObjectResponse.Ignore;
        }
        //对Grid进行重构
        private void ComputeParamGridBounds()
        {
            List<IGH_Param> Inputs = Component.Params.Input;
            List<IGH_Param> Outputs = Component.Params.Output;

            int ParamHeight = ComponentHeight;
            RectangleF Bounds = this.Component.Attributes.Bounds;

            int InputCount = Inputs.Count;
            int OutputCount = Outputs.Count;

            if (InputCount != 1 || OutputCount != 1)
            {
                System.Windows.Forms.MessageBox.Show("无法创建 DropDown Attribute");
                return;
            }
            Inputs[0].Attributes.Bounds = new RectangleF(Bounds.Location, new SizeF(ParamWidth, ParamHeight));

            float O_Pos_X = Bounds.Right-ParamWidth;
            float O_Pos_Y = Bounds.Top;

            Outputs[0].Attributes.Bounds = new RectangleF(O_Pos_X, O_Pos_Y, ParamWidth, ParamHeight);
        }
        private void RenderBounds(Graphics g)
        {
            //Error Red
            //Warning Yellow
            //Select Green
            //Locked Dark
            //Normal Grey

            Color Palette = Color.FromArgb(255, 209, 212, 214);

            if (this.Component.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error) Palette = Color.FromArgb(255, 239, 62, 71);
            if (this.Component.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning) Palette = Color.FromArgb(255, 252, 228, 76);
            if (this.Component.Attributes.Selected) Palette = Color.FromArgb(255, 46, 186, 62);
            if (this.Component.Locked) Palette = Color.FromArgb(255, 157, 159, 161);

            RectangleF Bounds = this.Component.Attributes.Bounds;

            if (this.Owner.Params.Input.Count != 1 || this.Owner.Params.Output.Count != 1)
            {
                System.Windows.Forms.MessageBox.Show("无法创建 DropDown Attribute");
                return;
            }

            float Pos_X = Bounds.Left - GrasshopperPluginInfo.W_EXTEND / 2;
            SizeF SizeF = new SizeF(this.ComponentWidth+ GrasshopperPluginInfo.W_EXTEND, ComponentHeight);
            RectangleF RectF = new RectangleF(new PointF(Pos_X, Bounds.Y), SizeF);

            GraphicsPath Path = GH_CapsuleRenderEngine.CreateJaggedRectangle(RectF, 2, 2, 2, 2, false, false);
            g.DrawPath(new Pen(Color.Black), Path);
            SolidBrush Brush = new SolidBrush(Palette);
            g.FillPath(Brush, Path);

        }
        private void DrawTriangle(Graphics g)
        {
            RectangleF OutputBounds = this.Component.Params.Output[0].Attributes.Bounds;

            PointF Pt1 = new PointF(OutputBounds.Location.X,OutputBounds.Location.Y+5);
            PointF Pt2 = new PointF(Pt1.X - 10, Pt1.Y);
            PointF Pt3 = new PointF(Pt1.X - 5, Pt1.Y + 10);

            float Rect_X = OutputBounds.Location.X - 10;
            float Rect_Y = OutputBounds.Location.Y + 5;

            this.TrangleRect = new RectangleF(Rect_X, Rect_Y, TriangleSize, TriangleSize);

            Pen Pen = new Pen(GrasshopperPluginInfo.BORDERCOLOR);

            PointF[] Pts = new PointF[] { Pt1, Pt2, Pt3, Pt1 };
            g.DrawPolygon(new Pen(Color.Black,2), Pts);
            g.FillPolygon(Pen.Brush, Pts, FillMode.Alternate);

            PointF Line_Pt1 = new PointF(Pt1.X - 10 - 5, Pt1.Y - 4);
            PointF Line_Pt2 = new PointF(Pt1.X - 10 - 5, Pt1.Y + ComponentHeight - 6);
            g.DrawLine(new Pen(GrasshopperPluginInfo.BORDERCOLOR), Line_Pt1, Line_Pt2);
        }
        private void DrawComponentName(Graphics g, bool IsFullName)
        {
            Pen _Pen = new Pen(Color.Black, 1);
            Brush _Brush = _Pen.Brush;
            string Text = IsFullName ? this.Component.Name : this.Component.NickName;
            // 12+2
            //6 is the gape between triangle and contentbox
            RectangleF ContentBox = new RectangleF(Bounds.Left+ParamWidth,Bounds.Top,this.Owner.Attributes.Bounds.Width-2*ParamWidth- TriangleSize-10, ComponentHeight);

            StringFormat Format = new StringFormat();
            Format.LineAlignment = StringAlignment.Center;
            Format.Alignment = StringAlignment.Center;

            g.DrawString(Text, GH_FontServer.Standard, _Brush, ContentBox, Format);
        }
    }
}
