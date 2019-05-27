#if Hudson
using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;
using System.Windows.Forms;

namespace CSCECDEC.Plugin.CutDown
{
    public class ExcelTitleGenerate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExcelTitleGenerate class.
        /// </summary>
        string Identity = "Line";
        public ExcelTitleGenerate()
          : base("ExcelTitle", "生成表头行",
              "快速生成Excel表头,如果是输出长度数据的表头，请在Context Menu选择Line，如果是点坐标请选择Point‘",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.CUTDOWNCATATORY)
        {
            this.Message = "线表头";
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
        public override bool AppendMenuItems(ToolStripDropDown menu)
        {
            base.Menu_AppendObjectName(menu);
            base.Menu_AppendEnableItem(menu);
            GH_DocumentObject.Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "线类型表头",LineMenuItemClick);
            Menu_AppendItem(menu, "点类型表头",PointMenuItemClick);
            base.Menu_AppendObjectHelp(menu);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineMenuItemClick(object sender, EventArgs e)
        {
            this.Identity = "Line";
            this.Message = "线表头";
            this.ExpireSolution(true);

        }
        private void PointMenuItemClick(object sender,EventArgs e)
        {
            this.Identity = "Point";
            this.Message = "点表头";
            this.ExpireSolution(true);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Prefix", "Prefix", "单元格内容的前缀,如果是生成点表头请输入(多行形式的x,y,z)", GH_ParamAccess.item,"L");
            pManager.AddIntegerParameter("Count", "Count", "表头单元格的列数", GH_ParamAccess.item,1);
            pManager.AddTextParameter("Suffix", "Suffix", "表头单元格内容的后缀", GH_ParamAccess.item, "");

          
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Title", "Title","生成的表头",GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Prefix = "", Subfix = "", Title = "";
            int ColumnNum = 1;

            if (!DA.GetData(0, ref Prefix)) return;
            if (!DA.GetData(1, ref ColumnNum)) return;
            if (!DA.GetData(2, ref Subfix)) return;

            if(ColumnNum <= 0)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Num必须为大于1的整数");
                return;
            }
            if(this.Identity == "Line")
            {
                Title = "编号"+ ","+String.Join(",",Enumerable.Range(0, ColumnNum).Select(item => Prefix + item + Subfix).ToList());
            }else
            {
                Title = "编号" +","+String.Join(",",Enumerable.Range(0, ColumnNum).Select(item => "X(m),Y(m),Z(m)").ToList());
            }
            DA.SetData(0, Title);
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
                Bitmap originalImg = Properties.Resources.ExcelTitle;
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
            get { return new Guid("20c6c08c-3137-450c-951e-052283989913"); }
        }
    }
}
#endif