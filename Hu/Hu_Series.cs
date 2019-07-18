//条件编译
#if Personal
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace CSCECDEC.Plugin.Basic
{
    public class Hu_Series : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Hu_Series class.
        /// </summary>
        public Hu_Series()
          : base("Hu_Series", "Hu_Series",
              "使用字符串生成系列，如0：100：0.1表示生成一个起始为0，终止数为100，步距为0.1的系列",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PERSONAL)
        {
            
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", "注意输入的字符串要符合规则",GH_ParamAccess.item, "0:100:1");
            //该方法已经过时了，Register_GenericParam 与 AddGenetricParameter方法的效果是一样的，对于GH_OutputParamManager是不一样的
            //pManager.Register_GenericParam("RegGeneric", "", "");
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Result", "R", "输出的Series", GH_ParamAccess.list);
            //TODO
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Text = "";
            if(!DA.GetData(0, ref Text))return;

            string[] Result = Regex.Split(Text, @":|：");
            if (Result.Length != 3)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "输入的字符串格式不正确");
                return;
            }
            double[] ProcessResult = this.ConvertToNormalFormat(Result);
            double TempNum = ProcessResult[0];
            double InCrement = ProcessResult[2];
            double Count = ProcessResult[1] / InCrement;

            List<double> OutputList = new List<double>();

            for (int Index = 0; Index < Count; Index++)
            {
                TempNum = TempNum + InCrement;
                OutputList.Add(TempNum);
            }

            DA.SetDataList(0, OutputList);
        }
        ///<summary>
        ///return -1 indicate it is not an good format;
        /// 
        /// </summary>
        private double[] ConvertToNormalFormat(string[] StrArr)
        {
            double Start, Sum, InCrement;
            try
            {
                Start = Convert.ToDouble(StrArr[0]);

            }catch
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "输入的字符串格式不正确，使用0作为起始默认值");
                Start = 0;
            }
            try
            {
                Sum = Convert.ToDouble(StrArr[1]);

            }
            catch
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "输入的字符串格式不正确，使用10作为数目的默认值");
                Sum = 10;
            }
            try
            {
                InCrement = Convert.ToDouble(StrArr[2]);

            }
            catch
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "输入的字符串格式不正确，使用0.1作为默认步距值");
                InCrement = 0.1;
            }
            return new double[] { Start, Sum, InCrement };
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
                Bitmap originalImg = Properties.Resources.SeriesString;
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
            get { return new Guid("724a76c3-c969-408b-aae2-b6682498c765"); }
        }
    }
}
#endif