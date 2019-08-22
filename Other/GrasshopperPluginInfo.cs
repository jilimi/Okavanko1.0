
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
using System.Linq;
using Rhino.Geometry;
using CSCECDEC.Okavango.Attribute;

namespace CSCECDEC.Okavango
{

    //Grasshopper Setting is under here
    public class GrasshopperPluginInfo : GH_AssemblyInfo
    {
        Timer _timer;
        bool Is_Hu_Attribute = false;
        public GrasshopperPluginInfo():base()
        {
           if(Rhino.RhinoApp.Version.Major < 6)
            {
                Dialogs.ShowMessage("OKavango 插件需运行于Rhino6及以上版本中", "提示");
                return;
            }
            //Server_GHAFileLoaded();
            AddToMenu();
            Rhino.RhinoApp.Closing += RhinoApp_Closing;
        }

        private void RhinoApp_Closing(object sender, EventArgs e)
        {
            Properties.Settings.Default.Is_Hu_Attribute = Is_Hu_Attribute;
            Properties.Settings.Default.Save();
        }

        private void AddToMenu()
        {
            if (_timer != null)
                return;
            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }
        private void _timer_Tick(object sender, EventArgs e)
       // private void Server_GHAFileLoaded()
        {
            var editor = Grasshopper.Instances.DocumentEditor;
            if (null == editor || editor.Handle == IntPtr.Zero)
                return;
            var controls = editor.Controls;
            if (null == controls || controls.Count == 0)
                return;
            _timer.Stop();
            foreach (var ctrl in controls)
            {
                var menu = ctrl as Grasshopper.GUI.GH_MenuStrip;
                if (menu == null)
                    continue;
                for (int i = 0; i < menu.Items.Count; i++)
                {
                    var menuitem = menu.Items[i] as ToolStripMenuItem;
                    if (menuitem != null && menuitem.Text == "Display")
                    {
                        var Hu_AttrMenuItem = new ToolStripMenuItem("Hu_Attribute",Properties.Resources.HuIcon);
                        //这时程序会管理MenuteItem的状态
                        if (Properties.Settings.Default.Is_Hu_Attribute) Hu_AttrMenuItem.Checked = true;
                        else Hu_AttrMenuItem.Checked = false;
                        Hu_AttrMenuItem.Click += Hu_AttrMenuItem_CheckHandle;
                        menuitem.DropDownItems.Insert(3, Hu_AttrMenuItem);
                        menuitem.DropDownOpened += (s, args) =>
                        {
                            if (Properties.Settings.Default.Is_Hu_Attribute)
                            {
                                Hu_AttrMenuItem.Checked = true;
                            }else
                            {
                                Hu_AttrMenuItem.Checked = false;
                            }
                        };
                        break;
                    }
                }
            }
        }
        //下面的代码还是不符合逻辑
        private void Hu_AttrMenuItem_CheckHandle(object sender, EventArgs e)
        {
            ToolStripMenuItem MenuItem = sender as ToolStripMenuItem;

            if (MenuItem.Checked)
            {
                MenuItem.Checked = false;
                Is_Hu_Attribute = false;
                //最好修改成关闭时保存
            }else
            {
                MenuItem.Checked = true;
                Is_Hu_Attribute = true;
            }
            MessageBox.Show("重启Rhino软件后将会运用设置");
        }
        private void Traverse()
        {
            //TODO
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
