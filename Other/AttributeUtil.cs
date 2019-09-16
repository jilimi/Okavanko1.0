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
    sealed class AttributeUtil
    {
        protected dynamic Component;
        private bool IsComponent = true;
        private float ExtendWidth = 0;
        private float ExtendHeight = 0;

        public AttributeUtil(dynamic Component)
        {
            this.Component = Component;
            if ((this.Component as IGH_Component) != null)
            {
                IsComponent = true;
            }else
            {
                IsComponent = false;
            }
        }
        private void ComputeBounds(float ExtendWidth,float ExtendHeight)
        {
            if (!IsComponent) return;

            RectangleF Bounds = this.Component.Attributes.Bounds;

            this.ExtendWidth = ExtendWidth <= 0 ? 0 : ExtendWidth;
            this.ExtendHeight = ExtendHeight <= 0 ? 0 : ExtendHeight;
            float Bound_Height = Bounds.Height + ExtendHeight;

            this.Component.Attributes.Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bound_Height);
        }
        public void ComputePramGridBounds()
        {
            if (!IsComponent) return;

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

                Outputs[Index].Attributes.Bounds = new RectangleF(Pos_X, Pos_Y, Param_Width, Grid_Height);
            }
        }
        public void ComputeFixPramGridBounds(float Width = 0)
        {
            if (!IsComponent) return;

            List<IGH_Param> Inputs = Component.Params.Input;
            List<IGH_Param> Outputs = Component.Params.Output;
            RectangleF Bounds = this.Component.Attributes.Bounds;

            int InputCount = Inputs.Count;
            int OutputCount = Outputs.Count;

            for (int Index = 0; Index < InputCount; Index++)
            {
                float Grid_Height = (this.Component.Attributes.Bounds.Height - this.ExtendHeight) / InputCount;

                float Pos_X = Bounds.Left;
                float Pos_Y = Bounds.Top + Grid_Height * Index;

                Inputs[Index].Attributes.Bounds = new RectangleF(Pos_X, Pos_Y, Width, Grid_Height);
            }
            //Output Params
            for (int Index = 0; Index < OutputCount; Index++)
            {
                float Grid_Height = (this.Component.Attributes.Bounds.Height - this.ExtendHeight) / OutputCount;

                float Pos_X = Bounds.Right - Width;
                float Pos_Y = Bounds.Top + Grid_Height * Index;

                Outputs[Index].Attributes.Bounds = new RectangleF(Pos_X, Pos_Y, Width, Grid_Height);
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
        /// <summary>
        /// For Rendering Parameter Input/Output Grid
        /// </summary>
        /// <param name="g"></param>
        /// <param name="canvas"></param>
        public void RenderParamsGripPoints2(Graphics g, GH_Canvas canvas)
        {
            //render input params icon
            RectangleF ParamBounds = this.Component.Attributes.Bounds;
            float Grid_Height = ParamBounds.Height / 2;
            PointF GridLocation_Input = new PointF(ParamBounds.X, ParamBounds.Y + ParamBounds.Height / 2);
            GH_CapsuleRenderEngine.RenderInputGrip(g, canvas.Viewport.Zoom, GridLocation_Input, true);

            PointF GridLocation_Output = new PointF(ParamBounds.X + ParamBounds.Width, ParamBounds.Y + ParamBounds.Height / 2);
            GH_CapsuleRenderEngine.RenderInputGrip(g, canvas.Viewport.Zoom, GridLocation_Output, true);
        }
        /// <summary>
        /// For Rendering Component Parameter Input/Output Grid
        /// </summary>
        /// <param name="g"></param>
        /// <param name="canvas"></param>
        public void RenderParamsGripPoints1(Graphics g,GH_Canvas canvas)
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

            GH_ComponentAttributes Com_Attributes = this.Component.Attributes as GH_ComponentAttributes;
            RectangleF BitmapRect;
            if(IsComponent) BitmapRect = Com_Attributes.ContentBox;
            else BitmapRect = this.Component.Attributes.Bounds;
           
            GH_Capsule Capsule = GH_Capsule.CreateCapsule(Com_Attributes.ContentBox, GH_Palette.Normal);
            Bitmap Bitmap = this.Component.Locked ? this.Component.Icon_24x24_Locked : this.Component.Icon_24x24;
            Capsule.RenderEngine.RenderIcon(g, Bitmap, 0, 0);

        }
        public void ComputeLayout(float Width = 0,float Height = 0)
        {
            //还需要注意这里的先后顺序
            //先计算边界，然后在计算Param的Bounds
            //否则将会渲染的不正确
            this.ComputeBounds(Width, Height);
            this.ComputePramGridBounds();
        }
        public void SetupRender(Graphics g,GH_Canvas canvas)
        {
            if (IsComponent) this.RenderParamsGripPoints1(g, canvas);
            else this.RenderParamsGripPoints2(g, canvas);
          
            if (Grasshopper.CentralSettings.CanvasObjectIcons)
            {
                this.DrawIcon(g);
            }
            else
            {
                RectangleF ContentBox;
                if (IsComponent)
                {
                    ContentBox = this.Component.Attribute.ContentBox;
                }
                else
                {
                    ContentBox = this.Component.Bounds;
                }
                if (Grasshopper.CentralSettings.CanvasFullNames) this.DrawComponentName(ContentBox,g, true);
                else this.DrawComponentName(ContentBox,g, false);
            }
            if (Grasshopper.CentralSettings.CanvasFullNames&&IsComponent) this.RenderParamName(g, true);
            else this.RenderParamName(g, false);
        }

        //点击等效果到这里修改
        public void RenderComponentBounds(Graphics g)
        {
            bool LeftJagged = false, RightJagged = false;
            Color ComponentColor = Setting.COMPONENTNORMALCOLOR;

            if (this.Component.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error) ComponentColor = Setting.COMPONENTERRORCOLOR;
            if (this.Component.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning) ComponentColor = Setting.COMPONENTWARNINGCOLOR;
            if (this.Component.Attributes.Selected) ComponentColor = Setting.COMPONENTSELECTCOLOR;
            if (this.Component.Locked) ComponentColor = Setting.COMPONENTLOCKCOLOR;

            RectangleF Bounds = this.Component.Attributes.Bounds;
            if (IsComponent)
            {
                if (this.Component.Params.Output.Count == 0) RightJagged = true;
                if (this.Component.Params.Input.Count == 0) LeftJagged = true;
            }

            float Extend_Width = Setting.EXTEND_WIDTH;

            float Pos_X = Bounds.X - Extend_Width / 2;
            SizeF SizeF = new SizeF(Bounds.Width+ Extend_Width, Bounds.Height); 
            RectangleF RectF = new RectangleF(new PointF(Pos_X,Bounds.Y), SizeF);

            GraphicsPath Path = GH_CapsuleRenderEngine.CreateJaggedRectangle(RectF, 2, 2, 2, 2, LeftJagged, RightJagged);
            g.DrawPath(new Pen(Color.Black), Path);

            SolidBrush Brush = new SolidBrush(ComponentColor);
            g.FillPath(Brush, Path);

           // g.DrawRectangle(new Pen(Color.Red),GH_Convert.ToRectangle(Bounds));
        }
        private void RenderParamName(Graphics g,bool IsFullName)
        {

            Pen _Pen;
            if (this.Component.Locked)
            {
                _Pen = new Pen(Color.FromArgb(255, 134, 136, 137), 1);
            }else
            {
                _Pen = new Pen(Color.Black, 1);
            }
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
        private void DrawComponentName(RectangleF ContentBox,Graphics g,bool IsFullName = true)
        {
            Pen _Pen = new Pen(Color.Black, 1);
            Brush _Brush = _Pen.Brush;
            string Text = IsFullName? this.Component.Name: this.Component.NickName;

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
            Format.Dispose();
        }
        public void DrawHorizonComponentName(RectangleF ContentBox,Graphics g, bool IsFullName)
        {
            Pen _Pen = new Pen(Color.Black, 1);
            Brush _Brush = _Pen.Brush;
            string Text = IsFullName ? this.Component.Name : this.Component.NickName;
            // 12+2
            StringFormat Format = new StringFormat();
            Format.LineAlignment = StringAlignment.Center;
            Format.Alignment = StringAlignment.Center;

            g.DrawString(Text, GH_FontServer.Standard, _Brush, ContentBox, Format);
            Format.Dispose();
        }
    }
}
