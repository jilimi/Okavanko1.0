using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Windows.Forms;
using System.Drawing;

namespace CSCECDEC.Plugin.CutMaterial
{
    public class TableCeller : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TableCeller class.
        /// </summary>
        private int Direction = 1;

        private ToolStripMenuItem R2LMenuItem;
        private ToolStripMenuItem L2RMenuItem;
        bool R2LMenuItemCheeck = true;
        bool L2RMenuItemCheck = false;

        public TableCeller()
          : base("TableCeller", "TableCeller",
              "生成电子表格",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.CUTDOWNCATATORY)
        {
            this.Message = "Align:Left to Right";
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddPointParameter("Position", "Pos", "生成表格的位置,以表格左上角点的坐标表示位置", GH_ParamAccess.item,Point3d.Origin);
            pManager.AddNumberParameter("Width", "W", "单元格宽度", GH_ParamAccess.item, 300);
            pManager.AddNumberParameter("Height", "H", "单元格高度", GH_ParamAccess.item, 200);
            pManager.AddIntegerParameter("Column", "C", "列数", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Row", "R", "行数", GH_ParamAccess.item, 5);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddRectangleParameter("Table", "T", "生成的表格", GH_ParamAccess.tree);
        }
        //每次单击右键的时候就会调用这个函数
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
           R2LMenuItem =  Menu_AppendItem(menu, "Align：Right to Left", Right_to_Left, true, R2LMenuItemCheeck);
           L2RMenuItem = Menu_AppendItem(menu, "Align：Left to Right", Left_to_Right, true, L2RMenuItemCheck);
         }
        private void Right_to_Left(object sender, EventArgs e)
        {

            if (R2LMenuItem.Checked) return;

            R2LMenuItemCheeck = true;
            L2RMenuItemCheck = false;

            R2LMenuItem.Checked = R2LMenuItemCheeck;
            L2RMenuItem.Checked = L2RMenuItemCheck;

            this.Message = "Align:Right to Left";
            this.Direction = 1;
            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }
        private void Left_to_Right(object sender, EventArgs e)
        {

            if (L2RMenuItem.Checked) return;

            R2LMenuItemCheeck = false;
            L2RMenuItemCheck = true;

            L2RMenuItem.Checked = R2LMenuItemCheeck;
            R2LMenuItem.Checked = L2RMenuItemCheck;

            this.Message = "Align:Left to Right";
            this.Direction = -1;
            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double Width = 0, Height = 0;
            int Column = 0, Row = 0;
            Point3d Position = default(Point3d);
            GH_Structure<GH_Rectangle> OutputRect = new GH_Structure<GH_Rectangle>();

            if (!DA.GetData(0, ref Position)) return;
            if (!DA.GetData(1, ref Width)) return;
            if (!DA.GetData(2, ref Height)) return;
            if (!DA.GetData(3, ref Column)) return;
            if (!DA.GetData(4, ref Row)) return;

            double _X = Position.X, _Y = Position.Y; 

            for(int i = 0; i < Row; i++)
            {
                List<GH_Rectangle> TempList = new List<GH_Rectangle>();
                var Y = _Y - Height * i;
                GH_Path Path = new GH_Path(i);
                for (int j = 0; j < Column; j++)
                {
                    Point3d Origin;
                    if (Direction == 1)
                    {
                        var X = _X + Width * j*this.Direction;
                        Origin = new Point3d(X, Y-Height, 0);
                    }else
                    {
                        var X = _X + Width * j * this.Direction-Width;
                        Origin = new Point3d(X, Y - Height, 0);
                    }
                    Plane _Plane = new Plane(Origin, Vector3d.ZAxis);
                    Rectangle3d Rect = new Rectangle3d(_Plane, Math.Abs(Width), Math.Abs(Height));
                    TempList.Add(new GH_Rectangle(Rect));
                }
                OutputRect.AppendRange(TempList, Path);
            }
            DA.SetDataTree(0, OutputRect);
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
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.TableCeller;
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
            get { return new Guid("838e43c1-b13c-4cef-be3c-5427f06f2909"); }
        }
    }
}