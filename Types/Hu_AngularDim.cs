using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;
using Rhino.DocObjects;

using CSCECDEC.Okavango.Types;
using System.Windows.Forms;
using System.Drawing;
using GH_IO.Serialization;

namespace CSCECDEC.Okavango.Types
{
    class Hu_AngularDim : AnnotationBase
    {
        public Curve DimCrv;
        public Plane DimPl;
        public string DimText;
        public double DimSize;

        public Hu_AngularDim(Curve Crv, Plane Plane, string Text, double Size) : base()
        {
            this.DimCrv = Crv;
            this.DimPl = Plane;
            this.DimText = Text;
            this.DimSize = Size;
        }
    }
}