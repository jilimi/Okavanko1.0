using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Drawing;

namespace CSCECDEC.Plugin.Basic
{
    public class GroupByType : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GroupByType class.
        /// </summary>
        public GroupByType()
          : base("GroupByType", "GroupByType",
              "根据物体的类型进行分类",
             GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "需要分类的几何体", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Result", "R", "已经分好类的几何体", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IGH_GeometricGoo> InputList = new List<IGH_GeometricGoo>();
            GH_Structure<IGH_GeometricGoo> OutputTree = new GH_Structure<IGH_GeometricGoo>();

            if (!DA.GetDataList<IGH_GeometricGoo>(0, InputList))return;

            var Temp = InputList.GroupBy(item => item.GetType()).ToList();
            for (int Index = 0; Index < Temp.Count; Index++)
            {
                OutputTree.AppendRange(Temp[Index], new GH_Path(Index));
            }

            DA.SetDataTree(0, OutputTree);
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
                Bitmap originalImg = Properties.Resources.ClusterPoints;
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
            get { return new Guid("f74bc6ec-e03f-4e99-ad68-38c930345006"); }
        }
    }
}