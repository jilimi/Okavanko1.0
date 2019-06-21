﻿using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino;

namespace CSCECDEC.Plugin.Preview
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
              "查看工作平面的X,Y,Z坐标轴",
              GrasshopperPluginInfo.PLUGINNAME, GrasshopperPluginInfo.PREVIEWCATATORY)
        {
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

            this.ExpirePreview(true);
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
                    Point3d Origin = _Plane.Origin;

                    Line XLine = new Line(Origin, _Plane.XAxis, Scale * 100);
                    Line YLine = new Line(Origin, _Plane.YAxis, Scale * 100);
                    Line ZLine = new Line(Origin, _Plane.ZAxis, Scale * 100);

                    args.Display.DrawArrow(XLine, Color.Green);
                    args.Display.DrawArrow(YLine, Color.Green);
                    args.Display.DrawArrow(ZLine, Color.Green);
                }
             }
            else
            {
                for (int i = 0; i < ViewPlanes.Count; i++)
                {
                    Plane _Plane = ViewPlanes[i];
                    Point3d Origin = _Plane.Origin;

                    Line XLine = new Line(Origin, _Plane.XAxis, Scale * 100);
                    Line YLine = new Line(Origin, _Plane.YAxis, Scale * 100);
                    Line ZLine = new Line(Origin, _Plane.ZAxis, Scale * 100);

                    args.Display.DrawArrow(XLine, Color.Red);
                    args.Display.DrawArrow(YLine, Color.Green);
                    args.Display.DrawArrow(ZLine, Color.Blue);
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
                return null;
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