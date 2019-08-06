using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.CutDown
{
    public class CreateGridPoint : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateGridPoint class.
        /// </summary>
        public CreateGridPoint()
          : base("CreateGridPoint", "CreateGridPoint",
              "创建点集，一般需要结合Cplane来使用，主要用于Grasshopper中下料展平",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.CUTDOWNCATATORY)
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
            pManager.AddPointParameter("Point", "Point", "起点，用作待生成点集的起点", GH_ParamAccess.item,Point3d.Origin);
            pManager.AddNumberParameter("ExtendX", "ExtendX", "点与点之间沿世界坐标系X轴方向上的距离", GH_ParamAccess.item,100);
            pManager.AddNumberParameter("ExtendY", "ExtendY", "点与点之间沿世界坐标系Y轴方向上的距离", GH_ParamAccess.item,100);
            pManager.AddIntegerParameter("TotalNum", "TotalNum", "生成点的总的个数", GH_ParamAccess.item,10);
            pManager.AddIntegerParameter("ColumnNum", "ColumnNum", "每一列点的数目", GH_ParamAccess.item,10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Pts", "Pts", "生成的点的集合", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_Point> TempPtsTree = new GH_Structure<GH_Point>();

            Point3d StartPt = Point3d.Origin;
            double ExtendX = default(double), ExtendY = default(double);
            int TotalNum = default(int), ColumnNum = default(int);

            if (!DA.GetData(0, ref StartPt)) return;
            if (!DA.GetData(1, ref ExtendX)) return;
            if (!DA.GetData(2, ref ExtendY)) return;
            if (!DA.GetData(3, ref TotalNum)) return;
            if (!DA.GetData(4, ref ColumnNum)) return;

            double StartX = StartPt.X, StartY = StartPt.Y, StartZ = StartPt.Z,CurrentY = 0;
            double RowNum = TotalNum / ColumnNum;
            int TotalColumn = Convert.ToInt32((TotalNum % ColumnNum == 0) ? RowNum : (RowNum + 1));

            for(int Column = 0; Column < TotalColumn; Column++)
            {
                List<GH_Point> TempList = new List<GH_Point>();
                int leftNum = TotalNum - Column * ColumnNum;
                ColumnNum = leftNum < ColumnNum ? leftNum: ColumnNum;
                CurrentY = StartY;

                for (int j = 0; j < ColumnNum; j++)
                {
                    Point3d Temp_Pt = new Point3d(StartX, CurrentY, StartZ);
                    TempList.Add(new GH_Point(Temp_Pt));
                    CurrentY += ExtendY;
                }
                TempPtsTree.AppendRange(TempList, new GH_Path(Column));
                StartX += ExtendX;
            }
            TempPtsTree.Flatten();
            DA.SetDataList(0, TempPtsTree);
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
                Bitmap originalImg = Properties.Resources.CreatePointGrid;
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
            get { return new Guid("13a593e7-5c50-498a-bb01-42c9584e3dc1"); }
        }
    }
}