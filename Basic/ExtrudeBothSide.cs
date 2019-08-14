using System;
using System.Collections.Generic;

using Rhino.FileIO;
using Rhino.Collections;
using Rhino.Geometry.Collections;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Okavango.Basic
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
            pManager.AddGeometryParameter("Geom", "G", "需要推拉的Curve，Surface面和线必须是平面或平面线", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vector", "V", "推拉的方向和距离", GH_ParamAccess.item);
            pManager.AddBooleanParameter("BothSide", "D", "是否双向推拉", GH_ParamAccess.item, false);
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
            bool IsDubleSide = true,IsCape = true;
            Brep Output = default(Brep);

            if (!DA.GetData(0, ref Geom)) return;
            if (!DA.GetData(1, ref Direction)) return;
            if (!DA.GetData(2, ref IsDubleSide)) return;
            if (!DA.GetData(3, ref IsCape)) return;

            if(Geom is Curve)
            {
                Curve Crv = Geom as Curve;
                Output = this.ExtrudeCurveBothSide(Crv, Direction, IsDubleSide,IsCape);
            }
            else if(Geom is Surface)
            {
                Surface Srf = Geom as Surface;
                //
                //实现在函数里面书写函数
                //
                //
                Func<Surface, Curve> CreateBoundary = _Srf =>
                 {
                     Brep b = _Srf.ToBrep();
                     CurveList Crvs = new CurveList();

                     BrepEdgeList es = b.Edges;

                     foreach (BrepEdge e in es)
                     {
                         Crvs.Add(e.EdgeCurve);
                     }
                     return Curve.JoinCurves(Crvs)[0];
                 };
                Curve Crv = CreateBoundary(Srf);
                Output = this.ExtrudeCurveBothSide(Crv, Direction, IsDubleSide, IsCape);
            }
            if (Output == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "输入的几何体不是平面几何体");
            }
            DA.SetData(0, Output);
        }
        private Brep ExtrudeCurveBothSide(Curve Crv,Vector3d Direction,bool BothSide,bool IsCape)
        {

            double Tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

            if(Direction.Length == 0)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "拉伸向量的长度不能为0");
                return null;
            }
            if (BothSide)
            {
                Crv.Transform(Transform.Translation(Direction));
                Direction.Reverse();
                Direction = Direction * 2;

            }
            if (Crv.IsPlanar())
            {
                try
                {
                    Brep Temp = Extrusion.CreateExtrusion(Crv, Direction).ToBrep();
                    if (IsCape) return Temp.CapPlanarHoles(Tolerance);
                    else return Temp;
                }
                catch (Exception e)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("内部错误：{0}", e.Message));
                    return null;
                }

            }
            else
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "输入的几何体不符合条件");
                return null;
            }
        }
        private Curve GetSurfaceBoundary(Surface Srf)
        {
            Brep b = Srf.ToBrep();
            CurveList Crvs = new CurveList();

            BrepEdgeList es = b.Edges;
            
            foreach(BrepEdge e in es)
            {
                Crvs.Add(e.EdgeCurve);
            }
           return Curve.JoinCurves(Crvs)[0];
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
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.ExtrudeBothSide;
                //Graphic 沒有public的構造函數，不能使用new運算符，衹能通過其他方式創建graphic
                Graphics graphic = Graphics.FromImage((Image)newImage);
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphic.DrawImage(originalImg, 0, 0, newImage.Width, newImage.Height);
                return newImage;
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