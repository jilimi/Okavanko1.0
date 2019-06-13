using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using System.Windows.Forms;
using System.Drawing;

namespace CSCECDEC.Plugin.Hu
{
    public class Hu_GroupCurveByCPt : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Hu_GroupCurveByCPt class.
        /// </summary>
        public Hu_GroupCurveByCPt()
          : base("Hu_GroupCurveByCPt", "Hu_GroupCurveByCPt",
              "根据线的中点对线进行分组",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PERSONAL)
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
            pManager.AddCurveParameter("Geom", "G", "需要分组的线", GH_ParamAccess.list);
            pManager.AddNumberParameter("ThreadHold", "Th", "进行分组的容差,需大于1", GH_ParamAccess.item,0.1);
            pManager.AddTextParameter("Sign", "S", "成组的依据，只能取值'x','y','z'", GH_ParamAccess.item,"z");

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Result", "R", "分组结果", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> Crvs = new List<Curve>();
            double ThreadHold = 0.1;
            string SortSign = "x";
            GH_Structure<GH_Curve> OutputTree = new GH_Structure<GH_Curve>();

            if (!DA.GetDataList(0, Crvs)) return;
            if (!DA.GetData(1, ref ThreadHold)) return;
            if (!DA.GetData(2, ref SortSign)) return;

            int SortIndex = SortSign.ToLower() == "x" ?
                        0 : SortSign.ToLower() == "y" ?
                        1 : SortSign.ToLower() == "z" ?
                        2 : 0;


            var GroupData = Crvs.GroupBy(Crv => { double Pt_Seg = Crv.PointAtNormalizedLength(0.5)[SortIndex];return Pt_Seg * (1/ThreadHold); }).ToList();

            for (int Index = 0; Index < GroupData.Count; Index++)
            {
                var TempGroup = GroupData[Index];
                var TreeBranch = TempGroup.Select(Crv => new GH_Curve(Crv)).ToList();
                OutputTree.AppendRange(TreeBranch, new GH_Path(Index));
            }

            DA.SetDataTree(0, OutputTree);

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
                //return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.GroupCrvByCenterPoint;
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
            get { return new Guid("5facc5a3-bd4d-4d1b-bb3f-9b10275a38c6"); }
        }
    }
}