﻿using System;
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

using CSCECDEC.Plugin.Util;
using CSCECDEC.Plugin.Attribute;

namespace CSCECDEC.Plugin.CutDown
{
    public class CreateExcelFile : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateExcelFile class.
        /// </summary>
        /// 
        bool IsOutput = false;
        string DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "GrasshopperOutPut";
        public CreateExcelFile()
          : base("ExportExcel", "ExportExcel",
              "将输入的数据导出Excel,生成的文件默认位于桌面的GrasshopperOutPut文件夹",
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
        public override void CreateAttributes()
        {
            //   base.CustomAttributes(this,3);
            //m_attributes = new ButtonAttribute(this,Do_ButtonMouseDown,Do_ButtonMouseUp,"HelloWorld");
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
        protected override string HelpDescription
        {
            get
            {
                return "将给定数据以Excel表格的形式输出,每一个叶子节点生成一个Sheet，每一个叶子节点（List）下面的Item生成一行，行里面的数据请用','或者';'隔开，以生成单元格";
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FileName", "F", "导出的Excel文件的名称", GH_ParamAccess.item);
            pManager.AddTextParameter("SheetName", "S", "Excel表格名称", GH_ParamAccess.list);
            pManager.AddTextParameter("Data", "D", "需要导出的数据，每个树枝会生成一个表格，有多少个树枝就会生成多少个表格,每个数据之间用逗号隔开", GH_ParamAccess.tree);
            pManager.AddTextParameter("FilePath", "P", "保存输出文件的路径,如果不输入", GH_ParamAccess.item, DefaultFolder);

            pManager[1].Optional = true;
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
            List<GH_String> ReConstructSheetNames = new List<GH_String>();

            string FileName = null;
            string FilePath = null;

            if(!DA.GetData(0, ref FileName))return;
            if(!DA.GetDataList<GH_String>(1, SheetNames))return;
            if(!DA.GetDataTree<GH_String>(2, out BodyData))return;
            if(!DA.GetData(3, ref FilePath))return;

            ExcelPackage ExcelFile = new ExcelPackage();
            ExcelWorkbook Wb = ExcelFile.Workbook;

            if (!this.IsOutput) return;

            //Write Excel
            int BodyBranchsCount = BodyData.Branches.Count;
            List<GH_String> ReComputeSheetNames = this.ReConstructSheetNames(BodyData.Branches.Count, SheetNames);
            for (int Index = 0; Index < BodyData.Branches.Count; Index++)
            {
                string SheetName = ReComputeSheetNames[Index].Value;
                ExcelWorksheet Ws = Wb.Worksheets.Add(SheetName);

                Ws.Cells.Style.Font.Size = 10;
                Ws.Cells.Style.Font.Name = "宋体";
                Ws.Cells[1, 1, 100, 100].AutoFitColumns();
                Ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

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