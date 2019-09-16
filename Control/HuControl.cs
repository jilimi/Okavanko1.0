using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Grasshopper.GUI;

namespace CSCECDEC.Okavango.Control
{
    public class HuControl
    {
        public RectangleF Bounds { get; set; }
        public bool IsPress;
        public bool IsClick;
        public HuControl()
        {

        }
        public virtual void On_MouseDown(object sender, GH_CanvasMouseEvent e)
        {
        }
        public virtual void On_MouseUp(object sender, GH_CanvasMouseEvent e)
        {

        }
        public virtual void On_Click(object sender, GH_CanvasMouseEvent e)
        {

        }
        public virtual RectangleF GetBounds()
        {
            return new RectangleF();
        }
        public virtual void OnDraw(Graphics g)
        {

        }
    }
}
