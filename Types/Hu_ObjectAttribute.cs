using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Rhino.DocObjects;
using Rhino.DocObjects.Tables;


namespace CSCECDEC.Okavango.Types
{
    class Hu_ObjectAttribute : GH_Goo<ObjectAttributes>
    {
        public Layer ObjectLayer;
        public Color ObjectColor;
        public Hu_ObjectAttribute()
        {
            this.ObjectLayer = Rhino.RhinoDoc.ActiveDoc.Layers.CurrentLayer;
            this.ObjectColor = Rhino.RhinoDoc.ActiveDoc.Layers.CurrentLayer.Color;
        }
        public Hu_ObjectAttribute(Layer Layer,Color Color)
        {
            this.ObjectLayer = Layer;
            this.ObjectColor = Color;
        }
        public Hu_ObjectAttribute(Hu_ObjectAttribute Attribute)
        {
            this.ObjectLayer = Attribute.ObjectLayer;
            this.ObjectColor = Attribute.ObjectColor;
        }
        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        public override string TypeDescription
        {
            get
            {
                return "Object Attribute Contain Object Layer And Color";
            }
        }

        public override string TypeName
        {
            get
            {
                return "ObjectAttribute";
            }
        }

        public override IGH_Goo Duplicate()
        {
            return new Hu_ObjectAttribute(this);
        }

        public override string ToString()
        {
            return this.ObjectLayer.ToString()+";"+this.ObjectColor.ToString();
        }
    }
}
