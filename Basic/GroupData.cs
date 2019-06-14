using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace CSCECDEC.Plugin.Basic
{
    public class GroupData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GroupData class.
        /// </summary>
        public GroupData()
          : base("GroupData", "数据成组",
              "对给定的数据进行分组",
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
            pManager.AddNumberParameter("Nums", "Ns", "需要进行分组的数据列表", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Result", "R", "被分好组的数据", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> InputList = new List<double>();
            List<string> OutputList = new List<string>();

            if(!DA.GetDataList<double>(0, InputList))return;

            var Temp = InputList.GroupBy(item => item).ToList();
            for (int Index = 0; Index < Temp.Count; Index++)
            {
                var GroupData = Temp[Index];
                OutputList.Add(GroupData.Key.ToString() + "," + GroupData.Count().ToString());
            }
            DA.SetDataList(0, OutputList);
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
                Bitmap originalImg = Properties.Resources.DataGroup;
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
            get { return new Guid("af49aa52-1ece-497b-b53b-0e6c02a34b6b"); }
        }
    }
}