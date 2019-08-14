using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Drawing.Drawing2D;

namespace CSCECDEC.Okavango.Attribute
{
    class Hu_Attribute:GH_ComponentAttributes
    {

        public Hu_AttributeUtil AttributeUtil;
        public Hu_Attribute(GH_Component Owner):base(Owner)
        {
            this.AttributeUtil = new Hu_AttributeUtil(this.Owner);
        }
        public override void ExpireLayout()
        {
            base.ExpireLayout();
        }
        protected override void Layout()
        {
            //这里会把GH_Capsule的大小重置到默认大小
            base.Layout();
            AttributeUtil.ComputeLayout(0);
        }
        /*
        private void SetParamGridPosition(IGH_Param Param,float Pos_X,float Pos_Y)
        {
            RectangleF Temp_RectF = Param.Attributes.Bounds;
            Param.Attributes.Bounds = new RectangleF(Pos_X, Temp_RectF.Y + Pos_Y, Temp_RectF.Width, Temp_RectF.Height);
        }*/
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if(channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }
            if(channel == GH_CanvasChannel.Objects)
            {
                AttributeUtil.RenderBounds(graphics);
                AttributeUtil.CompoundRender(graphics, canvas);
            }
        }
    }
}
