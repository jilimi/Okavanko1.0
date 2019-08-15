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
        protected int InnerControlNumber = 0;
        protected IGH_Component Component;
        public Hu_Attribute(GH_Component Owner,int InnerControlNumber = 0):base(Owner)
        {
            this.AttributeUtil = new Hu_AttributeUtil(Owner);
            this.InnerControlNumber = InnerControlNumber;
            this.Component = Owner;
        }
        protected override void Layout()
        {
            base.Layout();
            float Extend_Height = this.InnerControlNumber * Setting.COMPONENTCONTROLBOXHEIGHT;
            AttributeUtil.ComputeLayout(Extend_Height);
        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if(channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }
            if(channel == GH_CanvasChannel.Objects)
            {
                AttributeUtil.RenderComponentBounds(graphics);
                AttributeUtil.CompoundRender(graphics, canvas);
            }
        }
    }
}
