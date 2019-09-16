using System;
using System.Drawing;
using System.Collections.Generic;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel;
using Rhino.Geometry;
using CSCECDEC.Okavango.Types;
using CSCECDEC.Okavango.Attribute;
using Rhino;

namespace CSCECDEC.Okavango.Dimension
{
    public class TextDotObj : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TextDot class.
        /// </summary>
        private TextDot Dot = null;
        private Color FillColor = Color.Transparent;
        public TextDotObj()
          : base("TextDot", "TextDot",
              "创建TextDot",
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
            pManager.AddPointParameter("Point", "P", "TextDot的位置", GH_ParamAccess.item);
            pManager.AddTextParameter("String", "S", "字符串", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "TextDot Color", GH_ParamAccess.item, Color.Black);

           // pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TextDot", "T", "TextDot", GH_ParamAccess.item);
        }
        protected override void BeforeSolveInstance()
        {
            this.Dot = null;
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d Location = default(Point3d);
            string Text = default(string);

            if (!DA.GetData(0, ref Location)) return;
            if (!DA.GetData(1, ref Text)) return;
            if (!DA.GetData(2, ref FillColor)) return;

            this.Dot = new Rhino.Geometry.TextDot(Text,Location);
            DA.SetData(0, Dot);
        }
        public override void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            var attr = doc.CreateDefaultAttributes();
            attr.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
            attr.ObjectColor = this.FillColor;
            doc.Objects.AddTextDot(this.Dot,attr);
        }
        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            base.DrawViewportMeshes(args);
            if (this.Dot == null) return;
            if (this.Hidden || this.Locked) return;

            System.Drawing.Color dotColor = System.Drawing.Color.Red;
            if (this.Attributes.Selected) {
                args.Display.DrawDot(this.Dot.Point, this.Dot.Text, Color.Green, Color.White);
            }else
            {
                args.Display.DrawDot(this.Dot, FillColor, Color.White, Color.Black);
            }
        }
        public override BoundingBox ClippingBox
        {
            get
            {
                return this.Dot.GetBoundingBox(true);
            }
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
            get { return new Guid("24a24263-416b-48c6-a199-680d50e3ffaa"); }
        }
    }
}