using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace CSCECDEC.Plugin.BIM
{
    public class FindGeometryByValue : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetGeometryByValue class.
        /// </summary>
        public FindGeometryByValue()
          : base("GetGeometryByValue", "GetGeometryByValue",
              "根据给定的物体信息获取对应的几何体",
             GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BIMCATATORY)
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
            pManager.AddGeometryParameter("Geom", "G", "待查找的几何体范围", GH_ParamAccess.list);
            pManager.AddTextParameter("Key", "K", "Key值，必须单个，如果是多个将查找失败", GH_ParamAccess.item);
            pManager.AddTextParameter("Value", "V", "带查找的Value值", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "包含有Value数值的几何体", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GeometryBase> Geoms = new List<GeometryBase>();
            List<GeometryBase> OutputGeoms = new List<GeometryBase>();
            List<string> TempStrList = new List<string>();

            string InfoKey = null;
            string InfoValue = null;

            if (!DA.GetDataList(0, Geoms)) return;
            if (!DA.GetData(1, ref InfoKey)) return;
            if (!DA.GetData(2, ref InfoValue)) return;

            List<GeometryBase> TempGeoms = new List<GeometryBase> (Geoms);

            for(int Index = 0; Index < TempGeoms.Count; Index++)
            {
                GeometryBase _Geom = Geoms[Index];
                TempStrList = _Geom.UserDictionary.Values.Select(item=>item.ToString()).ToList();
                if (TempStrList.Contains(InfoValue)) OutputGeoms.Add(_Geom);
            }

            if (OutputGeoms.Count == 0) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "未检索到任何符合要求的物体");
            DA.SetDataList(0, OutputGeoms);
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
                System.Drawing.Bitmap newImage = new System.Drawing.Bitmap(24, 24);
                System.Drawing.Bitmap originalImg = Properties.Resources.SearchGeometry;
                //Graphic 沒有public的構造函數，不能使用new運算符，衹能通過其他方式創建graphic
                System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage((System.Drawing.Image)newImage);
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
            get { return new Guid("1a48c84d-3d5f-4a68-9cf3-5d22db7786f9"); }
        }
    }
}