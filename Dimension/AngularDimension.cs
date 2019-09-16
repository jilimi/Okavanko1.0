using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using CSCECDEC.Okavango.Attribute;
using Grasshopper.Kernel.Attributes;

namespace CSCECDEC.Okavango.Dimension
{
    public class AngularDimension : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AngularDimension class.
        /// </summary>
        Rhino.Geometry.AngularDimension AngularDim = null;
        public AngularDimension()
          : base("AngularDimension", "AngularDimension",
              "角度标注",
              Setting.PLUGINNAME, Setting.DIMENSIONCATATORY)
        {
        }
        public override void CreateAttributes()
        {
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new GH_ComponentAttributes(this);

        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddArcParameter("Arc", "A", "需要进行测量的弧", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset", "O", "偏移的距离", GH_ParamAccess.item,10);
            pManager.AddTextParameter("Text", "T", "标注显示的文字", GH_ParamAccess.item);
            pManager.AddGenericParameter("DimStyle", "D", "标注样式", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Dim", "D", "弧度标注", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Arc arc  =default(Arc);
            double Offset = 10;
            string Text = null;
            string Style = null;
            double Size = 10;

            if (!DA.GetData(0, ref arc)) return;
            if (!DA.GetData(1, ref Offset)) return;
            if (!DA.GetData(2, ref Text)) return;
            if (!DA.GetData(3, ref Size))return;
            if (!DA.GetData(4, ref Style)) return;

            DA.GetData(2, ref Text);

            AngularDim = new Rhino.Geometry.AngularDimension(arc, Offset);
            if (Text != null) AngularDim.RichText = Text;

            AngularDim.ArrowSize = Size;
            AngularDim.TextHeight = Size;
            AngularDim.DimensionStyleId = this.GetStyle(Style).Id;

            DA.SetData(0, AngularDim);

        }
        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            if (this.AngularDim == null) return;
            if (this.Hidden || this.Locked) return;

            if (this.Attributes.Selected)
            {
                args.Display.DrawAnnotation(this.AngularDim, System.Drawing.Color.Green);
            }else
            {
                args.Display.DrawAnnotation(this.AngularDim, System.Drawing.Color.Red);
            }
        }
        private DimensionStyle GetStyle(string StyleName)
        {
            Rhino.DocObjects.Tables.DimStyleTable Tables = Rhino.RhinoDoc.ActiveDoc.DimStyles;
            DimensionStyle Style = Tables.FindName(StyleName);

            if(Style == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, string.Format("Dimension Name {0} is not found, use current Style Instead",StyleName));
                return RhinoDoc.ActiveDoc.DimStyles.Current;
            }else
            {
                return Style;
            }
        }
        public override BoundingBox ClippingBox
        {
            get
            {
                //这个在View Clipping的时候会用到，如果计算机不能准确计算出clippingbox，那么就会显示的不正确
                return AngularDim.GetBoundingBox(true);
            }
        }
        public override void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            base.BakeGeometry(doc, obj_ids);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7fb6e31f-4ae8-4faf-a678-82e3b748a7e4"); }
        }
    }
}