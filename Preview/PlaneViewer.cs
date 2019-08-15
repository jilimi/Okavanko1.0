using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino;
using Grasshopper.Kernel.Attributes;
using CSCECDEC.Okavango.Attribute;
namespace CSCECDEC.Okavango.Preview
{
    public class PlaneViewer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PlaneViewer class.
        /// </summary>
        /// set and get 的用法
        private double Scale = 1;
        private List<Plane> ViewPlanes = new List<Plane>();
        public PlaneViewer()
          : base("PlaneViewer", "PlaneViewer",
              "查看工作平面的X,Y,Z坐标轴，X，Y，Z轴的颜色分别为RGB,如果输入的是树状数据结果，请先对其执行Flatten操作",
              Setting.PLUGINNAME, Setting.PREVIEWCATATORY)
        {
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
            pManager.AddPlaneParameter("Plane", "P", "工作平面", GH_ParamAccess.list);
            pManager.AddNumberParameter("Scale", "S", "放大倍数", GH_ParamAccess.item,1);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
          //  this.OnPreviewExpired(true);
        }
        /*
        protected override void AfterSolveInstance()
        {
            //这两个方法要同时使用，
            //ExpirePreview只是使预览过期失效，
            //ExpireSolution让电池进行重新计算
            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }*/
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }
         /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //这一步至关重要
            if (ViewPlanes.Count != 0) ViewPlanes.Clear();
            if (!DA.GetDataList<Plane>(0, ViewPlanes)) return;
            if (!DA.GetData(1, ref Scale)) return;
        }
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            //Scale只能在0~10之间
            this.Scale = this.Scale <= 0 ? 1 : this.Scale>10?10:this.Scale;

            Color Red = Color.Red, Green = Color.Green, Blue = Color.Blue;
            if (this.Hidden || this.Locked) return;
            if (this.Attributes.Selected)
            {

                for(int i = 0; i < ViewPlanes.Count; i++)
                {
                    Plane _Plane = ViewPlanes[i];
                    this.DrawPlaneArrow(args, _Plane, Scale,true);
                }
             }
            else
            {
                for (int i = 0; i < ViewPlanes.Count; i++)
                {
                    Plane _Plane = ViewPlanes[i];
                    this.DrawPlaneArrow(args, _Plane, Scale,false);
                }
            }
        }
        private void DrawPlaneArrow(IGH_PreviewArgs args, Plane Pl,double Scale,bool IsSelected)
        {
            Point3d Origin = Pl.Origin;
            Line XLine = new Line(Origin, Pl.XAxis, Scale * 1000);
            Line YLine = new Line(Origin, Pl.YAxis, Scale * 1000);
            Line ZLine = new Line(Origin, Pl.ZAxis, Scale * 1000);
            if (IsSelected)
            {
                args.Display.DrawArrow(XLine, Color.Green);
                args.Display.DrawArrow(YLine, Color.Green);
                args.Display.DrawArrow(ZLine, Color.Green);
            }
            else
            {
                args.Display.DrawArrow(XLine, Color.Red);
                args.Display.DrawArrow(YLine, Color.Green);
                args.Display.DrawArrow(ZLine, Color.Blue);
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
                // return Resources.IconForThisComponent;
                Bitmap newImage = new Bitmap(24, 24);
                Bitmap originalImg = Properties.Resources.PlanePreview;
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
            get { return new Guid("e7b7bb99-f4da-4ce7-98af-abec47677eb3"); }
        }
    }
}