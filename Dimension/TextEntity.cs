using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.DocObjects;

using CSCECDEC.Okavango.Attribute;
using CSCECDEC.Okavango.Types;
using CSCECDEC.Okavango.Params;

using Rhino;

namespace CSCECDEC.Okavango.Dimension
{
    public class TextEntityObj : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TextEntity class.
        /// </summary>
        /// 
        private TextEntity TextObj;
        TextVerticalAlignment VerticalAlign;
        TextHorizontalAlignment HorizontalAlign;
        public TextEntityObj()
          : base("TextEntity", "TextEntity",
              "创建一个TextEntity",
              Setting.PLUGINNAME, Setting.DIMENSIONCATATORY)
        {
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
            pManager.AddTextParameter("Text", "T", "字符串", GH_ParamAccess.item);
            pManager.AddParameter(new GH_Font());
            pManager.AddPlaneParameter("Plane", "P", "TextEntity的位置", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TextEntity", "T", "TextEntity", GH_ParamAccess.list);
        }
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            this.AppendAddidentMenuItem(menu);
        }
        private void AppendAddidentMenuItem(ToolStripDropDown menu)
        {
            /*
            Menu_AppendItem(menu, "A0图框", Do_Add_PaperFrame, true, this.DrawingFrameType == 0 ? true : false);
            Menu_AppendItem(menu, "A1图框", Do_Add_PaperFrame, true, this.DrawingFrameType == 1 ? true : false);
            Menu_AppendItem(menu, "A2图框", Do_Add_PaperFrame, true, this.DrawingFrameType == 2 ? true : false);
            Menu_AppendItem(menu, "A3图框", Do_Add_PaperFrame, true, this.DrawingFrameType == 3 ? true : false);
            Menu_AppendItem(menu, "A4图框", Do_Add_PaperFrame, true, this.DrawingFrameType == 4 ? true : false);
            Menu_AppendItem(menu, "矩形图框", Do_Add_PaperFrame, true, this.DrawingFrameType == 5 ? true : false);
            */
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string Text = "";
            Hu_Font TextFont = new Hu_Font();
            Plane TextLocation = default(Plane);

            if (!DA.GetData(0, ref Text)) return;
            if (!DA.GetData(1, ref TextFont)) return;
            if (!DA.GetData(2, ref TextLocation)) return;

            TextObj = new TextEntity();

            TextObj.RichText = Text;
            TextObj.Font = new Rhino.DocObjects.Font(TextFont.FontFace);
            TextObj.Plane = TextLocation;
            TextObj.TextHeight = TextFont.FontHeight;
            TextObj.SetItalic(TextFont.IsItalic);
            TextObj.SetBold(TextFont.IsBold);
            TextObj.TextHorizontalAlignment = TextHorizontalAlignment.Center;

            DA.SetData(0, TextObj);

        }
        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            System.Drawing.Color TextColor = System.Drawing.Color.Red;
            if (this.Attributes.Selected) TextColor = System.Drawing.Color.Green;
            //Rhino5里面不支持Draw3dText 的TextAlign
            Transform Tran = this.CalculateTextPlane();
            Plane Pl = this.TextObj.Plane;
            Pl.Transform(Tran);
            args.Display.Draw3dText(TextObj.RichText, TextColor, Pl, TextObj.TextHeight, TextObj.Font.FaceName, TextObj.Font.Bold, TextObj.Font.Italic);
        }
        private Transform CalculateTextPlane()
        {
            string Text = this.TextObj.Text;
            System.Drawing.Font _Font = new System.Drawing.Font(this.TextObj.Font.FaceName, Convert.ToSingle(this.TextObj.TextHeight));

            int Width = GH_FontServer.StringWidth(Text, _Font);
            int Height = _Font.Height;
            Plane originPlane = this.TextObj.Plane;
            Transform H_Tran = new Transform(),V_Tran = new Transform();
            Vector3d H_Vector, V_Vector;

            switch (this.TextObj.TextHorizontalAlignment)
            {
                case TextHorizontalAlignment.Center:
                    H_Vector = originPlane.XAxis * Width * (-0.5);
                    break;
                case TextHorizontalAlignment.Left:
                    H_Vector = originPlane.XAxis;
                    break;
                case TextHorizontalAlignment.Right:
                    H_Vector = originPlane.XAxis * Width * (-1);
                    break;
                default:
                    H_Vector = originPlane.XAxis;
                    break;
            }
            switch (this.TextObj.TextVerticalAlignment)
            {
                case TextVerticalAlignment.Bottom:
                    V_Vector = originPlane.YAxis;
                    break;
                case TextVerticalAlignment.Middle:
                    V_Vector = originPlane.YAxis * Height * 0.5;
                    break;
                case TextVerticalAlignment.Top:
                    V_Vector = originPlane.YAxis * Height * 1;
                    break;
                default:
                    V_Vector = originPlane.YAxis;
                    break;
            }
            H_Tran = Transform.Translation(H_Vector);
            V_Tran = Transform.Translation(V_Vector);

            return Transform.Multiply(H_Tran, V_Tran);
        } 
        public override void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            doc.Objects.AddText(this.TextObj);
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
            get { return new Guid("3b5f43be-506a-44ad-968e-77e81ea77d24"); }
        }
    }
}