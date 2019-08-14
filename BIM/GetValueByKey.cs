using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.BIM
{
    public class GetUserString : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetUserString class.
        /// </summary>
        public GetUserString()
          : base("GetValueByKey", "GetValueByKey",
              "用于获取几何体的附加信息",
             GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BIMCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
        public override void CreateAttributes()
        {
            //   base.CustomAttributes(this,3);
            m_attributes = new Hu_Attribute(this);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "需要獲取信息的物體", GH_ParamAccess.item);
            pManager.AddTextParameter("Key", "K", "需要获取信息的键", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Value", "V", "获取的信息", GH_ParamAccess.item);
        }
        protected override string HelpDescription
        {
            get
            {
                return "根据给定的'键'获取用户自定义数据的‘值’如有一条边AB的长度为24m，那么用'键-值'对的方式我们可以将其表示为:‘AB:24m’,那么AB为'键',24为值";
            }
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryBase Geom  =default(GeometryBase);
            string Key = null;
            string Value = null;

            if(!DA.GetData(0, ref Geom))return;
            if(!DA.GetData(1, ref Key))return;

            try
            {
                Value = Geom.UserDictionary.GetString(Key);
                DA.SetData(0, Value);
            }catch
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The Key " + Key + " is not found");
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
                Bitmap originalImg = Properties.Resources.FetchDictDataFromGeom;
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
            get { return new Guid("{f5cccadb-d106-4095-8e37-e225f68cb4b2}"); }
        }
    }
}