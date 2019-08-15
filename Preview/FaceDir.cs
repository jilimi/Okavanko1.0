#if Hudson
using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace CSCECDEC.Okavango.Preview
{
    public class FaceDir : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FaceDir class.
        /// </summary>
        List<Line> OutputLine = new List<Line>();
        Color VectorColor = Color.Red;
        readonly int Density = 10;
        public FaceDir()
          : base("SurfaceDir", "SurfaceDir",
              "查看面的法线方向",
              Setting.PLUGINNAME, Setting.PREVIEWCATATORY)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "S", "需要查看方向的曲面", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "S", "法向量的长度", GH_ParamAccess.item,10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Surface Srf = default(Surface);
            double Scale = 0;

            if (OutputLine.Count != 0) OutputLine.Clear();

            if (!DA.GetData(0, ref Srf)) return;
            if (!DA.GetData(1, ref Scale)) return;

            Scale = (Scale <= 0) ? Scale = 1 : Math.Abs(Scale);
            List<List<double>> UVs = this.BuildDensityData();
            Surface RebuildSrf = this.RebuildSurface(Srf);

            for(int i = 0; i < UVs.Count; i++)
            {
                Point3d SPt = RebuildSrf.PointAt(UVs[i][0]/ Density, UVs[i][1]/ Density);
                Vector3d Vec = RebuildSrf.NormalAt(UVs[i][0]/ Density, UVs[i][1]/ Density);

                OutputLine.Add(new Line(SPt,Vec,100*Scale));

            }
        }
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (this.Hidden || this.Locked) return;
            if (this.Attributes.Selected) VectorColor = Color.Green;
            else VectorColor = Color.Red;
            args.Display.DrawArrows(OutputLine, VectorColor);
        }
        private List<List<double>> BuildDensityData()
        {
            List<List<double>> OutputList = new List<List<double>>();
            for (double i = 0; i < Density; i++)
            {
                for (double j = 0; j < Density; j++)
                {
                    List<double> TempList = new List<double> (){ i, j};
                    OutputList.Add(TempList);
                }
            }
            return OutputList;
        }
        private Surface RebuildSurface(Surface Srf)
        {
            Srf.SetDomain(0, new Interval(0, 1));
            Srf.SetDomain(1, new Interval(0, 1));

            return Srf;
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
            get { return new Guid("00217333-6437-4890-ba23-c49c35d58730"); }
        }
    }
}
#endif