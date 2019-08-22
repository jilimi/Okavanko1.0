using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.Basic
{
    public class ListToTree : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ListToTree class.
        /// </summary>
        public ListToTree()
          : base("ListToTree", "将列转换成树状数据",
              "将列表转换成树状数据结构",
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
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new GH_ComponentAttributes(this);

        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List", "L", "需要转换为树状数据结构的列表", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Num", "N", "构成每个树状数据结构的树枝数据的个数", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "R", "转换完成的树状数据结构", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IGH_Goo> SourceData = new List<IGH_Goo>();
            GH_Structure<IGH_Goo> DestData = new GH_Structure<IGH_Goo>();
            List<GH_Integer> BranchNums = new List<GH_Integer>();
            int Path = 0;

            if(!DA.GetDataList<IGH_Goo>(0, SourceData))return;
            if(!DA.GetDataList<GH_Integer>(1, BranchNums))return;

            if(BranchNums.Count == 1)
            {
                int BranchDataNums = Math.Abs(BranchNums[0].Value);
                if(BranchDataNums == 0)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Number不能为0");
                    return;
                }
                for (int Index = 0; Index < SourceData.Count;)
                {
                    if (Index + BranchDataNums > SourceData.Count)
                    {
                        DestData.AppendRange(SourceData.GetRange(Index, SourceData.Count - Index).ToList(), new GH_Path(Path));
                        Path++;
                    }
                    else
                    {
                        DestData.AppendRange(SourceData.GetRange(Index, BranchDataNums).ToList(), new GH_Path(Path));
                        Path++;
                    }
                    Index += BranchDataNums;
                }
                DA.SetDataTree(0, DestData);
            }else
            {
                int BranchTotalNum = BranchNums.Select(item => Math.Abs(item.Value)).Sum();
                if (BranchTotalNum == 0)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Number不能为0");
                    return;
                }
                if (BranchTotalNum == SourceData.Count)
                {
                    int Index = 0;
                    for (int i = 0; i < BranchNums.Count;i++)
                    {
                        DestData.AppendRange(SourceData.GetRange(Index, BranchNums[i].Value).ToList(), new GH_Path(Path));
                        Index += BranchNums[i].Value;
                        Path++;
                    }
                    DA.SetDataTree(0, DestData);
                }
                else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Number数目为1，且Number的数据之和与ListData的总数目不相匹配");
                    return;
                }
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
                Bitmap originalImg = Properties.Resources.List2Tree;
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
            get { return new Guid("55908a8e-ea0a-4fee-b7f9-0de01b637c54"); }
        }
    }
}