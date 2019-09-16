using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;

using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.GUI;

namespace CSCECDEC.Okavango.Control
{
    class DropDownControl
    {
        public IGH_Component Owner;
        public Action<ToolStripMenuItem> ItemClick;
        private List<string> DropDownTexts = new List<string>();
        public ContextMenuStrip DropDownMenu;

        public DropDownControl(Action<ToolStripMenuItem> _ItemClick, IGH_Component Owner) : base()
        {
            this.ItemClick = _ItemClick;
            this.Owner = Owner;
        }
        public void ConstructContextMenu(List<string> MenuNames)
        {

            this.DropDownMenu = new ContextMenuStrip
            {
                Width = 245,
                ShowCheckMargin = false,
            };
            for (int Index = 0; Index < MenuNames.Count; Index++)
            {
                ToolStripMenuItem Item = new ToolStripMenuItem(MenuNames[Index], null, On_ItemClick);
                Item.CheckOnClick = true;
                DropDownMenu.Items.Add(Item);
            }
        }
        private void On_ItemClick(object sender,EventArgs e)
        {
            ToolStripMenuItem Menuitem = sender as ToolStripMenuItem;

            if (Menuitem == null) return;
            this.ItemClick(Menuitem);
        }
        public void On_MouseDown(object sender, GH_CanvasMouseEvent e)
        {
            this.ShowDropMenu(sender, e);
        }
        private void ShowDropMenu(object sender, GH_CanvasMouseEvent e)
        {
            GH_Canvas Canvas = sender as GH_Canvas;
            this.DropDownMenu.Show(Canvas, GH_Convert.ToPoint(e.ControlLocation));
        }
        public RectangleF GetBounds()
        {
            RectangleF OutputBounds = this.Owner.Params.Output[0].Attributes.Bounds;
            float Rect_X = OutputBounds.Location.X - Setting.DROPDOWNTRIANGLEHEIGHT;
            float Rect_Y = OutputBounds.Location.Y + Setting.DROPDOWNTRIANGLEHEIGHT / 2;
            return new RectangleF(Rect_X, Rect_Y, Setting.DROPDOWNTRIANGLEHEIGHT, Setting.DROPDOWNTRIANGLEHEIGHT);
        }
        public void OnDraw(Graphics g)
        {

            Pen Pen = new Pen(Setting.BORDERCOLOR);
            RectangleF OutputBounds = this.Owner.Params.Output[0].Attributes.Bounds;
        
            PointF Pt1 = new PointF(OutputBounds.Location.X, OutputBounds.Location.Y + (Setting.DROPDOWNCOMPONENTHEIGHT-Setting.DROPDOWNTRIANGLEHEIGHT)/2);
            PointF Pt2 = new PointF(Pt1.X - Setting.DROPDOWNTRIANGLEHEIGHT, Pt1.Y);
            PointF Pt3 = new PointF(Pt1.X - Setting.DROPDOWNTRIANGLEHEIGHT/2, Pt1.Y + Setting.DROPDOWNTRIANGLEHEIGHT);

            PointF[] Pts = new PointF[] { Pt1, Pt2, Pt3, Pt1 };
            g.DrawPolygon(new Pen(Color.Black, 2), Pts);
            g.FillPolygon(Pen.Brush, Pts, FillMode.Alternate);
        }
        public void DrawLine(Graphics g)
        {
            RectangleF OutputBounds = this.Owner.Params.Output[0].Attributes.Bounds;
            PointF Pt1 = new PointF(OutputBounds.Location.X, OutputBounds.Location.Y + (Setting.DROPDOWNCOMPONENTHEIGHT - Setting.DROPDOWNTRIANGLEHEIGHT) / 2);

            PointF Line_Pt1 = new PointF(Pt1.X - Setting.DROPDOWNTRIANGLEHEIGHT - 7, OutputBounds.Location.Y + 1);
            PointF Line_Pt2 = new PointF(Pt1.X - Setting.DROPDOWNTRIANGLEHEIGHT - 7, OutputBounds.Bottom - 1);

            g.DrawLine(new Pen(Setting.BORDERCOLOR), Line_Pt1, Line_Pt2);
        }
    }
}
