#if Hudson
using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Okavango.Basic
{
    public class TransformFromCplane : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TransformFromCplane class.
        /// </summary>
        public TransformFromCplane()
          : base("GetTransByPlane", "GetTransByPlane",
              "对物体进行坐标系变换",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Source", "S", "原工作平面", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddPlaneParameter("Target", "T", "目标工作平面", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTransformParameter("Tran", "T", "变换矩阵",GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Plane SourcePlane = default(GH_Plane), TargetPlane = default(GH_Plane);

            if(!DA.GetData(0, ref SourcePlane))return;
            if(!DA.GetData(1, ref TargetPlane))return;
            Transform Tran = Rhino.Geometry.Transform.ChangeBasis(SourcePlane.Value,TargetPlane.Value);
            DA.SetData(0, Tran);
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
                Bitmap originalImg = Properties.Resources.PlaneTransform;
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
            get { return new Guid("3095a707-57bf-43fc-a93b-fc709afc2861"); }
        }
    }
}
#endif