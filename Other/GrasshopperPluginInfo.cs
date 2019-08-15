
/* Copyright(c) <year> <copyright holders>
       Permission is hereby granted, free of charge, to any person obtaining a copy
       of this software and associated documentation files(the "Software"), to deal
       in the Software without restriction, including without limitation the rights
       to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
       copies of the Software, and to permit persons to whom the Software is
       furnished to do so, subject to the following conditions:

       The above copyright notice and this permission notice shall be included in all
       copies or substantial portions of the Software.

       THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
       IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
       FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
       AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
       LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
       OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
       SOFTWARE.
 */
using System;
using System.Drawing;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Windows.Forms;
using Grasshopper;

using Rhino;
using Rhino.UI;
using Rhino.Geometry;

namespace CSCECDEC.Okavango
{
   
    //Grasshopper Setting is under here
    public class GrasshopperPluginInfo : GH_AssemblyInfo
    {
        //是否渲染Hu_Attribute
        /*
        public static readonly bool ISRENDERHUATTRIBUTE = false;
        //C#最佳实践 使用readonly 替代const
        //
        public static readonly string PLUGINNAME = "Okavango";
        public static readonly string BASICCATATORY = "基础"; 
        public static readonly string BIMCATATORY = "BIM"; 
        public static readonly string PREVIEWCATATORY = "预览";
        public static readonly string CUTDOWNCATATORY = "下料";
        public static readonly string DIMENSIONCATATORY = "标注";
        public static readonly string PERSONAL = "For Hudson Personal";
        public static readonly string PARAMS = "params";
        //Control Color Control
        public static readonly Color BUTTONPRESSCOLOR = Color.FromArgb(255, 209, 212, 214);
        public static readonly Color BUTTONUNPRESSCOLOR = Color.FromArgb(255, 157, 159, 161);
        public static readonly Color BORDERCOLOR = Color.FromArgb(255, 30, 44, 51);
        public static readonly Color HOWERCOLOR = Color.FromArgb(255, 80, 94, 101);
        public static readonly Color TEXTCOLOR = Color.FromArgb(255, 30, 44, 51);
        //Component Color Control
        //error
        //warning
        //normal
        //selected
        //Lock
        public static readonly Color COMPONENTERRORCOLOR = Color.FromArgb(255, 239, 62, 71); 
        public static readonly Color COMPONENTWARNINGCOLOR = Color.FromArgb(255, 252, 228, 76);
        public static readonly Color COMPONENTNORMALCOLOR = Color.FromArgb(255, 209, 212, 214);
        public static readonly Color COMPONENTSELECTCOLOR = Color.FromArgb(255, 46, 186, 62);
        public static readonly Color COMPONENTLOCKCOLOR = Color.FromArgb(255, 157, 159, 161);
        //Hu_Attribute 中的拓展宽度
        //Component中控件如按钮和Radio的外包框高度
        //Component中控件如按钮和Radio的高度
        public static readonly float EXTEND_WIDTH = 18;
        public static readonly float EXTEND_HEIGHT = 8;
        public static readonly float COMPONENTCONTROLBOXHEIGHT = 24;
        public static readonly float COMPONENTCONTROLHEIGHT = 18;
        //DROPDOWN Attribute Size
        public static readonly float DROPDOWNCOMPONENTHEIGHT = 20;
        public static readonly float DROPDOWNTRIANGLEWIDTH = 10;
        public static readonly float DROPDOWNTRIANGLEHEIGHT = 10;
        */
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
                return "WeChat:13312949505,It is Also my Mobile Phone Number";
            }
        }
    }
}
