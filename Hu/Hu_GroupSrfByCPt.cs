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
    public class Hu_GroupSrfByCPt : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Hu_GroupCurveByCPt class.
        /// </summary>
        public Hu_GroupSrfByCPt()
          : base("Hu_GroupSrfByCPt", "Hu_GroupSrfByCPt",
              "根据面的中点对面进行分组",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PERSONAL)
        {
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "S", "需要分组的面", GH_ParamAccess.list);
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
            List<Surface> Srfs = new List<Surface>();
            double ThreadHold = 0.1;
            string SortSign = "x";
            GH_Structure<GH_Surface> OutputTree = new GH_Structure<GH_Surface>();

            if (!DA.GetDataList(0, Srfs)) return;
            if (!DA.GetData(1, ref ThreadHold)) return;
            if (!DA.GetData(2, ref SortSign)) return;

            int SortIndex = SortSign.ToLower() == "x" ?
                        0 : SortSign.ToLower() == "y" ?
                        1 : SortSign.ToLower() == "z" ?
                        2 : 0;


            var GroupData = Srfs.GroupBy(Srf=> { double Pt_Seg = AreaMassProperties.Compute(Srf).Centroid[SortIndex];return Pt_Seg * (1/ThreadHold); }).ToList();

            for (int Index = 0; Index < GroupData.Count; Index++)
            {
                var TempGroup = GroupData[Index];
                var TreeBranch = TempGroup.Select(Srf => new GH_Surface(Srf)).ToList();
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
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.GroupSrfByCenterPoint;
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
            get { return new Guid("67BD8419-37B2-41BF-9627-1A4047ECD699"); }
        }
    }
}