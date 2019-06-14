using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.Basic
{
    public class YZPlane : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the XZPlane class.
        /// </summary>
        public YZPlane()
          : base("YZPlane", "YZPlane",
              "根据Y、Z轴构造一个工作平面",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Origin", "Origin", "坐标原点", GH_ParamAccess.item,Plane.WorldXY.Origin);
            pManager.AddVectorParameter("Y", "Y", "Y 轴向量", GH_ParamAccess.item);
            pManager.AddVectorParameter("Z", "Z", "Z 轴向量", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "Plane", "工作平面", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d Origin = default(Point3d);
            Vector3d YAxis = default(Vector3d), ZAxis = default(Vector3d);
            Plane OutputPlane = Plane.WorldXY;

            if (!DA.GetData(0, ref Origin)) return;
            if (!DA.GetData(1, ref YAxis)) return;
            if (!DA.GetData(2, ref ZAxis)) return; 

            Vector3d XAxis = Vector3d.CrossProduct(YAxis, ZAxis);
            OutputPlane = new Plane(Origin, XAxis, YAxis);

            DA.SetData(0, OutputPlane);
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
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.CreateYZPlane;
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
            get { return new Guid("2B2762FB-5D73-46BE-ABCE-54AA2C771698"); }
        }
    }
}