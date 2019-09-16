using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

using Rhino.DocObjects;
using Rhino.DocObjects.Tables;


namespace CSCECDEC.Okavango.Types
{
    public class Hu_Font : GH_Goo<System.Drawing.Font>
    {
        public float FontHeight = 12;
        public bool IsBold = false;
        public bool IsItalic = false;
        public string FontFace = GH_FontServer.Standard.FontFamily.Name;
        public Hu_Font()
        {
            this.FontFace = GH_FontServer.Standard.Name;
            this.IsBold = false;
            this.IsItalic = false;
            this.FontHeight = 12F;
            //DO nothing
        }
        public Hu_Font(string FontFace, bool IsBold = false, bool IsItalic = false, float FontHeight = 12)
        {
            this.FontFace = FontFace;
            this.IsBold = IsBold;
            this.IsItalic = IsItalic;
            this.FontHeight = FontHeight;
        }
        public Hu_Font(System.Drawing.Font Font)
        {
            this.FontFace = Font.Name;
            this.IsBold = Font.Bold;
            this.IsItalic = Font.Italic;
            this.FontHeight = Font.GetHeight();
        }
        //拷贝构造函数
        public Hu_Font(Hu_Font HuFont)
        {
            this.FontFace = HuFont.FontFace;
            this.IsBold = HuFont.IsBold;
            this.IsItalic = HuFont.IsItalic;
            this.FontHeight = HuFont.FontHeight;
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
                return "Font that hold font information";
            }
        }

        public override string TypeName
        {
            get
            {
                return "Font";
            }
        }

        public override IGH_Goo Duplicate()
        {
            return new Hu_Font(this);
        }

        public override string ToString()
        {
            return this.FontFace.ToString();
        }
        public override bool CastFrom(object source)
        {
            if (source == null) return false;
            string Name = "";
            if (GH_Convert.ToString(source, out Name, GH_Conversion.Both))
            {
                //bold, Italic都为false
                Value = new System.Drawing.Font(Name,12F);
                return true;
            }
            return false;
        }
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(Q).IsAssignableFrom(typeof(string)))
            {
                object _Name = this.Value.Name;
                target = (Q)_Name;
                return true;
            }
            if (typeof(Q).IsAssignableFrom(typeof(GH_Number)))
            {
                object _Index = new GH_Number(this.Value.Height);
                target = (Q)_Index;
                return true;
            }
           return false;
        }
    }
}
