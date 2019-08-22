using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Microsoft.CSharp;
using Rhino.Runtime;

using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango.CutDown
{
    public class GroupDataByKeys : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GroupData class.
        /// </summary>
        public GroupDataByKeys()
          : base("GroupGeometry", "GroupGeometry",
              "根据附加给物体的数据对几何体进行分组",
             Setting.PLUGINNAME, Setting.BIMCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
        protected override string HelpDescription
        {
            get
            {
                return "根据目标物体的'键-值'对对目标物体进行分组";
            }
        }
        public override void CreateAttributes()
        {
            //   base.CustomAttributes(this,3);
            if (Properties.Settings.Default.Is_Hu_Attribute) m_attributes = new Hu_Attribute(this);
            else m_attributes = new Grasshopper.Kernel.Attributes.GH_ComponentAttributes(this);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geom", "G", "需要分组的几何体数据", GH_ParamAccess.list);
            pManager.AddTextParameter("Key", "K", "数据分组依据,为附加数据的key值", GH_ParamAccess.item);

        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geom", "G", "分好组的几何体数据", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            //
            // Grasshopper.Kernel.GH_Convert.ToGeometricGoo，这个方法非常的重要
            // 可以实现IGH_GeometricGoo 到GeometryBase之间的转换
            //
            //
            List<IGH_GeometricGoo> GeomList = new List<IGH_GeometricGoo>();
            GH_Structure<IGH_GeometricGoo> OutPutTree = new GH_Structure<IGH_GeometricGoo>();
            string ClassfilyText = null;

            if(!DA.GetDataList<IGH_GeometricGoo>(0, GeomList))return;
            if(!DA.GetData(1, ref ClassfilyText))return;

            try
            {
                var Geom_Temp = GeomList.Select(item =>
                {
                    GeometryBase Geom = Grasshopper.Kernel.GH_Convert.ToGeometryBase(item);
                    return Geom;
                });
                var Temp = Geom_Temp.GroupBy(item =>item.UserDictionary[ClassfilyText]).ToList();

                for (int Index = 0; Index < Temp.Count; Index++)
                {
                    int[] PathArr = new int[] {Index };
                    GH_Path Path = new GH_Path(PathArr);

                    List<GeometryBase> Temp_Geom = Temp[Index].ToList();
                    List<IGH_GeometricGoo> Temp_IGeom = Temp_Geom.Select(item => { var Geom = Grasshopper.Kernel.GH_Convert.ToGeometricGoo(item);return Geom; }).ToList();

                    OutPutTree.AppendRange(Temp_IGeom,Path);
                }
                DA.SetDataTree(0, OutPutTree);
            }
            catch(Exception Ex)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, Ex.Message);
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

                System.Drawing.Bitmap newImage = new System.Drawing.Bitmap(24, 24);
                System.Drawing.Bitmap originalImg = Properties.Resources.GroupGeometry;
                //Graphic 沒有public的構造函數，不能使用new運算符，衹能通過其他方式創建graphic
                System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage((System.Drawing.Image)newImage);
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
            get { return new Guid("0d943b2e-e9e0-472e-8228-d2052e14fd03"); }
        }
    }
}