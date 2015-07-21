using System;
using System.Data;
using System.Threading;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MCS.Web.WebControls.Test.DeluxeTree
{
	public partial class OneTreeTest : System.Web.UI.Page
    {
		private class ExtendedData
		{
			public string Data = "Hello world";
		}

        protected void Page_Load(object sender, EventArgs e)
        {

        }

		protected void tree_GetChildrenData(DeluxeTreeNode parentNode, DeluxeTreeNodeCollection result, string callBackContext)
        {
			if (parentNode.Text == "�����ӽڵ������쳣")
				throw new System.ApplicationException("�����ӽڵ�����쳣");
			else
				if (parentNode.Text == "�ܶ��ӽڵ㣬С�Ĵ򿪣�")
					CreateSubTreeNodes(result, 250);
				else
				{
					Random rnd = new Random();
					CreateSubTreeNodes(result, 1);
				}
        }

		private void CreateSubTreeNodes(DeluxeTreeNodeCollection parent, int subNodesCount)
		{
			for (int i = 0; i < subNodesCount; i++)
			{
				string text = "��̬�ӽڵ�" + i;
				DeluxeTreeNode node = new DeluxeTreeNode(text, text);

				node.ExtendedData = new ExtendedData();
				node.ShowCheckBox = true;

				if (i != 1)
					node.ChildNodesLoadingType = ChildNodesLoadingTypeDefine.LazyLoading;

				parent.Add(node);

				if (i == 1)
				{
					DeluxeTreeNode subNode = new DeluxeTreeNode("�̶���ڵ�", text);
					node.Nodes.Add(subNode);
				}
			}
		}
    }
}
