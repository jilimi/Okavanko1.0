using System;
using System.Drawing;
using System.IO;
//This namespace contain a lot of predefine Collection;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;
using GH_IO.Serialization;
using System.Text.RegularExpressions;
using System.Linq;
using Grasshopper.Kernel.Types;
//using GH_IO;

namespace CSCECDEC.Plugin.BIM
{
    /// <summary>
    /// 
    /// </summary>
    public class SetValue : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AddDataToObject class.
        /// </summary>
       // GH_Archive archive = new GH_Archive();
        public SetValue()
          : base("SetValue", "给几何体附加信息",
              "为几何体（点除外）添加额外的信息,'编号'为预设键值，请不要作为键值输入",
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
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "需要添加信息的几何体", GH_ParamAccess.item);
            pManager.AddTextParameter("Key", "K", "附加信息的键", GH_ParamAccess.item);
            pManager.AddTextParameter("Value", "V", "附加信息的值", GH_ParamAccess.item);
        }
        protected override string HelpDescription
        {
            get
            {
                return "为给定几何体设置用户自定义数据信息,不适用于点";
            }
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void BeforeSolveInstance()
        {
           
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "已经附加信息的几何体", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryBase Geom = default(GeometryBase);
            string InfoKey = null;
            string InfoValue = null;

            if(!DA.GetData(0, ref Geom))return;
            if(!DA.GetData(1, ref InfoKey))return;
            if(!DA.GetData(2, ref InfoValue))return;

            //这句代码至关重要
            GeometryBase TempGeom = Geom.Duplicate();

            if((TempGeom is Rhino.Geometry.Point))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "目前尚不支持Point的户自定义数据写入");
                return;
            }
            //如果不存在
            if (TempGeom.UserDictionary.Keys.Contains<string>(InfoKey))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "几何体中包含同名‘键’值");
            }else
            {
                TempGeom.UserDictionary.Set(InfoKey, InfoValue);
            }
            DA.SetData(0, TempGeom);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                 Bitmap newImage = new Bitmap(24, 24);
                 Bitmap originalImg = Properties.Resources.BindDictDataToGeom;
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
            get { return new Guid("{d6fec195-b9c2-44ab-b785-c9626b910ceb}"); }
        }
    }
}