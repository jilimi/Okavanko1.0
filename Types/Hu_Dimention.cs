using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;

using Rhino;
using Rhino.Geometry;

namespace CSCECDEC.Okavango.Types
{
    class Hu_Dimention:GH_Goo<LinearDimension>
    {
        public Curve Curve;
        public Plane Plane;
        public string Text;
        public double StyleIndex;
        public double Dis = 0;
        public bool Align = false;

        public Hu_Dimention(Curve Crv,Plane Plane,string Text, int StyleIndex,double Dist,bool Align):base()
        {
            this.Curve = Crv;
            this.Plane = Plane;
            this.Text = Text;
            this.StyleIndex = StyleIndex;
            this.Align = Align;
        }
        public Hu_Dimention(Hu_Dimention Dim) : base()
        {
            this.Curve = Dim.Curve;
            this.Plane = Dim.Plane;
            this.Text = Dim.Text;
            this.StyleIndex = Dim.StyleIndex;
            this.Align = Dim.Align;
        }
        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        public override string TypeName
        {
            get
            {
                return "线性标注";
            }
        }

        public override string TypeDescription
        {
            get
            {
                return "线性标注，用于几何体的尺寸标注";
            }
        }
        public override IGH_Goo Duplicate()
        {
            return new Hu_Dimention(this);
        }

        public override string ToString()
        {
            return "LinearDimention";
        }
    }
}
