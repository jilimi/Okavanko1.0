using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.Basic
{
    public class CreateSolidByOffsetSrf : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OffsetSolid class.
        /// </summary>
        public CreateSolidByOffsetSrf()
          : base("OffsetSolid", "OffsetSolid",
              "通过偏移面创建几何实体",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quinary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "待偏移的面", GH_ParamAccess.item);
            pManager.AddNumberParameter("Distance", "D", "面偏移的距离", GH_ParamAccess.item,4);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "通过偏移生成的几何体", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep[] a_Blend = new Brep[] { };
            Brep[] o_Wall = new Brep[] { };

            Brep BrepItem = null;
            double o_Distance = 4;
            List<Brep> OutputLs = new List<Brep>();

            if (!DA.GetData(0, ref BrepItem)) return;
            if (!DA.GetData(1, ref o_Distance)) return;

            if(o_Distance < 0)
            {
                o_Distance = Math.Abs(o_Distance);
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "面偏移的距离不能为负数");
                o_Distance = 0;
            }
            OutputLs = Brep.CreateOffsetBrep(BrepItem, o_Distance, true, false, 0.001, out a_Blend, out o_Wall).ToList();

            DA.SetDataList(0, OutputLs);
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
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.CreateSolidByOffset;
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
            get { return new Guid("4c5b48bf-dbd1-45bb-a797-03dc41d34a3a"); }
        }
    }
}