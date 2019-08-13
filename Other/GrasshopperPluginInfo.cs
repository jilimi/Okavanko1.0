using System;
using System.Drawing;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Windows.Forms;
using Grasshopper;

using Rhino;
using Rhino.UI;
using Rhino.Geometry;

namespace CSCECDEC.Plugin
{
   
    public class GrasshopperPluginInfo : GH_AssemblyInfo
    {
        //有关版权保护的信息可以写再这里
        //C#最佳实践 使用readonly 替代const
        public static readonly string PLUGINNAME = "Okavango";
        public static readonly string BASICCATATORY = "基础"; 
        public static readonly string BIMCATATORY = "BIM"; 
        public static readonly string PREVIEWCATATORY = "预览";
        public static readonly string CUTDOWNCATATORY = "下料";
        public static readonly string DIMENSIONCATATORY = "标注";
        public static readonly string PERSONAL = "For Hudson Personal";
        //控制电池的样式
        public const float W_EXTEND = 18;
        public static readonly float GRIDEGAPE = 8;
        public static readonly float H_EXTEND = 8;

        public GrasshopperPluginInfo():base()
        {
           if(Rhino.RhinoApp.Version.Major < 6)
            {
                Dialogs.ShowMessage("OKavango 插件需运行于Rhino6及以上版本中", "提示");
                return;
            }
        }

       private void Server_GHAFileLoaded(object sender, GH_GHALoadingEventArgs e)
        {
        }

        public override string Name
        {
            get
            {
                return "Okavango";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "An Grasshopper Plugin for CHINA CONSTRUCTION SHENXHEN DECORATION CO.,LTD";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("27a01422-ca2c-4447-a88c-ef68a452f3ce");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "cughudson";
            }
        }
        public override GH_LibraryLicense AssemblyLicense
        {
            get
            {
                return base.AssemblyLicense;
            }
        }
        
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "WeChat:13312949505,It is Also my Mobile Number";
            }
        }
    }
}
