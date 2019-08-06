using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;
using CSCECDEC.Plugin.Attribute;

namespace CSCECDEC.Plugin.BIM
{
    public class GetValues : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetDictValues class.
        /// </summary>
        public GetValues()
          : base("GetValues", "GetValues",
              "获取几何体的值",
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
            pManager.AddGeometryParameter("Geom", "G", "含有用户自定义数据的几何体", GH_ParamAccess.item);
        }
        public override void CreateAttributes()
        {
            //   base.CustomAttributes(this,3);
            m_attributes = new Hu_Attribute(this);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Values", "V", "获取的值", GH_ParamAccess.list);
        }
        protected override string HelpDescription
        {
            get
            {
                return "获取用户自定义数据的所有的‘值’,如有一条边AB的长度为24m，那么用'键-值'对的方式我们可以将其表示为:‘AB:24m’,那么AB为'键',24为值";
            }
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryBase Geom = default(GeometryBase);

            if(!DA.GetData(0, ref Geom))return;
            List<object> TempOutPutString = Geom.UserDictionary.Values.ToList();
            List<Grasshopper.Kernel.Types.GH_String> OutPutStringList = (from item in TempOutPutString
                                                                    where item as string != null
                                                                    select new Grasshopper.Kernel.Types.GH_String(item.ToString())).ToList();
            if (OutPutStringList.Count == 0) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "该物体没有任何几何信息");

            DA.SetDataList(0, OutPutStringList);
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
                Bitmap originalImg = Properties.Resources.Value;
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
            get { return new Guid("a569b372-8b50-4a12-b0cd-046e578cb4f4"); }
        }
    }
}