using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace CSCECDEC.Plugin.CutMaterial
{
    public class TableCeller : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TableCeller class.
        /// </summary>
        public TableCeller()
          : base("TableCeller", "TableCeller",
              "生成电子表格",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.CUTDOWNCATATORY)
        {
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
            pManager.AddPointParameter("Position", "Pos", "生成表格的位置", GH_ParamAccess.item,Point3d.Origin);
            pManager.AddNumberParameter("Width", "W", "单元格宽度", GH_ParamAccess.item, 200);
            pManager.AddNumberParameter("Height", "H", "单元格高度", GH_ParamAccess.item, 300);
            pManager.AddNumberParameter("Column", "C", "列数", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("Row", "R", "行数", GH_ParamAccess.item, 5);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddRectangleParameter("Table", "T", "生成的表格", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int Width = 0, Height = 0, Column = 0, Row = 0;
            Point3d Position = default(Point3d);
            int Z = 0;
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
                var X = _X + Height * i;
                GH_Path Path = new GH_Path();
                for (int j = 0; j < Column; j++)
                {
                    
                    var Y = _Y - Width * j;

                    Point3d Origin = new Point3d(X+Width,Y-Height, Z);
                    Plane _Plane = new Plane(Origin, Vector3d.ZAxis);
                    Rectangle3d Rect = new Rectangle3d(_Plane, Math.Abs(Width), Math.Abs(Height));
                    TempList.Add(new GH_Rectangle(Rect));
                    Path.AppendElement(i);
                    OutputRect.AppendRange(TempList, Path);

                }
            }
            DA.SetDataList(0, OutputRect);
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
                return null;
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