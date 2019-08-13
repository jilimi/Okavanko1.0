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

namespace CSCECDEC.Plugin.Attribute
{
    class Hu_Attribute:GH_ComponentAttributes
    {

        //GAPE_1表示Grip Circle 和 Param Text之间的距离
        //GAPE_2表示Param Text与Component Text之间的距离；
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
        private void SetParamGridPosition(IGH_Param Param,float Pos_X,float Pos_Y)
        {
            RectangleF Temp_RectF = Param.Attributes.Bounds;
            Param.Attributes.Bounds = new RectangleF(Pos_X, Temp_RectF.Y + Pos_Y, Temp_RectF.Width, Temp_RectF.Height);
            //Param.Attributes.
        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if(channel == GH_CanvasChannel.Wires)
            {
                AttributeUtil.RenderBounds(graphics);
                base.Render(canvas, graphics, channel);
            }
            if(channel == GH_CanvasChannel.Objects)
            {
                AttributeUtil.CompoundRender(graphics, canvas);
            }
        }
    }
}
