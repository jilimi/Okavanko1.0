using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.Hu
{
    public class Hu_RemoveAt : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RemoveAt class.
        /// </summary>
        public Hu_RemoveAt()
          : base("RemoveAt", "RemoveAt",
              "移除第N个元素",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PERSONAL)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Ls", "L", "列表", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Num", "N", "第N各元素", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "R", "列表", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<object> InputList = new List<object>();
            int RemoveNum = 0;

            if (!DA.GetDataList<object>(0, InputList)) return;
            if (!DA.GetData(0,ref RemoveNum)) return;

            List<object> OutputList = new List<object>(InputList); 

            if(RemoveNum < 0)
            {
                RemoveNum = 0;
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Num 不能小于 0");
                return;
            }

            OutputList.RemoveAt(RemoveNum);

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
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.RemoveAtN;
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
            get { return new Guid("13e97fe1-7486-4b6b-aea2-994cc48c75eb"); }
        }
    }
}