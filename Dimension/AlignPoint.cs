using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel.Attributes;

using Grasshopper.Kernel;
using Rhino.Geometry;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Dimension
{
    public class AlignPoint : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AlignPoint class.
        /// </summary>
        public AlignPoint()
          : base("AlignPoint", "AlignPoint",
              "对齐点位",
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

            pManager.AddPointParameter("Points", "Pts", "需要对齐的点位", GH_ParamAccess.list);
            pManager.AddPointParameter("Point", "P", "需要对齐的基准点", GH_ParamAccess.item);
            pManager.AddTextParameter("Axis", "A", "需要对齐的轴", GH_ParamAccess.item,"X");

            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "已经对齐的点位", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> Pts = new List<Point3d>();
            List<Point3d> OutputPts = new List<Point3d>(); 
            Point3d BasePt = default(Point3d);
            string Axis = "X";

            if (!DA.GetDataList(0, Pts)) return;
            if (!DA.GetData(1, ref BasePt)) return;
            if (!DA.GetData(2, ref Axis)) return;

            OutputPts = Pts.Select(item =>
            {
                Point3d NewPt = new Point3d();
                switch (Axis.ToLower())
                {
                    case "x":
                        NewPt = new Point3d(BasePt.X,item.Y,item.Z);
                        break;
                    case "y":
                        NewPt = new Point3d(item.X, BasePt.Y, item.Z);
                        break;
                    case "z":
                        NewPt = new Point3d(item.X, item.Y, BasePt.Z);
                        break;
                    default:
                        NewPt = new Point3d(BasePt.X, item.Y, item.Z);
                        break;
                }
                return NewPt;
            }).ToList();
            DA.SetDataList(0, OutputPts);
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
            get { return new Guid("5818aa23-2b16-40d5-90ba-a2eac1a8ecac"); }
        }
    }
}