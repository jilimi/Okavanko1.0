using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Windows.Forms;
using CSCECDEC.Okavango.Attribute;
using Grasshopper.Kernel.Attributes;
using Rhino.DocObjects;
using Rhino;

namespace CSCECDEC.Okavango.Dimension
{
    public class LinearDimensionObj : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateLinearDimension class.
        /// </summary>
        bool IsAlign = false;
        LinearDimension Dimension;
        string Id = Guid.NewGuid().ToString();
        public LinearDimensionObj()
          : base("LinearDimension", "LinearDimension",
              "Description",
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
            Rhino.RhinoDoc Doc = Rhino.RhinoDoc.ActiveDoc;

            pManager.AddCurveParameter("Curve","C","需要标注的曲线",GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "标注所在工作平面", GH_ParamAccess.item,Plane.WorldXY);
            pManager.AddNumberParameter("Offset", "O", "标注偏移距离", GH_ParamAccess.item,10);
            pManager.AddTextParameter("DimStyle", "D", "标注样式",GH_ParamAccess.item);
            pManager.AddTextParameter("Text", "T", "文字", GH_ParamAccess.item,Id);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Dimension", "D", "标注", GH_ParamAccess.item);
        }
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Align",On_AlignClick);

        }

        private void On_AlignClick(object sender, EventArgs e)
        {
            if (IsAlign) IsAlign = false;
            else IsAlign = true;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Curve Crv = default(Curve);
            Plane Plane = default(Plane);
            double Distance = default(double);
            String Text = null;
            String StyleName = default(String);
            DimensionStyle DimStyle = default(DimensionStyle);

            if (!DA.GetData(0, ref Crv)) return;
            if (!DA.GetData(1, ref Plane)) return;
            if (!DA.GetData(2, ref Distance)) return;
            if (!DA.GetData(3, ref StyleName)) return;
            if (!DA.GetData(4, ref Text)) return;

            DimStyle = GetStyle(StyleName);
            if(DimStyle == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, string.Format("未找到指定名称为：{0}的标注样式",StyleName));
                DimStyle = Rhino.RhinoDoc.ActiveDoc.DimStyles.Current;
            }
            //计算偏移的距离
            Vector3d MoveDir = Vector3d.CrossProduct(Plane.ZAxis, Point3d.Subtract(Crv.PointAtStart,Crv.PointAtEnd));
            MoveDir.Unitize();
            Transform Trans = Transform.Translation(Vector3d.Multiply(Distance,MoveDir));

            Point3d LinePt = (Crv.PointAtStart + Crv.PointAtEnd)/2;
            Point3d StartPt = Crv.PointAtStart;
            Point3d EndPt = Crv.PointAtEnd;
            LinePt.Transform(Trans);

            Plane PlacePlane = new Plane(StartPt, EndPt, LinePt);

            double u, v;
            PlacePlane.ClosestParameter(LinePt, out u, out v);
            Point2d _LinePt = new Point2d(u, v);

            PlacePlane.ClosestParameter(StartPt, out u, out v);
            Point2d _ext1 = new Point2d(u, v);

            PlacePlane.ClosestParameter(EndPt, out u, out v);
            Point2d _ext2 = new Point2d(u, v);


            Dimension = new LinearDimension(PlacePlane, _ext1,_ext2, _LinePt);
            Dimension.DimensionStyleId = DimStyle.Id;
            Dimension.Aligned = IsAlign;
            if (Text != Id) Dimension.RichText = Text;
            else Dimension.RichText = Math.Floor(Crv.GetLength()).ToString();
            DA.SetData(0, Dimension);
        }
        public override void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            doc.Objects.AddLinearDimension(Dimension);
            base.BakeGeometry(doc, obj_ids);
        }
        private DimensionStyle GetStyle(string StyleName)
        {
            Rhino.DocObjects.Tables.DimStyleTable Tables = Rhino.RhinoDoc.ActiveDoc.DimStyles;
            DimensionStyle Style = Tables.FindName(StyleName);

            if (Style == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, string.Format("Dimension Name {0} is not found, use current Style Instead", StyleName));
                return RhinoDoc.ActiveDoc.DimStyles.Current;
            }
            else
            {
                return Style;
            }
        }
        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            Color DimColor = Color.Red;
            if (this.Attributes.Selected) DimColor = Color.Green;
            if (this.Locked || this.Hidden) return;
            args.Display.DrawAnnotation(Dimension, DimColor);
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
            get { return new Guid("35e53053-2713-4c98-9fee-2c80746f9a61"); }
        }
    }
}