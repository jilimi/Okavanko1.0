using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;
using Rhino.DocObjects;

using CSCECDEC.Plugin.Types;
using System.Windows.Forms;
using System.Drawing;
using GH_IO.Serialization;

namespace CSCECDEC.Plugin.Params
{
    class GH_Dimention : GH_Param<Types.Hu_Dimention>
    {
        public GH_Dimention() : base(new GH_InstanceDescription("LinearDim", "LinearDim", "线性标注","Params","Okavango"))
        {

        }
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
        }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("6773B337-6F9E-44F1-9804-5163C62BE846");
            }
        }
    }
}
