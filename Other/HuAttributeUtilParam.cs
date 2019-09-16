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
    sealed class Hu_AttributeUtilParam
    {
        protected dynamic ParamComponent;

        public Hu_AttributeUtilParam(IGH_Param ParamComponent)
        {
            this.ParamComponent = ParamComponent;
        }
        public void RenderParamsGripPoints(Graphics g,GH_Canvas canvas)
        {
            //render input params icon
                RectangleF ParamBounds = this.ParamComponent.Attributes.Bounds;
                float Grid_Height = ParamBounds.Height/ 2;
                PointF GridLocation_Input = new PointF(ParamBounds.X,ParamBounds.Y+ParamBounds.Height/2);
                GH_CapsuleRenderEngine.RenderInputGrip(g, canvas.Viewport.Zoom, GridLocation_Input, true);

                PointF GridLocation_Output = new PointF(ParamBounds.X+ParamBounds.Width, ParamBounds.Y+ParamBounds.Height/2);
                GH_CapsuleRenderEngine.RenderInputGrip(g, canvas.Viewport.Zoom, GridLocation_Output, true);
        }
        private void DrawIcon(Graphics g)
        {

            RectangleF Bounds = this.ParamComponent.Attributes.Bounds;
            GH_Capsule Capsule = GH_Capsule.CreateCapsule(Bounds, GH_Palette.Normal);
          
            Bitmap Bitmap = this.ParamComponent.Locked ? this.ParamComponent.Icon_24x24_Locked : this.ParamComponent.Icon_24x24;
            Capsule.RenderEngine.RenderIcon(g, Bitmap, 0, 0);

        }
        public void CompoundRender(Graphics g,GH_Canvas canvas)
        {
            RectangleF ContentBox = this.ParamComponent.Attributes.Bounds;
            this.RenderParamsGripPoints(g,canvas);
            if (Grasshopper.CentralSettings.CanvasObjectIcons)
            {
                this.DrawIcon(g);
            }
            else
            {
                if (Grasshopper.CentralSettings.CanvasFullNames) this.DrawComponentName(g, ContentBox,true);
                else this.DrawComponentName(g,ContentBox, false);
            }
        }
        //点击等效果到这里修改
        public void RenderComponentBounds(Graphics g)
        {
            Color ComponentColor = Setting.COMPONENTNORMALCOLOR;

            if (ParamComponent.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error) ComponentColor = Setting.COMPONENTERRORCOLOR;
            if (ParamComponent.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning) ComponentColor = Setting.COMPONENTWARNINGCOLOR;
            if (ParamComponent.Attributes.Selected) ComponentColor = Setting.COMPONENTSELECTCOLOR;
            if (ParamComponent.Locked) ComponentColor = Setting.COMPONENTLOCKCOLOR;

            RectangleF Bounds = ParamComponent.Attributes.Bounds;

            float Extend_Width = Setting.EXTEND_WIDTH;

            float Pos_X = Bounds.X - Extend_Width / 2;
            SizeF SizeF = new SizeF(Bounds.Width+ Extend_Width, Bounds.Height); 
            RectangleF RectF = new RectangleF(new PointF(Pos_X,Bounds.Y), SizeF);

            GraphicsPath Path = GH_CapsuleRenderEngine.CreateJaggedRectangle(RectF, 2, 2, 2, 2, false, false);
            g.DrawPath(new Pen(Color.Black), Path);

            SolidBrush Brush = new SolidBrush(ComponentColor);
            g.FillPath(Brush, Path);

           // g.DrawRectangle(new Pen(Color.Red),GH_Convert.ToRectangle(Bounds));
        }
        private void DrawComponentName(Graphics g,RectangleF ContentBox = new RectangleF(),bool IsFullName = true)
        {
            Pen _Pen = new Pen(Color.Black, 1);
            Brush _Brush = _Pen.Brush;
            string Text = IsFullName? this.ParamComponent.Name: this.ParamComponent.NickName;

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
    }
}
