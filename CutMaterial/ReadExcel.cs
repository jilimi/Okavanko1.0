using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Utils;

using CSCECDEC.Okavango.Util;
using CSCECDEC.Okavango.Attribute;
using CSCECDEC.Okavango.Control;

namespace CSCECDEC.Okavango.CutDown
{
    public class ReadExcelFile : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ReadExcelFile class.
        /// </summary>
        /// 
        bool IsOutput = false;
        string DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public ReadExcelFile()
          : base("ReadExcelFile", "ReadExcelFile",
              "读取Excel表格中的数据",
              Setting.PLUGINNAME, Setting.CUTDOWNCATATORY)
        {
            
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
        public override void CreateAttributes()
        {
            //此时不能对其进行调用因为这个时候Attributes是空值
            /*
            RectangleF Bounds = base.Attributes.Bounds;

            */
            ButtonControl Btn1 = new ButtonControl(this,"Output");
            //RadioButtonControl RadioBtn = new RadioButtonControl(this, "ClickMe");
            Btn1.MouseDownCallback = Do_ButtonMouseDown;
            Btn1.MouseUpCallback = Do_ButtonMouseUp;
           // RadioBtn.ClickCallback = Do_ClickMe;
            Hu_AttributeWithControl Hu_Attr = new Hu_AttributeWithControl(this,new List<HuControl> {Btn1});
            NormalAttributeWithControl Attr = new NormalAttributeWithControl(this,new List<HuControl> {Btn1});
            if (Properties.Settings.Default.Is_Hu_Attribute)
            {
                m_attributes = Hu_Attr;
            }else
            {
                m_attributes = Attr;
            }
        }
        public void Do_ButtonMouseDown(IGH_Component Component)
        {
            this.IsOutput = true;
            this.ExpireSolution(true);
        }
        public void Do_ButtonMouseUp(IGH_Component Component)
        {
            this.IsOutput = false;
            this.ExpireSolution(true);
        }
        private void Do_ClickMe(IGH_Component Component)
        {
            Rhino.RhinoApp.WriteLine("Hello World");
        }
        protected override string HelpDescription
        {
            get
            {
                return "读取Excel表格中的数据";
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File", "F","文件名称", GH_ParamAccess.item);
            pManager.AddTextParameter("SheetName", "S", "Excel表格名称", GH_ParamAccess.list,new List<string>());
            pManager.AddIntegerParameter("StartRow", "R", "表格起始", GH_ParamAccess.item,0);
            pManager.AddIntegerParameter("StartColumn", "C", "需要导出的数据，每个树枝会生成一个表格，有多少个树枝就会生成多少个表格,每个数据之间用逗号隔开", GH_ParamAccess.item,0);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //TODO
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GH_String> SheetNames = new List<GH_String>();
            GH_Structure<GH_String> BodyData = new GH_Structure<GH_String>();

            string FileName = null;
            int StartRow = 0;
            int StartColumn = 0;

            if(!DA.GetData(0, ref FileName))return;
            DA.GetDataList<GH_String>(1, SheetNames);
            DA.GetData(2, ref StartRow);
            DA.GetData(3, ref StartColumn);

            ExcelPackage ExcelFile = new ExcelPackage();
            ExcelWorkbook Wb = ExcelFile.Workbook;

            if (!this.IsOutput) return;

            //Write Excel
            int BodyBranchsCount = BodyData.Branches.Count;
            if (SheetNames.Count != 0)
            {
                ReComputeSheetNames = this.ReConstructSheetNames(BodyData.Branches.Count, SheetNames);
            }else
            {
                ReComputeSheetNames = new List<GH_String>();
            }
            for (int Index = 0; Index < BodyData.Branches.Count; Index++)
            {
                string SheetName = ReComputeSheetNames.Count == 0?BodyData.Paths[Index].ToString():ReComputeSheetNames[Index].Value;
                ExcelWorksheet Ws = Wb.Worksheets.Add(SheetName);


                IList<GH_String> DataBranchs = BodyData.Branches[Index];
                List<string> BodyTextList = DataBranchs.Select(item => item.Value).ToList();
                Util.ExcelWriter.WriteBodyValue(BodyTextList,ref Ws);
            }
            try
            {

                string OutPutFilePath = CreateSavePath(FilePath) +"\\"+FileName + ".xlsx"; 
                System.IO.FileInfo FileInfo = new System.IO.FileInfo(OutPutFilePath);
                if (System.IO.File.Exists(OutPutFilePath)) System.IO.File.Delete(OutPutFilePath);
                ExcelFile.SaveAs(FileInfo);
                ExcelFile.Dispose();

            }catch(System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "错误", System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Error); ;
                ExcelFile.Dispose();
            }
        }
        private List<GH_String> ReConstructSheetNames(int SheetNum,List<GH_String> SheetNames)
        {
            List<GH_String> SheetNamesResult = new List<GH_String>();
            if(SheetNum > SheetNames.Count)
            {

                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "表格的页数与表单的名称数目不相等");
                int TileNum = SheetNum - SheetNames.Count;
                SheetNamesResult.AddRange(SheetNames);
                SheetNamesResult.AddRange(Enumerable.Repeat(SheetNames[SheetNames.Count - 1], TileNum));
                return SheetNamesResult;

            }else if(SheetNum < SheetNames.Count)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "表格的页数与表单的名称数目不相等");
                SheetNamesResult.AddRange(SheetNames.Take(SheetNum));
                return SheetNamesResult;
            }else
            {
                return SheetNames;
            }
        }
        private string CreateSavePath(string FilePath)
        {

            bool IsPathExist = System.IO.Directory.Exists(FilePath);
            if (IsPathExist) return FilePath;
            if (!System.IO.Directory.Exists(DefaultFolder)) System.IO.Directory.CreateDirectory(DefaultFolder);
            return DefaultFolder;
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
                Bitmap originalImg = Properties.Resources.ExportExcel;
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
            get { return new Guid("b2b603ff-650e-45b3-9b46-c4bb51149828"); }
        }
    }
}