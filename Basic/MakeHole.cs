using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class MakeHole : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MakeHole class.
        /// </summary>
        public MakeHole()
          : base("MakeHole", "MakeHole",
              "Make hole for Brep Surface",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
        }
        public override void CreateAttributes()
        {
            if (Setting.ISRENDERHUATTRIBUTE) m_attributes = new Hu_Attribute(this);
            else m_attributes = new GH_ComponentAttributes(this);

        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Panel", "P", "需要开洞的面板", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "P", "开洞位置", GH_ParamAccess.list);
            pManager.AddNumberParameter("Dist", "D", "推拉距离", GH_ParamAccess.item, 4);
            pManager.AddNumberParameter("Radius", "R", "开洞半径", GH_ParamAccess.item,4);

            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Pipe", "P", "开孔圆管", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Brep Panel = default(Brep);
            List<Point3d> Points = new List<Point3d>();
            List<Brep> OutputBrep = new List<Brep>();
            double Radius = 4, Dist = 10;

            if (!DA.GetData(0, ref Panel)) return;
            if (!DA.GetDataList(1, Points)) return;
            if (!DA.GetData(2, ref Dist)) return;
            if (!DA.GetData(3, ref Radius)) return;

            OutputBrep = this.CreatePipe(Points, Panel, Radius, Dist);
            DA.SetDataList(0, OutputBrep);
        }
        protected List<Brep> CreatePipe(List<Point3d> Pts,Brep Panel, double Radius,double Dist)
        {
            List<Brep> OutputBrep = new List<Brep>();
            
            foreach(Point3d Pt in Pts)
            {
                Point3d ClosePt;
                ComponentIndex Index;
                double T0, T1;
                Vector3d Normal;

                if (Panel.ClosestPoint(Pt, out ClosePt, out Index, out T0, out T1, 0, out Normal)) {

                    Circle Curve_Circle = new Circle(new Plane(ClosePt, Normal),Radius);
                    Curve Crv = Curve_Circle.ToNurbsCurve();
                    Crv.Transform(Transform.Translation(Vector3d.Multiply(Normal, Dist)));
                    Surface Srf = Extrusion.CreateExtrusion(Crv, Vector3d.Multiply(Normal, -2*Dist));
                    OutputBrep.Add(Srf.ToBrep());
                };
            }
            return OutputBrep;
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
            get { return new Guid("53bb303d-c1fc-4d6a-9071-e21b7e729c3d"); }
        }
    }
}