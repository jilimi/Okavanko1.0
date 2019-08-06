using System;
using System.Collections.Generic;

using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using System.Drawing;

namespace CSCECDEC.Plugin.Basic
{
    public class ClassifyDataByTree : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ClassifyByTree class.
        /// </summary>
        public ClassifyDataByTree()
          : base("ClassifyDataByTree", "ClassifyDataByTree",
              "对数据列按一定的区间进行分组",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Data", "D", "需要进行分类的数据", GH_ParamAccess.list);
            pManager.AddTextParameter("Intervals", "Is", "分类区间，每个分类区间为一个树枝，数值内的大值与小值用逗号隔开，如9,20表示一个大于9小于20的分类区间", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Result", "R", "已经分好类的数据", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_String> ClassifyTree = new GH_Structure<GH_String> ();

            List<double> InputList = new List<double>();

            GH_Structure<GH_Number> OutputTree = new GH_Structure<GH_Number>();

            if(!DA.GetDataList<double>(0, InputList))return;
            if(!DA.GetDataTree<GH_String>(1, out ClassifyTree))return;
            List<List<double>> IntervalDataList = new List<List<double>>();
            try
            {
                IntervalDataList = Convert2ListInterval(ClassifyTree);
            }catch(Exception e){
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
            }
            int IntervalCount = IntervalDataList.Count;
            for (int Index = 0; Index < IntervalCount; Index++)
            {

                List<double> IntervalBranch = IntervalDataList[Index];
                 
                double min = IntervalBranch.Min();
                double max = IntervalBranch.Max();

                List<double> TempList = (from item in InputList
                                         where item >= min && item < max
                                         select item).ToList();
                TempList.OrderByDescending(item => item).ToList();
                TempList.Reverse();

                OutputTree.AppendRange(TempList.Select(item=>new GH_Number(item)).ToList(), new GH_Path(Index));
            }
            DA.SetDataTree(0, OutputTree);

        }
        private List<List<double>> Convert2ListInterval(GH_Structure<GH_String> TreeInterval)
        {

            List<List<double>> IntervalList = new List<List<double>>();
            int TreeCount = TreeInterval.Branches.Count;
            for (int Index = 0; Index < TreeCount; Index++)
            {
                var BranchData = TreeInterval.Branches[Index];
                List<double> Interval = new List<double>();
                foreach (GH_String item in BranchData)
                {
                    string itemData = item.Value;
                    string[] string_items = itemData.Split(',');
                    string_items.Select(data => Convert.ToDouble(data)).ToList();
                    Interval.AddRange(string_items.Select(data => Convert.ToDouble(data)).ToList());
                }
                IntervalList.Add(Interval);
            }
            return IntervalList;
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
                Bitmap originalImg = Properties.Resources.ClassifyByTree;
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
            get { return new Guid("e2da9a5b-f593-4f72-9938-8d540f87a2b2"); }
        }
    }
}