using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.CutDown
{
    public class Point2SurveryData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Point2SurveryData class.
        /// </summary>
        public Point2SurveryData()
          : base("Point2SurveryData", "Point2SurveryData",
              "生成可供现场测量队伍使用的标准点位坐标字符串（单位：米）",
             GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.CUTDOWNCATATORY)
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
            pManager.AddPointParameter("Point", "Point", "点数据", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Fix", "Fix", "精确到的小数点位数,默认值为3", GH_ParamAccess.item,3);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("String", "String", "坐标字符串", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d InputPt = default(Point3d);
            int Precise = 0;

            if (!DA.GetData(0, ref InputPt)) return;
            if (!DA.GetData(1, ref Precise)) return;

            Rhino.UnitSystem CurrentUnit =  Rhino.RhinoDoc.ActiveDoc.ModelUnitSystem;
            string SurveyPtStr = "";
            switch (CurrentUnit)
            {
                case UnitSystem.Millimeters:
                    SurveyPtStr = Math.Round(InputPt.Y / 1000, Precise) + "," + Math.Round(InputPt.X / 1000, Precise) + ","+Math.Round(InputPt.Z / 1000, Precise);
                    break;
                case UnitSystem.Centimeters:
                    SurveyPtStr = Math.Round(InputPt.Y / 100, Precise) + "," + Math.Round(InputPt.X / 100, Precise) + "," + Math.Round(InputPt.Z / 100, Precise);
                    break;
                case UnitSystem.Meters:
                    SurveyPtStr = Math.Round(InputPt.Y , Precise) + "," + Math.Round(InputPt.X , Precise) + "," + Math.Round(InputPt.Z , Precise);
                    break;
                default:
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "不可识别的模型空间单位");
                    break;
            }
            DA.SetData(0, SurveyPtStr);
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
                Bitmap originalImg = Properties.Resources._2SurveyPointsString;
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
            get { return new Guid("bfce8061-d6b4-4e1e-86d8-d16c033b9f4d"); }
        }
    }
}