using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rhino;
using Rhino.DocObjects;
using Rhino.DocObjects.Tables;

namespace CSCECDEC.Plugin.Forms
{
    public partial class LayerDialog : Form
    {
        public int SelectLayerIndex = -1;
        public LayerDialog(/*int SelectLayerIndex*/)
        {
            InitializeComponent();
            this.ConstructLayerTreeView();
        }
        private void ConstructLayerTreeView()
        {
            LayerTable Layers = Rhino.RhinoDoc.ActiveDoc.Layers;
            TreeNode RootNode = new TreeNode("Rhino Layer");
            RootNode.Tag = -1;
            this.LayerTree.Nodes.Add(RootNode);
            for(int Index = 0; Index < Layers.Count; Index++)
            {

                if(Layers[Index].ParentLayerId == Guid.Empty)
                {
                    TreeNode Node = new TreeNode();
                    Node.Text = Layers[Index].Name;
                    Node.Tag = Layers[Index].Index;

                    RootNode.Nodes.Add(Node);
                    AddTreeNode(Node, Layers[Index]); 
                } 
            }
            /*
            if(this.SelectLayerIndex != -1)
            {
                LayerTree.SelectedNode = LayerTree.
            }*/
        }
        private void AddTreeNode(TreeNode ParentNode,Layer layer)
        {
            
            Layer[] Children = layer.GetChildren();

            if (Children == null) return;
            for(int Index = 0; Index < Children.Count(); Index++)
            {
                TreeNode Node = new TreeNode();
                Node.Text = Children[Index].Name;
                Node.Tag = Children[Index].Index;
                ParentNode.Nodes.Add(Node);
                //递归处理子图层
                AddTreeNode(Node, Children[Index]);
            }
        }
        private void OK_Btn_Click(object sender, EventArgs e)
        {
            TreeNode Node = this.LayerTree.SelectedNode;
            this.SelectLayerIndex = Convert.ToInt32(Node.Tag);
            this.FindForm().DialogResult = DialogResult.OK;

            this.FindForm().Close();
            this.FindForm().Dispose();
        }

        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            this.FindForm().DialogResult = DialogResult.Cancel;

            this.FindForm().Close();
            this.FindForm().Dispose();
        }
        private void LayerItem_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }
    }
}
