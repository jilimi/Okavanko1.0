using System;
using System.Collections.Generic;

using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.DocObjects.Tables;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using GH_IO.Serialization;

namespace CSCECDEC.Okavango.Types
{
    //不要直接继承IGH_Goo,很多时候我们只需要GH_Goo的默认功能，
    public class Hu_Layer : GH_Goo<Rhino.DocObjects.Layer>
    {
        public Hu_Layer()
        {
            this.Value = new Layer();
        }
        public Hu_Layer(string LayerName)
        {
            Layer _Layer = new Layer();
            _Layer.Name = LayerName;
            this.Value = _Layer;
        }
        public Hu_Layer(int LayerIndex)
        {
            LayerTable LT = Rhino.RhinoDoc.ActiveDoc.Layers;
            this.Value = LT.FindIndex(LayerIndex);
        }
        //这个构造函数一般都用不到，以防万一
        public Hu_Layer(Guid Id)
        {
            LayerTable LT = Rhino.RhinoDoc.ActiveDoc.Layers;
            this.Value = LT.FindId(Id);
        }
        public Hu_Layer(Hu_Layer LayerWrapper)
        {
            this.Value = LayerWrapper.Value;
        }
        public Hu_Layer(Rhino.DocObjects.Layer Layer)
        {
            this.Value = Layer;
        }
        public override bool IsValid
        {
            get
            {
                if(this.Value == null)
                {
                    return false;
                }else
                {
                    return true;
                }
            }
        }

        public override string TypeDescription
        {
            get
            {
                return "An Wrapper of Rhino Layer";
            }
        }
        public override object ScriptVariable()
        {
            return this.Value;
        }
        public override string TypeName
        {
            get
            {
                return "Layer";
            }
        }
        public override Layer Value
        {
            get
            {
                return base.Value;
            }

            set
            {
                base.Value = value;
            }
        }
        public override IGH_Goo Duplicate()
        {
            return new Hu_Layer(this);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("LayerIndex",this.Value.Index);
            return base.Write(writer);
        }
        public override bool Read(GH_IReader reader)
        {
            this.Value = Rhino.RhinoDoc.ActiveDoc.Layers.FindIndex(reader.GetInt32("LayerIndex"));
            return base.Read(reader);
        }
        public override string ToString()
        {
            return this.Value.FullPath;
        }
        public override bool CastFrom(object source)
        {
            if (source == null) return false;
            int val;
            LayerTable LT = Rhino.RhinoDoc.ActiveDoc.Layers;
            if (GH_Convert.ToInt32(source, out val, GH_Conversion.Both))
            {
                Rhino.DocObjects.Layer La = LT.FindIndex(val);
                if (La == null)
                {
                    Value = null;
                    return false;
                }else
                {
                    Value = La;
                    return true;    
                }
            }
            Guid id;
            if(GH_Convert.ToGUID(source,out id, GH_Conversion.Both)){

                Rhino.DocObjects.Layer La = LT.FindId(id);
                if (La == null)
                {
                    Value = null;
                    return false;
                }
                else
                {
                    Value = La;
                    return true;
                }
            }
            string Name;
            if (GH_Convert.ToString(source, out Name, GH_Conversion.Both)){

                int Index = LT.FindByFullPath(Name,-1);
                if (Index == -1)
                {
                    Rhino.DocObjects.Layer La = new Layer();
                    La.Name = Name;
                    Value = La;
                    return true;
                }
                else
                {
                    Value = LT.FindIndex(Index);
                    return true;
                }
            }
            if(source.GetType() == typeof(Layer))
            {
                Value = (Layer)source;
                return true;
            }
            if(source.GetType() == typeof(Hu_Layer))
            {
                Value = ((Hu_Layer)source).Value;
                return true;
            }
            return false;
        }
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(Q).IsAssignableFrom(typeof(int)))
            {
                object _Index = this.Value.Index;
                target = (Q)_Index;
                return true;
            }

            if (typeof(Q).IsAssignableFrom(typeof(GH_Integer)))
            {
                object _Index = new GH_Integer(this.Value.Index);
                target = (Q)_Index;
                return true;
            }
            if (typeof(Q).IsAssignableFrom(typeof(Guid)))
            {
                object _Id = this.Value.Id;
                target = (Q)_Id;
                return true;
            }
            if (typeof(Q).IsAssignableFrom(typeof(GH_Guid)))
            {
                object _Id = new GH_Guid(this.Value.Id);
                target = (Q)_Id;
                return true;
            }
            if (typeof(Q).IsAssignableFrom(typeof(GH_String)))
            {
                object _Path = new GH_String(this.Value.FullPath);
                target = (Q)_Path;
                return true;
            }
            if (typeof(Q).IsAssignableFrom(typeof(string)))
            {
                object _Path = this.Value.FullPath;
                target = (Q)_Path;
                return true;
            }
            if (typeof(Q).IsAssignableFrom(typeof(Layer)))
            {
                LayerTable LT = Rhino.RhinoDoc.ActiveDoc.Layers;
                object _layer = LT.FindIndex(this.Value.Index);
                target = (Q)_layer;
                return true;
            }
            return false;
        }
    }
}