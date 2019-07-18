using System;
using System.Collections.Generic;

using Rhino.DocObjects;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.Basic
{
    public class BakeGeometry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BakeGeometry class.
        /// </summary>
        public BakeGeometry()
          : base("BakeGeometry", "BakeGeometry",
              "将物体烘焙到图层当中",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.senary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Layer", "L", "图层", GH_ParamAccess.item);
            pManager.AddGeometryParameter("Geometry", "G", "需要烘焙的几何体", GH_ParamAccess.list);
            pManager.AddBooleanParameter("isBake", "B", "是否进行烘焙", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
           //TODO 
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Layer La = null;
            List<GeometryBase> Geoms = new List<GeometryBase>();
            bool IsBake = false;

            if (!DA.GetData(0, ref La)) return;
            if (!DA.GetDataList(1, Geoms)) return;
            if (!DA.GetData(2, ref IsBake)) return;

            if (!IsBake) return;

            foreach(GeometryBase Geom in Geoms)
            {
                ObjectAttributes Attr = new ObjectAttributes();
                Attr.LayerIndex = La.Index;
                Rhino.RhinoDoc.ActiveDoc.Objects.Add(Geom, Attr);
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
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.bake;
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
            get { return new Guid("f2ca8b21-71b7-4eba-86a4-874d96b4aefd"); }
        }
    }
}