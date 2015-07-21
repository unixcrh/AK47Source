using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ChinaCustoms.Framework.DeluxeWorks.Web.WebControls;
using System.Threading;

namespace DeluxeWorks.Web.WebControls.Test.DeluxeTree
{
    public partial class CompleteTreeTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                SetTree();
        }

        protected void tree_GetChildrenData(DeluxeTreeNode parentNode, DeluxeTreeNodeCollection result)
        {
            Thread.Sleep(new Random().Next(1000));

            Random rnd = new Random();

            for (int i = 0; i < rnd.Next(5); i++)
            {
                string text = "��̬�ӽڵ�" + i;
                DeluxeTreeNode node = new DeluxeTreeNode(text, text);

                if (i != 1)
                    node.ChildNodesLoadingType = ChildNodesLoadingTypeDefine.LazyLoading;

                result.Add(node);

                if (i == 1)
                {
                    DeluxeTreeNode subNode = new DeluxeTreeNode("�̶���ڵ�", text);
                    node.Nodes.Add(subNode);
                }
            }
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            if (txtExpandImage.Text != "")
                tree.ExpandImage = txtExpandImage.Text;
            if (txtCollapseImage.Text != "")
                tree.CollapseImage = txtCollapseImage.Text;
            if (txtParentNodeCloseImg.Text != "")
                tree.NodeCloseImg = txtParentNodeCloseImg.Text;
            if (txtParentNodeOpenImg.Text != "")
                tree.NodeOpenImg = txtParentNodeOpenImg.Text;
            if (txtNodeIndent.Text != "")
                tree.NodeIndent = Int32.Parse(txtNodeIndent.Text);
            
            SetTree();
        }

        private void SetTree()
        {
            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                if (txtToolTip.Text != "")
                    tree.Nodes[i].ToolTip = "���ڵ�" + i.ToString() + "��" + txtToolTip.Text;
                if (txtNodeOpenImg.Text != "")
                    tree.Nodes[i].NodeOpenImg = txtNodeOpenImg.Text;
                if (txtNodeCloseImg.Text != "")
                    tree.Nodes[i].NodeCloseImg = txtNodeCloseImg.Text;
                if (txtCssClass.Text != "")
                    tree.Nodes[i].CssClass = txtCssClass.Text;
                if (txtSelectedCssClass.Text != "")
                    tree.Nodes[i].SelectedCssClass = txtSelectedCssClass.Text;

                tree.Nodes[i].Expanded = ddlExpanded.SelectedValue == "1" ? true : false;
                tree.Nodes[i].ShowCheckBox = ddlShowCheckBox.SelectedValue == "1" ? true : false;

                tree.Nodes[i].ChildNodesLoadingType = ddlChildNodesLoadingType.SelectedValue == "1" ? ChildNodesLoadingTypeDefine.LazyLoading : ChildNodesLoadingTypeDefine.Normal;
                if (txtLazyLoadingText.Text != "")
                    tree.Nodes[i].LazyLoadingText = "���ڵ�" + i.ToString() + "��" + txtLazyLoadingText.Text;

                if (tree.Nodes[i].Value == "parent1")
                {
                    foreach (DeluxeTreeNode node in tree.Nodes[i].Nodes)
                    {
                        if (node.Value == "node2")
                        {
                            lbValue.Text = node.Value;
                            lbChecked.Text = node.Checked == true ? "��" : "��";
                            lbHasChildren.Text = node.Nodes.Count > 0 ? "��" : "��";
                            lbPreviousSibling.Text = node.PreviousSibling != null ? node.PreviousSibling.Text : "��";
                            lbNextSibling.Text = node.NextSibling != null ? node.NextSibling.Text : "��";
                            lbParent.Text = node.Parent != null ? node.Parent.Text : "��";
                        }
                    }
                }
            }
        }
    }
}
