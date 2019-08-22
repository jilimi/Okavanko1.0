using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Drawing;
using GH_IO.Serialization;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class FindPointClosetCurve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PointClosetCurve class.
        /// </summary>
        ///0 is default : Nearst
        ///1 is custom: fareast;
        int FindType = 0;
        public FindPointClosetCurve()
          : base("PointClosetCurve", "PointClosetCurve",
              "找出距离点最近的N根曲线",
             Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
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
            pManager.AddCurveParameter("Curves", "C", "曲线集合", GH_ParamAccess.list);
            pManager.AddPointParameter("Point", "P", "坐标点", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Num", "N", "需要查找的数目", GH_ParamAccess.item,1);
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {

        }
        public void AppendCustomMenuItem(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Nearest", ChangeFindType, true, this.FindType == 0 ? true : false);
            Menu_AppendItem(menu, "Farest", ChangeFindType, true, this.FindType == 1 ? true : false);
        }

        private void ChangeFindType(object sender, EventArgs e)
        {
            ToolStripMenuItem MenuItem = sender as ToolStripMenuItem;
            string Text = MenuItem.Text;
            MenuItem.Checked = true;
            switch (Text)
            {
                case "Nearest":
                    this.FindType = 0;
                    break;
                case "Farest":
                    this.FindType = 1;
                    break;
                default:
                    this.FindType = 0;
                    break;
            }
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("FindType", this.FindType);
            return base.Write(writer);
        }
        public override bool Read(GH_IReader reader)
        {
            this.FindType = reader.GetInt32("FindType");
            return base.Read(reader);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Result", "R", "计算结果", GH_ParamAccess.list);
        }
        
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //字典立面的Key必须唯一，如何保证TempDict的Key值唯一？
            //调换Curve和Dict的顺序
            //这里存在的一个bug
            Point3d Pt = default(Point3d);
            List<Curve> Crvs = new List<Curve>();
            int Num = 1;
            Dictionary<Curve, double> _TempDict = new Dictionary<Curve, double>();

            if (!DA.GetDataList<Curve>(0, Crvs)) return;
            if (!DA.GetData(1,ref Pt)) return;
            if (!DA.GetData(2,ref Num)) return;

            if (Num == 0) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Num为0");

            double Param = 0;

            foreach(var Crv in Crvs)
            {
                if(Crv.ClosestPoint(Pt,out Param))
                {
                    Point3d TempPt = Crv.PointAt(Param);
                    double Dist = Pt.DistanceTo(TempPt);
                    _TempDict.Add(Crv, Dist);
                }
            }
            var ResultDict = _TempDict.OrderByDescending(item => -item.Value).ToDictionary(item => item.Key, item => item.Value);
            List<Curve> TempCrvs = ResultDict.Keys.ToList();
            List<Curve> ResultCrvs = new List<Curve>();
            if(TempCrvs.Count < Math.Abs(Num))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Num 数值过大");
                return;
            }
            if (this.FindType == 0) {
                ResultCrvs = TempCrvs.GetRange(0, Math.Abs(Num));
            }else
            {
                ResultCrvs = TempCrvs.GetRange(TempCrvs.Count - Num - 1, TempCrvs.Count);
            }
            DA.SetDataList(0, ResultCrvs);
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
                Bitmap originalImg = Properties.Resources.FindClosetCrvs;
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
            get { return new Guid("64040c71-fff8-4ca1-a7f1-efd435b4606c"); }
        }
    }
}