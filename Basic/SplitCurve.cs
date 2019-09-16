using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class SplitCurve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SplitCurveAtParameter class.
        /// </summary>
        public SplitCurve()
          : base("SplitCrv", "SplitCrv",
              "根据T值对线执行Split操作",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
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
            pManager.AddCurveParameter("Curve", "C", "需要执行Split操作的曲线", GH_ParamAccess.item);
            pManager.AddNumberParameter("Param", "T", "曲线的T值", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "分割后的曲线", GH_ParamAccess.list);
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve Crv = default(Curve);
            List<double> Ts = new List<double>();
            
            Curve[] TempCrvs = Crv.Split(Ts);

            if (TempCrvs.Length == 0) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "曲线切割失败");
            DA.SetDataList(0, TempCrvs);
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
            get { return new Guid("2518540c-f82d-4440-b4fc-82e28c066a66"); }
        }
    }
}