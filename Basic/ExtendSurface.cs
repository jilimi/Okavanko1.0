using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System.Drawing;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class ExtendSurface : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExcendSurface class.
        /// </summary>
        public ExtendSurface()
          : base("ExtendSurface", "ExtendSurface",
              "延伸曲面,目前只适用于非修剪面，对于修剪面，在对面进行延伸的时候将会自动将面变成非修剪面后再延伸",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
            
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quinary;
            }
        }
        public override void CreateAttributes()
        {
            if (Setting.ISRENDERHUATTRIBUTE) m_attributes = new Hu_Attribute(this);
            else m_attributes = new GH_ComponentAttributes(this);

        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "S", "曲面", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curves", "C", "曲线", GH_ParamAccess.list);
            pManager.AddNumberParameter("Num", "N", "延伸的距离", GH_ParamAccess.item,2);
            pManager.AddBooleanParameter("Smooth", "B", "是否平滑延伸", GH_ParamAccess.item,true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "S", "曲面", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {


            Surface Srf = default(Surface);
            List<Curve> Crvs = new List<Curve>();
            List<BrepTrim> TrimsList = new List<BrepTrim>();
            double Length = 2;
            bool IsSmooth = true;

            if (!DA.GetData(0, ref Srf)) return;
            if (!DA.GetDataList<Curve>(1, Crvs)) return;
            if (!DA.GetData(2, ref Length)) return;
            if (!DA.GetData(3, ref IsSmooth)) return;


            Brep TempBrep = Srf.ToBrep();
            if (!TempBrep.IsSurface || TempBrep == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to extend trimmed surfaces.:line 67");
                return;
            }
            //BrepTrimList is an reference object
            BrepTrimList BrepTrimList = TempBrep.Trims;
            for(int Index = 0; Index < Crvs.Count; Index++)
            {
                BrepTrim TrimItem = this.GetClosetBrepTrim(BrepTrimList, Crvs[Index]);
                TrimsList.Add(TrimItem);

            }
            var TempList = TrimsList.Where(item => item == null || item.TrimType == BrepTrimType.Seam).ToList();
            if (TempList.Count != 0)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to extend trimmed surfaces. line 81");
            }
            ///<summary>
            ///如果输入的Surface为UntrimSurface，那么Extend会对Surface进行延伸，如果输入的Surface为Untrim Surface,Extend
            ///方法就会先对Surface 执行‘untrim’操作，然后再对面进行延伸
            /// </summary>
            for(int Index = 0; Index < TrimsList.Count; Index++)
            {
                 Srf = Srf.Extend(TrimsList[Index].IsoStatus, Length, IsSmooth);

            }
            DA.SetData(0, Srf);
        }
        private BrepTrim GetClosetBrepTrim(BrepTrimList TrimList, Curve Crv)
        {
            foreach (BrepTrim item in TrimList)
            {
                //
                //item.TrimCurve 会出现问题，
                //关于TrimCurve官方的解释如下
                //Gets the Brep.Curves2D 2d curve geometry used by this trim, or null.
                //item.Edge is an 2D Curve
                //
                //这里只对两根线是否相等进行了简单的判断，如果起始点相等，那么就相等，否则不相等
                Curve EdgeCrv = item.Edge.EdgeCurve;

                Point3d TrimSPt = EdgeCrv.PointAtStart;
                Point3d TrimEPt = EdgeCrv.PointAtEnd;

                bool IsMeetEnd = (TrimSPt.EpsilonEquals(Crv.PointAtStart,10) && TrimEPt.EpsilonEquals(Crv.PointAtEnd,10))
                                || (TrimSPt.EpsilonEquals(Crv.PointAtEnd,10) && TrimEPt.EpsilonEquals(Crv.PointAtStart,10));

                if (IsMeetEnd) return item;

            }
            return null;
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
                Bitmap originalImg = Properties.Resources.ExtendSurface;
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
            get { return new Guid("72eb81e6-2236-4cec-a90c-384a6bec1a12"); }
        }
    }
}