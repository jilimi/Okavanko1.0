using System;
using System.Collections.Generic;


using Rhino.Collections;
using Rhino.Geometry.Collections;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace CSCECDEC.Plugin.Basic
{
    public class ExtrudeBothSide : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExtrudeBothSide class.
        /// </summary>
        public ExtrudeBothSide()
          : base("ExtrudeBothSide", "ExtrudeBothSide",
              "向两边推拉曲线或面",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quinary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "需要推拉的Curve，Surface，或Point,面和线必须是平面或平面线", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vector", "V", "推拉的方向和距离", GH_ParamAccess.item);
            pManager.AddBooleanParameter("DoulbleSide", "D", "是否双向推拉", GH_ParamAccess.item,true);
            pManager.AddBooleanParameter("Cap", "C", "是否封口", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "推拉的结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryBase Geom = default(GeometryBase);
            Vector3d Direction = default(Vector3d);
            bool IsDubleSide = true;
            GeometryBase Output = default(GeometryBase);

            if (!DA.GetData(0, ref Geom)) return;
            if (!DA.GetData(1, ref Direction)) return;
            if (!DA.GetData(2, ref IsDubleSide)) return;

            if(Geom is Curve)
            {
                Curve Crv = Geom as Curve;
                Crv.Transform(Transform.Translation(Direction));

                Direction.Reverse();
                Direction = Direction * 2;
                if (Crv.IsPlanar())
                {
                    Output = Extrusion.CreateExtrusion(Crv, Direction);
                }else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "输入的线为非平面");
                }
            }
            else if(Geom is Point)
            {
                Point Pt = Geom as Point;
                Pt.Transform(Transform.Translation(Direction));

                Direction.Reverse();
                Direction = Direction * 2;

                Output = new LineCurve(new Line(Pt.Location, Direction, Direction.Length));
            }
            else
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "输入的几何体不符合要求");
            }

            DA.SetData(0, Output);
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
            get { return new Guid("00e8b263-9257-44a1-a4d9-290d232ea31a"); }
        }
    }
}