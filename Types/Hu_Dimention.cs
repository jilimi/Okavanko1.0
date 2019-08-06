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

namespace CSCECDEC.Plugin.Types
{
    class Hu_Dimention:AnnotationBase
    {
        public Curve DimCrv;
        public Plane DimPl;
        public string DimText;
        public double DimSize;

        public Hu_Dimention(Curve Crv,Plane Plane,string Text, double Size):base()
        {
            this.DimCrv = Crv;
            this.DimPl = Plane;
            this.DimText = Text;
            this.DimSize = Size;
        }
    }
}
