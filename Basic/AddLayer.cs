using System;
using System.Linq;
using System.Collections.Generic;

using Rhino.DocObjects;
using Rhino.DocObjects.Tables;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

using CSCECDEC.Plugin.Types;
using CSCECDEC.Plugin.Params;

namespace CSCECDEC.Plugin.Basic
{
    public class AddLayer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AddLayer class.
        /// </summary>
        public AddLayer()
          : base("AddLayer", "AddLayer",
              "添加图层",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.BASICCATATORY)
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.senary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GH_Layer(), "Layer", "L", "父图层", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "N", "需要添加图层的名称", GH_ParamAccess.list);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new GH_Layer(), "Layer", "L", "An Layer in Rhino", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        /// 
        /// 以下代码还需要优化
        ///
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Layer ParentLayer = new Layer();
            List<string> LayerNames = new List<string>();
            List<Layer> OutputLayers = new List<Layer>();
            LayerTable LTable = Rhino.RhinoDoc.ActiveDoc.Layers;

            if (!DA.GetData(0, ref ParentLayer)) return;
            if (!DA.GetDataList(1, LayerNames)) return;

            if (!this.LayerExist(ParentLayer))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("{0}不存在", ParentLayer.Name));
                return;
            }

            foreach(string LayerName in LayerNames)
            {
                Layer L = new Layer();
                L.Name = LayerName;
                L.ParentLayerId = ParentLayer.Id;

                int LayerIndex = this.AddLayerToDocument(L);
                OutputLayers.Add(LTable.FindIndex(LayerIndex));
            }
            DA.SetDataList(0, OutputLayers);
        }
        private int AddLayerToDocument(Layer layer)
        {
            LayerTable Layers = Rhino.RhinoDoc.ActiveDoc.Layers;
            int LayerIndex = Layers.Add(layer);
            if (LayerIndex == -1) {

                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "父图层下已经存在同名图层");
                Layer ParentLayer = Layers.FindId(layer.ParentLayerId);
                Layer[] Arr_Layer = ParentLayer.GetChildren();
                int  TempIndex = (Arr_Layer.Where(item => item.Name == layer.Name).ToList())[0].Index;
                return TempIndex;
            }
            else
            {
                return LayerIndex;
            }
        }
        private bool LayerExist(Layer layer)
        {
            LayerTable Layers = Rhino.RhinoDoc.ActiveDoc.Layers;
            if (Layers.FindIndex(layer.Index) == null)
            {
                return false;
            }
            else
            {
                return true;
            };
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
                Bitmap originalImg = Properties.Resources.AddLayer;
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
            get { return new Guid("591516bf-cca8-4215-aad4-721b0a13ed68"); }
        }
    }
}