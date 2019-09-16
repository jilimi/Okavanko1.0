using System;
using System.Collections.Generic;

using Rhino.DocObjects;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Attributes;

using Rhino.Geometry;
using System.Drawing;

using CSCECDEC.Okavango.Params;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class BakeGeometry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BakeGeometry class.
        /// </summary>
        public BakeGeometry()
          : base("BakeGeometry", "BakeGeometry",
              "将物体烘焙到图层当中",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.senary;
            }
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
            pManager.AddGeometryParameter("Geometry", "G", "需要烘焙的几何体", GH_ParamAccess.list);
            pManager.AddGenericParameter("Attributes", "A", "需要烘培物体的属性", GH_ParamAccess.item);
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
            List<IGH_GeometricGoo> Geoms = new List<IGH_GeometricGoo>();
            bool IsBake = false;
            Rhino.DocObjects.ObjectAttributes Attr = null;

            if (!DA.GetDataList(0, Geoms)) return;
            if (!DA.GetData(1, ref Attr)) return;
            if (!DA.GetData(2, ref IsBake)) return;

            if (!IsBake) return;

            foreach(IGH_GeometricGoo Geom in Geoms)
            {
                if(Geom != null)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Add(GH_Convert.ToGeometryBase(Geom), Attr);
                }
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