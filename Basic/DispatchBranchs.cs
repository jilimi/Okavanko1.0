using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Linq;
using Rhino.Geometry;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class DispatchBranchs : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DispatchBranchs class.
        /// </summary>
        public DispatchBranchs()
          : base("DispatchBranchs", "DispatchBranchs",
              "对树状数据结构执行类似于List的Dispatch的操作",
              Setting.PLUGINNAME, Setting.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
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
            pManager.AddGenericParameter("Tree", "T", "需要进行dispatch的树状数据结构", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Pattern", "DP", "Dispatch Pattern", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("A", "A", "数值为true的树枝", GH_ParamAccess.tree);
            pManager.AddGenericParameter("B", "TB", "数值为false的数枝", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("IndexA", "IA", "A 树枝所在原树状结构中的位置", GH_ParamAccess.list);
            pManager.AddIntegerParameter("IndexB", "IB", "B 树枝所在原树状结构中的位置", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<IGH_Goo> TreeA = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> TreeB = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> InputTree = new GH_Structure<IGH_Goo>();
            List<GH_Boolean> InputPattern = new List<GH_Boolean>();

            List<int> IA_List = new List<int>();
            List<int> IB_List = new List<int>();

            if (!DA.GetDataTree<IGH_Goo>(0, out InputTree)) return;
            if(!DA.GetDataList<GH_Boolean>(1, InputPattern))return;

            List<GH_Boolean> PatternList = this.PatternConstruct(InputPattern, InputTree.Branches.Count);
            int Branch_A = 0, Branch_B = 0, Index = 0;

            for(;Index < PatternList.Count; Index++)
            {
                if (PatternList[Index].Value)
                {
                    TreeA.AppendRange(InputTree.Branches[Index], InputTree.Paths[Index]);
                    IA_List.Add(Index);
                    Branch_A++;
                }
                else
                {
                    TreeB.AppendRange(InputTree.Branches[Index], InputTree.Paths[Index]);
                    IB_List.Add(Index);
                    Branch_B++;
                }
            }
            DA.SetDataTree(0,TreeA);
            DA.SetDataTree(1, TreeB);
            DA.SetDataList(2, IA_List);
            DA.SetDataList(3, IB_List);

        }
        private List<GH_Boolean> PatternConstruct(List<GH_Boolean> InputList,int TreeCount)
        {
            int InputCount = InputList.Count;
            if (InputCount == 0) return InputList;
            if (InputCount == TreeCount || TreeCount == 0) return InputList;
            int RepeatCount = Convert.ToInt32(TreeCount / InputCount);
            List<GH_Boolean> OutputList = new List<GH_Boolean>();
            int YuNumber = TreeCount % InputCount;
            if (RepeatCount == 0)
            {
                return InputList.GetRange(0, TreeCount);
            }
            else
            {
                for (int Index = 0; Index < RepeatCount; Index++)
                {
                    OutputList.AddRange(InputList);
                }
                List<GH_Boolean> TempList = InputList.GetRange(0, YuNumber);
                OutputList.AddRange(TempList);
                return OutputList;
            }
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
                Bitmap originalImg = Properties.Resources.DispatchTree;
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
            get { return new Guid("d493c8d5-9606-4a73-ad2a-30a310557b5c"); }
        }
    }
}