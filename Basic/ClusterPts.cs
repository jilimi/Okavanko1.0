using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Linq;
using Rhino.Geometry;

namespace CSCECDEC.Plugin.Basic
{
    public class ClusterPts : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ClusterPts class.
        /// </summary>
        public ClusterPts()
          : base("ClusterPoints", "ClusterPoints",
              "对点按坐标分量或者距离进行归类",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
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
            pManager.AddPointParameter("Points", "Pt", "需要进行聚类的点集合", GH_ParamAccess.tree);
            pManager.AddNumberParameter("Num", "N", "聚类的阈值，通过设置阈值可以获取不同的效果", GH_ParamAccess.item);
            pManager.AddTextParameter("Sign", "S", "聚类的标志，该数值只能输入x（x 轴）,y（y轴）,z(Z 轴),d(距离)等四个数值，输入其他数值则默认为d", GH_ParamAccess.item);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Result", "R", "已聚类好的点序列", GH_ParamAccess.tree);
           // pManager.AddIntegerParameter("Length", "Length", "数列的长度", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            //不一定要进行初始化对于 out 关键字
            //可能还存在一定的问题
            //
            GH_Structure<GH_Point> InputPtsTree;// = new GH_Structure<GH_Point>();
            GH_Structure<GH_Point> OutputPts = new GH_Structure<GH_Point>();

            double ThreadHold = default(double);
            string Sign = default(string);
            double Result = 0;
            GH_Path BasePath;

            if(!DA.GetDataTree<GH_Point>(0, out InputPtsTree))return;
            if(!DA.GetData(1, ref ThreadHold))return;
            if(!DA.GetData(2, ref Sign))return;

            for(int K = 0; K < InputPtsTree.PathCount;K++)
            {
                //对每一个树枝进行遍历
                //
                //We'll make a copy of the branch, so we can remove items from it
                // without modifying the original. We'll also remove all nulls from
                //
                //重点在这里
                var BranchsPts = new List<GH_Point>(InputPtsTree.Branches[K]);
                //这里有点问题
                BranchsPts.RemoveAll(point => point == null);
                BasePath = InputPtsTree.Paths[K];
                int i = 0;
                if (BranchsPts.Count == 0) return;
                //对每一个Branch内的数据进行遍历处理
                while (BranchsPts.Count > 0)
                {

                    GH_Point i_Pt = BranchsPts[0];
                    List<GH_Point> NewBranch = new List<GH_Point> { i_Pt };
                    BranchsPts.RemoveAt(0);

                    for(int j=0;j < BranchsPts.Count; j++)
                    {
                        GH_Point j_Pt = BranchsPts[j];
                        switch (Sign.ToLower())
                        {
                            case "x":
                                Result = Math.Abs(i_Pt.Value.X - j_Pt.Value.X);
                                break;
                            case "y":
                                Result = Math.Abs(i_Pt.Value.Y - j_Pt.Value.Y);
                                break;
                            case "z":
                                Result = Math.Abs(i_Pt.Value.Z - j_Pt.Value.Z);
                                break;
                            case "d":
                                Result = Math.Abs(i_Pt.Value.DistanceTo(j_Pt.Value));
                                break;
                            default:
                                Result = Math.Abs(i_Pt.Value.DistanceTo(j_Pt.Value));
                                break;
                        }
                        if(Result <= ThreadHold)
                        {
                            NewBranch.Add(BranchsPts[j]);
                            BranchsPts.RemoveAt(j);
                            j--;
                        }
                    }
                    GH_Path NewPath = BasePath.AppendElement(i);
                    i++;
                    OutputPts.AppendRange(NewBranch, NewPath);
                }

            }
           // DA.SetDataTree(0, OutputPts);
            int BranchCount = InputPtsTree.Branches.Count;
            DA.SetDataTree(0, OutputPts);
           // DA.SetData(1, BranchCount);
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
                Bitmap originalImg = Properties.Resources.ClusterPoints;
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
            get { return new Guid("a10f572b-183b-4a0c-96fc-2108f340e1b1"); }
        }
    }
}