using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;
using System.Drawing;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;
namespace CSCECDEC.Okavango.Basic
{
    public class UniformCrvDirect : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the UniformLineDirect class.
        /// </summary>
        public UniformCrvDirect()
          : base("UniformDirection", "UniformDirection",
              "对多根靠的比较近统一方向",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
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
            pManager.AddCurveParameter("Curves", "Cs", "需要统一方向的曲线", GH_ParamAccess.list);
            pManager.AddPointParameter("Point", "P", "线的起点", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Reverse", "R", "是否反转", GH_ParamAccess.item,false);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Result", "R", "统一方向的曲线", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> _CurveList = new List<Curve>();
            List<Curve> ProcessCurve = new List<Curve>();
            List<Curve> _OutputCurveList = new List<Curve>();
            GH_Point ReferencePoint = default(GH_Point);
            GH_Boolean IsReverse = default(GH_Boolean);

            if (!DA.GetDataList<Curve>(0, _CurveList)) return;
            if (!DA.GetData(1, ref ReferencePoint)) return;
            if (!DA.GetData(2, ref IsReverse)) return;

            ProcessCurve =  _CurveList.Select(item => item.DuplicateCurve()).ToList();

            for(int Index = 0; Index < ProcessCurve.Count; Index++)
            {
                Curve CrvItem = _CurveList[Index];
                if(CrvItem.PointAtStart.DistanceTo(ReferencePoint.Value) > CrvItem.PointAtEnd.DistanceTo(ReferencePoint.Value))
                    CrvItem.Reverse();
                if (IsReverse.Value) CrvItem.Reverse();
                _OutputCurveList.Add(CrvItem);
            }
            DA.SetDataList(0, _OutputCurveList); 
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
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.UniformCrv;
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
            get { return new Guid("c8956de3-36cb-4ec2-9b79-cf4c1ea26c31"); }
        }
    }
}