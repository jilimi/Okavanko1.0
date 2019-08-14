using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;
using Rhino.DocObjects;

using CSCECDEC.Okavango.Types;
using System.Windows.Forms;
using System.Drawing;
using GH_IO.Serialization;

namespace CSCECDEC.Okavango.Params
{
    class GH_ObjectAttribute : GH_Param<Types.Hu_ObjectAttribute>
    {
        public GH_ObjectAttribute():base(new GH_InstanceDescription("ObjectAttribute","Attribute","物体属性数据，如图层、颜色等","Params"))
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.hidden;
            }
        }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("062F24C8 - 7CF1 - 4E31 - 8E10 - 8679889EBD72");
            }
        }
    }
}