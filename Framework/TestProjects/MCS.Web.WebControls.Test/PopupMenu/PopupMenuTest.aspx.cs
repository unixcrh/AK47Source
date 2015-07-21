using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MCS.Web.WebControls.Test.PopupMenu
{
	public partial class PopupMenuTest : System.Web.UI.Page
	{
		#region ��������
		//ctrlPopupMenu.XLeft;					Delete	//(���ؼ�)�˵�������λ�� x����
		//ctrlPopupMenu.YTop;					Delete	//(���ؼ�)�˵�������λ�� y����
		//ctrlPopupMenu.Orientation;					//(���ؼ�)һ���˵����з���Ĭ��ֵΪVertical
		//ctrlPopupMenu.IsImageIndent;					//(���ؼ�)�˵���Ŀǰ��ͼ���Ƿ�������Ĭ��Ϊ��
		//ctrlPopupMenu.MultiSelect;					//(���ؼ�)�Ƿ��Ƕ�ѡ��Ĭ��Ϊfalse
		//ctrlPopupMenu.HasControlSeparator;	New		//(���ؼ�)�Ƿ���ָ��ߣ�Ĭ��Ϊfalse
		//ctrlPopupMenu.StaticDisplayLevels;			//(���ؼ�)��̬�˵�����Ĭ��Ϊ1
		//ctrlPopupMenu.SubMenuIndent;					//(���ؼ�)��̬�˵��У��Ӳ˵��������ȣ�Ĭ��ֵΪ10
		//ctrlPopupMenu.TextHeadWidth;					//(���ؼ�)�˵��ı�ǰ�Ŀո��ȣ�Ĭ��Ϊ5
		//ctrlPopupMenu.ItemFontWidth;			New		//(���ؼ�)�˵������ֿ�ȣ�Ĭ��ֵΪ150
		//ctrlPopupMenu.Target;							//(���ؼ�)[������Ŀ�꣬Ĭ��Ϊ��]������Ŀ��,��Ŀ��Ĳ�ͬ��ʽ��_blank, _parent, _search, _self, _top��
		//ctrlPopupMenu.Items;							//(���ؼ�)�˵���Ŀ����
		//ctrlPopupMenu.OnMenuItemClick;				//(���ؼ�)�����˵����¼�
		//ctrlPopupMenu.OnMenuItemShowen;		Delete	//(���ؼ�)��Ӧ������̬�˵��¼��Ŀͻ��˺�������
		//ctrlPopupMenu.OnMenuPopup;			New		//(���ؼ�)�Ӳ˵������¼�
		//ctrlPopupMenu.DefaultPopOutImageUrl;	New		//(���ؼ�)��ʾĬ����һ����̬�˵�ͼ��
		//ctrlPopupMenu.StaticPopOutImageUrl;			//(���ؼ�)������̬�˵�ͼ��
		//ctrlPopupMenu.DynamicPopOutImageUrl;			//(���ؼ�)������̬�˵�ͼ��
		//ctrlPopupMenu.CssClass;				Delete	//(���ؼ�)�˵���CssClass
		//ctrlPopupMenu.ItemCssClass;					//(���ؼ�)�˵���Ŀ��CssClass
		//ctrlPopupMenu.HoverItemCssClass;				//(���ؼ�)����Ƶ��˵���Ŀ��CssClass
		//ctrlPopupMenu.SelectedItemCssClass;			//(���ؼ�)�˵���Ŀѡ����CssClass
		//ctrlPopupMenu.SeparatorCssClass;				//(���ؼ�)�˵��ָ��ߵ�CssClass
		//ctrlPopupMenu.ImageColCssClass;				//(���ؼ�)�˵���Ŀǰ��ͼ����CssClass

		//ctrlPopupMenu.GetMenuDesignHTML(ctrlPopupMenu);

		//ctrlPopupMenu.Items[0].Enable;		New		//(�˵���)�ò˵����Ƿ���ã�Ĭ��Ϊtrue
		//ctrlPopupMenu.Items[0].IsSeparator;	New		//(�˵���)�Ƿ��Ƿָ��ߣ�Ĭ��Ϊfalse
		//ctrlPopupMenu.Items[0].Visible;		New		//(�˵���)�ò˵����Ƿ�ɼ���Ĭ��Ϊtrue
		//ctrlPopupMenu.Items[0].Text;					//(�˵���)�˵���Ŀ�ı�
		//ctrlPopupMenu.Items[0].Value;					//(�˵���)�˵���Ŀֵ
		//ctrlPopupMenu.Items[0].Target;				//(�˵���)[�˵���Ŀ����Ŀ�꣬Ĭ��Ϊ��]�˵���Ŀ����Ŀ��,��Ŀ��Ĳ�ͬ��ʽ��_blank, _parent, _search, _self, _top��
		//ctrlPopupMenu.Items[0].NavigateUrl;			//(�˵���)�˵���Ŀ����Url
		//ctrlPopupMenu.Items[0].ImageUrl;				//(�˵���)�˵���Ŀǰͼ��
		//ctrlPopupMenu.Items[0].ToolTip;				//(�˵���)�˵���Ŀ��ʾ
		//ctrlPopupMenu.Items[0].Selected;				//(�˵���)�˵���Ŀ�Ƿ�ѡ��,Ĭ��Ϊfalse
		//ctrlPopupMenu.Items[0].SeparatorMode;			//(�˵���)�˵���Ŀ�ָ�����ʽ,Ĭ��ΪPopupMenuItemSeparatorMode.None
		//ctrlPopupMenu.Items[0].Parent;				//(�˵���)�˵���ĸ���
		//ctrlPopupMenu.Items[0].ChildItems;			//(�˵���)�˵��������ļ���
		//ctrlPopupMenu.Items[0].DynamicPopOutImageUrl;	//(�˵���)��̬�����˵�ͼ��·��(��������ʹ��Ĭ��ͼ��)
		//ctrlPopupMenu.Items[0].StaticPopOutImageUrl;	//(�˵���)��̬�����˵�ͼ��·��(��������ʹ��Ĭ��ͼ��)
		//ctrlPopupMenu.Items[0].Previous;				//(�˵���)�˵����ǰһ��
		//ctrlPopupMenu.Items[0].Next;					//(�˵���)�˵���ĺ�һ��
		//ctrlPopupMenu.Items[0].NodeID;				//(�˵���)�ڵ�ID
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void btnSetProperties_Click(object sender, EventArgs e)
		{
			ctrlPopupMenu.Orientation = (MenuOrientation)Enum.Parse(typeof(MenuOrientation), ddlOrientation.Text);
			ctrlPopupMenu.IsImageIndent = ckbIsImageIndent.Checked;
			ctrlPopupMenu.MultiSelect = ckbMultiSelect.Checked;
			ctrlPopupMenu.HasControlSeparator = ckbHasControlSeparator.Checked;
			ctrlPopupMenu.StaticDisplayLevels = Convert.ToInt32(txbStaticDisplayLevels.Text);
			ctrlPopupMenu.SubMenuIndent = Convert.ToInt32(txbSubMenuIndent.Text);
			ctrlPopupMenu.TextHeadWidth = Convert.ToInt32(txbTextHeadWidth.Text);
			ctrlPopupMenu.ItemFontWidth = Convert.ToInt32(txbItemFontWidth.Text);
			ctrlPopupMenu.Target = ddlTarget.Text;
			if (ddlStaticPopOutImageUrl.Text != "Default")
			{
				ctrlPopupMenu.StaticPopOutImageUrl = ddlStaticPopOutImageUrl.Text;
			}
			if (ddlDynamicPopOutImageUrl.Text != "Default")
			{
				ctrlPopupMenu.DynamicPopOutImageUrl = ddlDynamicPopOutImageUrl.Text;
			}
			if (ddlItemCssClass.Text != "Default")
			{
				ctrlPopupMenu.ItemCssClass = ddlItemCssClass.Text;
			}
			if (ddlHoverItemCssClass.Text != "Default")
			{
				ctrlPopupMenu.HoverItemCssClass = ddlHoverItemCssClass.Text;
			}
			if (ddlSelectedItemCssClass.Text != "Default")
			{
				ctrlPopupMenu.SelectedItemCssClass = ddlSelectedItemCssClass.Text;
			}
			if (ddlImageColCssClass.Text != "Default")
			{
				ctrlPopupMenu.ImageColCssClass = ddlImageColCssClass.Text;
			}

			//���ؼ����Ը��Ƶ��ڶ����˵��ؼ�
			ctrlPopupMenu2.Orientation = ctrlPopupMenu.Orientation;
			ctrlPopupMenu2.IsImageIndent = ctrlPopupMenu.IsImageIndent;
			ctrlPopupMenu2.MultiSelect = ctrlPopupMenu.MultiSelect;
			ctrlPopupMenu2.HasControlSeparator = ctrlPopupMenu.HasControlSeparator;
			ctrlPopupMenu2.StaticDisplayLevels = ctrlPopupMenu.StaticDisplayLevels;
			ctrlPopupMenu2.SubMenuIndent = ctrlPopupMenu.SubMenuIndent;
			ctrlPopupMenu2.TextHeadWidth = ctrlPopupMenu.TextHeadWidth;
			ctrlPopupMenu2.ItemFontWidth = ctrlPopupMenu.ItemFontWidth;
			ctrlPopupMenu2.Target = ctrlPopupMenu.Target;
			ctrlPopupMenu2.StaticPopOutImageUrl = ctrlPopupMenu.StaticPopOutImageUrl;
			ctrlPopupMenu2.DynamicPopOutImageUrl = ctrlPopupMenu.DynamicPopOutImageUrl;
			ctrlPopupMenu2.ItemCssClass = ctrlPopupMenu.ItemCssClass;
			ctrlPopupMenu2.HoverItemCssClass = ctrlPopupMenu.HoverItemCssClass;
			ctrlPopupMenu2.SelectedItemCssClass = ctrlPopupMenu.SelectedItemCssClass;
			ctrlPopupMenu2.ImageColCssClass = ctrlPopupMenu.ImageColCssClass;
		}

		#region ���ýڵ㣨��ȡ����
		protected void btnSetMenuItem_Click(object sender, EventArgs e)
		{
			int[] intNodeID ={ 0, 0, 0, 0, 0 };
			string[] strTrans = txbItemNodeID.Text.Split(',');
			for (int i = 0; i < strTrans.Length; i++)
			{
				intNodeID[i] = Convert.ToInt32(strTrans[i]);
			}
			MenuItem transMenuItem = new MenuItem();
			switch (strTrans.Length)
			{
				case 0:
					break;
				case 1:
					transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1];
					break;
				case 2:
					transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1].ChildItems[intNodeID[1] - 1];
					break;
				case 3:
					transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1].ChildItems[intNodeID[1] - 1].ChildItems[intNodeID[2] - 1];
					break;
				case 4:
					transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1].ChildItems[intNodeID[1] - 1].ChildItems[intNodeID[2] - 1].ChildItems[intNodeID[3] - 1];
					break;
				case 5:
					transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1].ChildItems[intNodeID[1] - 1].ChildItems[intNodeID[2] - 1].ChildItems[intNodeID[3] - 1].ChildItems[intNodeID[4] - 1];
					break;
			}
			transMenuItem.Enable = ckbItemEnable.Checked;
			transMenuItem.IsSeparator = ckbItemIsSeparator.Checked;
			transMenuItem.Visible = ckbItemVisible.Checked;
			transMenuItem.Selected = ckbItemSelected.Checked;
			transMenuItem.Text = txbItemText.Text;
			transMenuItem.Value = txbItemValue.Text;
			transMenuItem.Target = txbItemTarget.Text;
			transMenuItem.NavigateUrl = txbItemNavigateUrl.Text;
			transMenuItem.ImageUrl = txbItemImageUrl.Text;
			transMenuItem.ToolTip = txbItemToolTip.Text;
			transMenuItem.DynamicPopOutImageUrl = txbItemDynamicPopOutImageUrl.Text;
			transMenuItem.StaticPopOutImageUrl = txbItemStaticPopOutImageUrl.Text;
			txbItemNodeID.Enabled = true;
			btnGetMenuItem.Enabled = true;
			btnSetMenuItem.Enabled = false;

			txbItemNodeID.Text = string.Empty;
			txbItemText.Text = string.Empty;
			txbItemValue.Text = string.Empty;
			txbItemTarget.Text = string.Empty;
			txbItemNavigateUrl.Text = string.Empty;
			txbItemImageUrl.Text = string.Empty;
			ckbItemSelected.Checked = false;
			txbItemStaticPopOutImageUrl.Text = string.Empty;
			txbItemDynamicPopOutImageUrl.Text = string.Empty;
			txbItemToolTip.Text = string.Empty;
			txbItemPrevious.Text = string.Empty;
			txbItemNext.Text = string.Empty;
			txbItemParent.Text = string.Empty;
			textAreaChildren.Value = string.Empty;
		}
		#endregion

		protected void btnGetMenuItem_Click(object sender, EventArgs e)
		{
			int[] intNodeID ={ 0, 0, 0, 0, 0 };
			string[] strTrans = txbItemNodeID.Text.Split(',');
			for (int i = 0; i < strTrans.Length; i++)
			{
				intNodeID[i] = Convert.ToInt32(strTrans[i]);
			}
			MenuItem transMenuItem = new MenuItem();
			try
			{
				switch (strTrans.Length)
				{
					case 0:
						break;
					case 1:
						transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1];
						break;
					case 2:
						transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1].ChildItems[intNodeID[1] - 1];
						break;
					case 3:
						transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1].ChildItems[intNodeID[1] - 1].ChildItems[intNodeID[2] - 1];
						break;
					case 4:
						transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1].ChildItems[intNodeID[1] - 1].ChildItems[intNodeID[2] - 1].ChildItems[intNodeID[3] - 1];
						break;
					case 5:
						transMenuItem = ctrlPopupMenu.Items[intNodeID[0] - 1].ChildItems[intNodeID[1] - 1].ChildItems[intNodeID[2] - 1].ChildItems[intNodeID[3] - 1].ChildItems[intNodeID[4] - 1];
						break;
				}
				ckbItemEnable.Checked = transMenuItem.Enable;
				ckbItemIsSeparator.Checked = transMenuItem.IsSeparator;
				ckbItemVisible.Checked = transMenuItem.Visible;
				ckbItemSelected.Checked = transMenuItem.Selected;
				txbItemText.Text = transMenuItem.Text;
				txbItemValue.Text = transMenuItem.Value;
				txbItemTarget.Text = transMenuItem.Target;
				txbItemNavigateUrl.Text = transMenuItem.NavigateUrl;
				txbItemImageUrl.Text = transMenuItem.ImageUrl;
				txbItemStaticPopOutImageUrl.Text = transMenuItem.StaticPopOutImageUrl;
				txbItemDynamicPopOutImageUrl.Text = transMenuItem.DynamicPopOutImageUrl;
				txbItemToolTip.Text = transMenuItem.ToolTip;
				if (transMenuItem.Previous == null)
				{
					txbItemPrevious.Text = "No Previous MenuItem";
				}
				else
				{
					txbItemPrevious.Text = transMenuItem.Previous.Text;
				}
				if (transMenuItem.Next == null)
				{
					txbItemNext.Text = "No Next MenuItem";
				}
				else
				{
					txbItemNext.Text = transMenuItem.Next.Text;
				}
				if (transMenuItem.Parent == null)
				{
					txbItemParent.Text = "No Parent MenuItem";
				}
				else
				{
					txbItemParent.Text = transMenuItem.Parent.Text;
				}
				if (transMenuItem.ChildItems.Count > 0)
				{
					StringBuilder stringbTrans = new StringBuilder(512);
					stringbTrans.Append(transMenuItem.ChildItems[0].Text);
					if (transMenuItem.ChildItems.Count > 1)
					{
						for (int i = 1; i < transMenuItem.ChildItems.Count; i++)
						{
							stringbTrans.Append("\n" + transMenuItem.ChildItems[i].Text);
						}
					}
					textAreaChildren.Value = stringbTrans.ToString();
				}
				else
				{
					textAreaChildren.Value = "No Children MenuItems";
				}
				txbItemNodeID.Enabled = false;
				btnGetMenuItem.Enabled = false;
				btnSetMenuItem.Enabled = true;
				LiteralJS.Text = string.Empty;
			}
			catch
			{
				txbItemNodeID.Text = string.Empty;
				LiteralJS.Text = "<script>alertJS();</script>";
				txbItemText.Text = string.Empty;
				txbItemValue.Text = string.Empty;
				txbItemTarget.Text = string.Empty;
				txbItemNavigateUrl.Text = string.Empty;
				txbItemImageUrl.Text = string.Empty;
				ckbItemSelected.Checked = false;
				txbItemStaticPopOutImageUrl.Text = string.Empty;
				txbItemDynamicPopOutImageUrl.Text = string.Empty;
				txbItemToolTip.Text = string.Empty;
				txbItemPrevious.Text = string.Empty;
				txbItemNext.Text = string.Empty;
				txbItemParent.Text = string.Empty;
				textAreaChildren.Value = string.Empty;
			}
		}
	}
}
