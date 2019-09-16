using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CSCECDEC.Okavango
{
    class Setting
    {
        //是否渲染Hu_Attribute
      //  public static readonly bool ISRENDERHUATTRIBUTE = Properties.Settings.Default.Is_Hu_Attribute;
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
        public static readonly string SETCATATORY = "Set";
        public static readonly string WORKFLOWCATATORY = "流程";
        //Control Color Control
        public static readonly Color BUTTONPRESSCOLOR = Color.FromArgb(255, 13, 78, 104);
        public static readonly Color BUTTONUNPRESSCOLOR = Color.FromArgb(255, 8, 47, 63);
        public static readonly Color BORDERCOLOR = Color.FromArgb(255, 6, 40, 53);
        public static readonly Color HOWERCOLOR = Color.FromArgb(255, 80, 94, 101);
        public static readonly Color TEXTCOLOR = Color.FromArgb(255, 239, 239, 239);
        public static readonly float HEIGHTFIXED = 6;
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
        public static readonly Color COMPONENTLOCKTEXTCOLOR = Color.FromArgb(255, 134, 136, 137);
        //Hu_Attribute 中的拓展宽度
        //Component中控件如按钮和Radio的外包框高度
        //Component中控件如按钮和Radio的高度
        public static readonly float EXTEND_WIDTH = 18;
        public static readonly float EXTEND_HEIGHT = 8;
        public static readonly float COMPONENTCONTROLBOXHEIGHT = 20;
        public static readonly float COMPONENTCONTROLHEIGHT = 16;
        //DROPDOWN Attribute Size
        public static readonly float DROPDOWNCOMPONENTPARAMHEIGHT = 10;
        public static readonly float DROPDOWNCOMPONENTHEIGHT = 20;
        public static readonly float DROPDOWNTRIANGLEWIDTH = 8;
        public static readonly float DROPDOWNTRIANGLEHEIGHT = 8;

        public Setting()
        {
                        
        }
    }
}
