using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;

using Grasshopper.Kernel.Attributes;
using System.Drawing.Drawing2D;

namespace CSCECDEC.Okavango.Attribute
{
    sealed class Hu_AttributeUtil
    {
        private IGH_Component Component;
        private float ExtendWidth = 0;
        private float ExtendHeight = 0;

        public Hu_AttributeUtil(IGH_Component Component)
        {
            this.Component = Component;
        }
        private void ComputeBounds(float ExtendWidth,float ExtendHeight)
        {
            RectangleF Bounds = this.Component.Attributes.Bounds;

            ExtendWidth = ExtendWidth <= 0 ? 0 : ExtendWidth;
            ExtendHeight = ExtendHeight <= 0 ? 0 : ExtendHeight;

            float Bound_Height = Bounds.Height + ExtendHeight;

            this.ExtendWidth = ExtendWidth;
            this.ExtendHeight = ExtendHeight;

            this.Component.Attributes.Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bound_Height);
        }
        private void ComputePramGridBounds()
        {
            List<IGH_Param> Inputs = Component.Params.Input;
            List<IGH_Param> Outputs = Component.Params.Output;
            RectangleF Bounds = this.Component.Attributes.Bounds;

            int InputCount = Inputs.Count;
            int OutputCount = Outputs.Count;

            for (int Index = 0; Index < InputCount; Index++)
            {
                float Grid_Height = (this.Component.Attributes.Bounds.Height - this.ExtendHeight) / InputCount;
                float Param_Width = this.Component.Params.InputWidth;

                float Pos_X = Bounds.Left;
                float Pos_Y = Bounds.Top + Grid_Height * Index;

                Inputs[Index].Attributes.Bounds = new RectangleF(Pos_X, Pos_Y, Param_Width, Grid_Height);
            }
            //Output Params
            for (int Index = 0; Index < OutputCount; Index++)
            {
                float Grid_Height = (this.Component.Attributes.Bounds.Height - this.ExtendHeight) / OutputCount;
                float Param_Width = this.Component.Params.OutputWidth;

                float Pos_X = Bounds.Right - Param_Width;
                float Pos_Y = Bounds.Top + Grid_Height * Index;

                Outputs[Index].Attributes.Bounds = new RectangleF(Pos_X, Pos_Y, this.Component.Params.OutputWidth, Grid_Height);
            }
        }
        public void _DrawPramGrid(Graphics g)
        {
            List<IGH_Param> Inputs = this.Component.Params.Input;
            List<IGH_Param> Outputs = this.Component.Params.Output;

            Pen _Pen = new Pen(Color.Black, 1);

            GH_ComponentAttributes Com_Attributes = this.Component.Attributes as GH_ComponentAttributes;
            g.DrawRectangle(_Pen, GH_Convert.ToRectangle(Com_Attributes.ContentBox));
            for (int Index = 0; Index < Inputs.Count; Index++)
            {
                g.DrawRectangle(_Pen, GH_Convert.ToRectangle(Inputs[Index].Attributes.Bounds));
            }
            //Output Params
            for (int Index = 0; Index < Outputs.Count; Index++)
            {
                g.DrawRectangle(_Pen, GH_Convert.ToRectangle(Outputs[Index].Attributes.Bounds));
            }
        }
        private void RenderParamsGripPoints(Graphics g,GH_Canvas canvas)
        {
            //render input params icon
            int InputCount = this.Component.Params.Input.Count;
            int OutputCount = this.Component.Params.Output.Count;

            List<IGH_Param> InputParam = this.Component.Params.Input;
            List<IGH_Param> OutputParam = this.Component.Params.Output;

            for (int Index = 0; Index < InputCount; Index++)
            {
                float Grid_Height = (this.Component.Attributes.Bounds.Height-this.ExtendHeight) / InputCount;
                RectangleF ParamBounds = InputParam[Index].Attributes.Bounds;
                PointF GridLocation = new PointF(ParamBounds.X,ParamBounds.Y+ParamBounds.Height/2);
                GH_CapsuleRenderEngine.RenderInputGrip(g, canvas.Viewport.Zoom, GridLocation, true);
            }
            for (int Index = 0; Index < OutputCount; Index++)
            {
                float Grid_Height = (this.Component.Attributes.Bounds.Height - this.ExtendHeight) / OutputCount;
                RectangleF ParamBounds = OutputParam[Index].Attributes.Bounds;
                PointF GridLocation = new PointF(ParamBounds.X+ParamBounds.Width, ParamBounds.Y + ParamBounds.Height / 2);
                GH_CapsuleRenderEngine.RenderInputGrip(g, canvas.Viewport.Zoom, GridLocation, true);
            }
        }
        private void DrawIcon(Graphics g)
        {

            RectangleF Bounds = this.Component.Attributes.Bounds;
            GH_ComponentAttributes Com_Attributes = this.Component.Attributes as GH_ComponentAttributes;
            GH_Capsule Capsule = GH_Capsule.CreateCapsule(Com_Attributes.ContentBox, GH_Palette.Normal);
          
            Bitmap Bitmap = this.Component.Locked ? this.Component.Icon_24x24_Locked : this.Component.Icon_24x24;
            Capsule.RenderEngine.RenderIcon(g, Bitmap, 0, 0);

        }
        public void ComputeLayout(float Height, float Width = GrasshopperPluginInfo.W_EXTEND)
        {
            //还需要注意这里的先后顺序
            //先计算边界，然后在计算Param的Bounds
            //否则将会渲染的不正确
            this.ComputeBounds(Width, Height);
            this.ComputePramGridBounds();
        }
        public void CompoundRender(Graphics g,GH_Canvas canvas)
        {
            this.RenderParamsGripPoints(g,canvas);
            if (Grasshopper.CentralSettings.CanvasObjectIcons)
            {
                this.DrawIcon(g);
            }
            else
            {
                if (Grasshopper.CentralSettings.CanvasFullNames) this.DrawComponentName(g, true);
                else this.DrawComponentName(g, false);
            }
            if (Grasshopper.CentralSettings.CanvasFullNames) this.RenderParamName(g, true);
            else this.RenderParamName(g, false);
        }
        //点击等效果到这里修改
        public void RenderBounds(Graphics g)
        {
            //Error Red
            //Warning Yellow
            //Select Green
            //Locked Dark
            //Normal Grey
            bool LeftJagged = false, RightJagged = false;
            Color Palette = Color.FromArgb(255, 209, 212, 214);

            if (this.Component.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error) Palette = Color.FromArgb(255, 239, 62, 71);
            if (this.Component.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning) Palette = Color.FromArgb(255, 252, 228, 76);
            if (this.Component.Attributes.Selected) Palette = Color.FromArgb(255, 46, 186, 62);
            if (this.Component.Locked) Palette = Color.FromArgb(255, 157, 159, 161);

            RectangleF Bounds = this.Component.Attributes.Bounds;
            if (this.Component.Params.Input.Count == 0) LeftJagged = true;
            if (this.Component.Params.Output.Count == 0) RightJagged = true;

            float Extend_Width = GrasshopperPluginInfo.W_EXTEND;

            float Pos_X = Bounds.X - Extend_Width / 2;
            SizeF SizeF = new SizeF(Bounds.Width+ExtendWidth, Bounds.Height); 
            RectangleF RectF = new RectangleF(new PointF(Pos_X,Bounds.Y), SizeF);

            GraphicsPath Path = GH_CapsuleRenderEngine.CreateJaggedRectangle(RectF, 2, 2, 2, 2, LeftJagged, RightJagged);
            g.DrawPath(new Pen(Color.Black), Path);

            SolidBrush Brush = new SolidBrush(Palette);
            g.FillPath(Brush, Path);

           // g.DrawRectangle(new Pen(Color.Red),GH_Convert.ToRectangle(Bounds));
        }
        private void RenderParamName(Graphics g,bool IsFullName)
        {

           
            Pen _Pen = new Pen(Color.Black, 1);
            Brush _Brush = _Pen.Brush;

            List<IGH_Param> InputParam = this.Component.Params.Input;
            List<IGH_Param> OutputParam = this.Component.Params.Output;

            int InputCount = InputParam.Count;
            int OutputCount = OutputParam.Count;

            //可以实现垂直居中
            StringFormat InputFormat = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            InputFormat.LineAlignment = StringAlignment.Center;

            StringFormat OutputFormat = new StringFormat();
            OutputFormat.LineAlignment = StringAlignment.Center;

            for (int Index = 0; Index < InputCount; Index++)
            {
                float Grid_Height = (this.Component.Attributes.Bounds.Height - this.ExtendHeight) / InputCount;
                RectangleF ParamBounds = InputParam[Index].Attributes.Bounds;
                string Text = IsFullName ? InputParam[Index].Name : InputParam[Index].NickName;

                float Point_X = ParamBounds.X;
                float Point_Y = (float)ParamBounds.Top;

                PointF TextLocation = new PointF(Point_X, Point_Y);
                RectangleF TextBox = new RectangleF(TextLocation, new SizeF(ParamBounds.Width+2, Grid_Height));
                g.DrawString(Text, GH_FontServer.Standard, _Brush, TextBox, InputFormat);
            }
            for (int Index = 0; Index < OutputCount; Index++)
            {
                RectangleF ParamBounds = OutputParam[Index].Attributes.Bounds;
                float Grid_Height = (this.Component.Attributes.Bounds.Height - this.ExtendHeight) / OutputCount;
                string Text = IsFullName ? OutputParam[Index].Name : OutputParam[Index].NickName;

                float Point_X = ParamBounds.Right - this.Component.Params.OutputWidth-2;
                float Point_Y = ParamBounds.Top;

                PointF TextLocation = new PointF(Point_X, Point_Y);
                RectangleF TextBox = new RectangleF(TextLocation, new SizeF(this.Component.Params.OutputWidth+2, Grid_Height));
                g.DrawString(Text, GH_FontServer.Standard, _Brush, TextBox, OutputFormat);
            }
        }
        private void DrawComponentName(Graphics g,bool IsFullName)
        {
            Pen _Pen = new Pen(Color.Black, 1);
            Brush _Brush = _Pen.Brush;
            string Text = IsFullName? this.Component.Name: this.Component.NickName;
            RectangleF ContentBox = (this.Component.Attributes as GH_ComponentAttributes).ContentBox;

            SizeF NameSize = GH_FontServer.MeasureString(Text, GH_FontServer.Large);
            //ContentBox is the property and m_innerBounds is the field
            StringFormat Format = new StringFormat(StringFormatFlags.DirectionVertical);

            Format.Alignment = StringAlignment.Center;
            Format.LineAlignment = StringAlignment.Center;
            //先存储Graphics的状态
            //对Graphics执行操作
            GraphicsState State = g.Save();
            //修改旋转中心点
            g.TranslateTransform(ContentBox.X + ContentBox.Width / 2, ContentBox.Y + ContentBox.Height / 2);
            g.RotateTransform(-180);
            g.TranslateTransform(-(ContentBox.X + ContentBox.Width / 2), -(ContentBox.Y + ContentBox.Height / 2));
            //回复旋转中心点
            g.DrawString(Text, GH_FontServer.Large, _Brush, ContentBox, Format);
           // 恢复Graphics的状态，否则Parameter Name的绘制将会出错
            g.Restore(State);
        }
    }
}
