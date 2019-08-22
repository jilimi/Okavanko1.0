using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class FindClosetBrep : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FindClosetBrep class.
        /// </summary>
        public FindClosetBrep()
          : base("FindClosetBrep", "FindClosetBrep",
              "找到距离指定点最近的N个曲面",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
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
            pManager.AddBrepParameter("Breps", "B", "Brep集合，最近的面就从这里找出", GH_ParamAccess.list);
            pManager.AddPointParameter("Point", "P", "几何点", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Num", "N", "欲找到面的块数，如该数值为负数，系统自动会将其转换成正数", GH_ParamAccess.item,1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "B", "Breps", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GH_Brep> OutputBreps = new List<GH_Brep>();
            List<GH_Brep> SourceBreps = new List<GH_Brep>();
            int Number = default(int);
            GH_Point SourcePt = default(GH_Point);

            if(!DA.GetDataList<GH_Brep>(0, SourceBreps))return;
            if(!DA.GetData(1, ref SourcePt))return;
            if(!DA.GetData(2, ref Number))return;

            if(SourceBreps.Count < Math.Abs(Number))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "面的块数大于SourceBreps的块数");
                Number = SourceBreps.Count;
            }

            OutputBreps = SourceBreps.OrderByDescending(item => -item.Value.ClosestPoint(SourcePt.Value).DistanceTo(SourcePt.Value)).ToList();

            DA.SetDataList(0, OutputBreps.GetRange(0,Number));

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
                Bitmap originalImg = Properties.Resources.FindClosterBrep;
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
            get { return new Guid("aefe4acd-43ac-4b61-b08c-35087785480d"); }
        }
    }
}