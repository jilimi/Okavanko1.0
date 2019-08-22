using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.BIM
{
    public class RemoveDataByKey : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ClearDictByKey class.
        /// </summary>
        public RemoveDataByKey()
          : base("RemoveDataByKey", "RemoveDataByKey",
              "根据Key值，清除指定字典数据",
             Setting.PLUGINNAME, Setting.BIMCATATORY)
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
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new Grasshopper.Kernel.Attributes.GH_ComponentAttributes(this);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "含有用户自定义数据的几何体", GH_ParamAccess.item);
            pManager.AddTextParameter("Key", "K", "附加信息的键", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "含有用户自定义数据的几何体", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {


            GeometryBase Geom = default(GeometryBase);
            string InfoKey = null;

            if (!DA.GetData(0, ref Geom)) return;
            if (!DA.GetData(1, ref InfoKey)) return;
            //此处务必对几何体进行拷贝，否则将会影响到前面的几何体
            GeometryBase TempGeom = Geom.Duplicate();
            try
            {
                if (TempGeom.UserDictionary.ContainsKey(InfoKey))
                {
                    TempGeom.UserDictionary.Remove(InfoKey);
                    DA.SetData(0, TempGeom);
                }else
                {
                    throw new Exception("在几何体中未找到Key值为："+InfoKey+ " 的字段");
                }

            }catch(Exception ex)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
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
                Bitmap originalImg = Properties.Resources.RemoveCustomData;
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
            get { return new Guid("ab7b45f7-e21f-4215-b2c6-ce8b2f0a7465"); }
        }
    }
}