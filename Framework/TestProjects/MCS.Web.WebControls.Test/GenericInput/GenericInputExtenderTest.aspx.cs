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

namespace ChinaCustoms.Framework.DeluxeWorks.Web.WebControls.Test.GenericInput
{
    public partial class GenericInputExtenderTest : System.Web.UI.Page
    {
        #region ����������

        //ctrlGenericInputExtender.HighlightBorderColor;                //�ؼ��߿����ɫ                    "#2353B2"
        //ctrlGenericInputExtender.DropArrowBackgroundColor;            //������ͷ����ı���ɫ              "#C6E1FF"
        //ctrlGenericInputExtender.ItemFontColor;               Delete  //ѡ����ĿĬ��������ɫ              "#003399"
        //ctrlGenericInputExtender.ItemHoverFontColor;          Delete  //����ƶ���ѡ����Ŀ��ʱ��������ɫ  "#FFE6A0"
        //ctrlGenericInputExtender.ItemHoverBackgroundColor;    Delete  //����ƶ���ѡ����Ŀ��ʱ�ı���ɫ    "#003399"
        //ctrlGenericInputExtender.Items;                               //�ؼ��е�ѡ����Ŀ����
        //ctrlGenericInputExtender.ItemCssClass;                New
        //ctrlGenericInputExtender.ItemHoverCssClass;           New

        //ctrlGenericInputExtender.ClientID;                    Delete  //�ؼ��Ŀͻ���ID
        //ctrlGenericInputExtender.SelectedIndex;                       //��ǰѡ���Indexֵ
        //ctrlGenericInputExtender.Text;                                //�ؼ��ĵ�ǰ�ı�

        //ctrlGenericInputExtender.OnChange;
        //ctrlGenericInputExtender.OnSelectedItem;
        //ctrlGenericInputExtender.OnSelectItem;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private string BuildControlInfo()
        {
            StringBuilder strbInfo = new StringBuilder(512);

            strbInfo.Append("<cc1:GenericInputExtender ID=\"ctrlGenericInputExtender\" runat=\"server\"\n TargetControlID=\"txbTarget\"");

            if (ddlHighlightBorderColor.Text != "Default")
            {
                strbInfo.Append("\n HighlightBorderColor=\"" + GetColorString(ctrlGenericInputExtender.HighlightBorderColor.ToString()) + "\" ");
            }
            if (ddlDropArrowBackgroundColor.Text != "Default")
            {
                strbInfo.Append("\n DropArrowBackgroundColor=\"" + GetColorString(ctrlGenericInputExtender.DropArrowBackgroundColor.ToString()) + "\" ");
            }
            if (ddlItemCssClass.Text != "Default")
            {
                strbInfo.Append("\n ItemCssClass=\"" + ctrlGenericInputExtender.ItemCssClass.ToString() + "\" ");
            }
            if (ddlItemHoverCssClass.Text != "Default")
            {
                strbInfo.Append("\n ItemHoverCssClass=\"" + ctrlGenericInputExtender.ItemHoverCssClass.ToString() + "\" ");
            }
            strbInfo.Append("\n/>");
            if (txbTarget.Attributes["onSelectItem"] != null)
            {
                strbInfo.Append("\n\ntxbTarget.Attributes[\"onSelectItem\"] = " + txbTarget.Attributes["onSelectItem"].ToString());
            }
            if (txbTarget.Attributes["onSelectedItem"] != null)
            {
                strbInfo.Append("\n\ntxbTarget.Attributes[\"onSelectedItem\"] = " + txbTarget.Attributes["onSelectedItem"].ToString());
            }
            if (txbTarget.Attributes["onChange"] != null)
            {
                strbInfo.Append("\n\ntxbTarget.Attributes[\"onChange\"] = " + txbTarget.Attributes["onChange"].ToString());
            }
            return strbInfo.ToString();
        }

        private string GetColorString(string colorString)
        {
            int i = colorString.IndexOf("[");
            int j = colorString.IndexOf("]");
            return colorString.Substring(7, j - i - 1);
        }

        protected void btnSetProperties_Click(object sender, EventArgs e)
        {
            if (ddlHighlightBorderColor.Text == "Default")
            {
                ctrlGenericInputExtender.HighlightBorderColor = System.Drawing.ColorTranslator.FromHtml("#2353B2");
            }
            else
            {
                ctrlGenericInputExtender.HighlightBorderColor = System.Drawing.ColorTranslator.FromHtml(ddlHighlightBorderColor.Text);
            }
            if (ddlDropArrowBackgroundColor.Text == "Default")
            {
                ctrlGenericInputExtender.DropArrowBackgroundColor = System.Drawing.ColorTranslator.FromHtml("#C6E1FF");
            }
            else
            {
                ctrlGenericInputExtender.DropArrowBackgroundColor = System.Drawing.ColorTranslator.FromHtml(ddlDropArrowBackgroundColor.Text);
            }
            if (ddlItemCssClass.Text != "Default")
            {
                ctrlGenericInputExtender.ItemCssClass = ddlItemCssClass.Text;
            }
            if (ddlItemHoverCssClass.Text != "Default")
            {
                ctrlGenericInputExtender.ItemHoverCssClass = ddlItemHoverCssClass.Text;
            }
            ctrlGenericInputExtenderHtmlShow.Value = BuildControlInfo();

            //���Ƶ��ڶ���GenericInputExtender�ؼ�
            ctrlGenericInputExtender2.HighlightBorderColor = ctrlGenericInputExtender.HighlightBorderColor;
            ctrlGenericInputExtender2.DropArrowBackgroundColor = ctrlGenericInputExtender.DropArrowBackgroundColor;
            ctrlGenericInputExtender2.ItemCssClass = ctrlGenericInputExtender.ItemCssClass;
            ctrlGenericInputExtender2.ItemHoverCssClass = ctrlGenericInputExtender.ItemHoverCssClass;
        }

        protected void btnSetListitems_Click(object sender, EventArgs e)
        {
            ListItem litmTrans = new ListItem();
            litmTrans.Text = txbLitmText.Text;
            litmTrans.Value = txbLitmValue.Text;
            ctrlGenericInputExtender.Items.Add(litmTrans);

            //���Ƶ��ڶ���GenericInputExtender�ؼ�
            ctrlGenericInputExtender2.Items.Add(litmTrans);
        }

        protected void btnGetValue_Click(object sender, EventArgs e)
        {
            if (ctrlGenericInputExtender.SelectedIndex == -1)
            {
                txbGetValue.Text = "ctrlGenericInputExtender.Text=\"" + ctrlGenericInputExtender.Text + "\"";
            }
            else
            {
                txbGetValue.Text = "ctrlGenericInputExtender.Text=\"" + ctrlGenericInputExtender.Text + "\" , "
                                + "ctrlGenericInputExtender.SelectedIndex=\"" + ctrlGenericInputExtender.SelectedIndex.ToString() + "\"";
            }
            txbSetValue.Text = ctrlGenericInputExtender.Text;
        }

        protected void btnSetValue_Click(object sender, EventArgs e)
        {
            ctrlGenericInputExtender.Text = txbSetValue.Text;
        }

        protected void btnUseDefault_Click(object sender, EventArgs e)
        {
            ctrlGenericInputExtender.Items.Clear();
            for (int i = 0; i < 8; i++)
            {
                ListItem litmTrans = new ListItem();
                litmTrans.Text = "Text_" + i.ToString();
                litmTrans.Value = "Value_" + i.ToString();
                ctrlGenericInputExtender.Items.Add(litmTrans);

                //���Ƶ��ڶ���GenericInputExtender�ؼ�
                ctrlGenericInputExtender2.Items.Add(litmTrans);
            }
        }

        protected void btnSetOnSelectItem_Click(object sender, EventArgs e)
        {
            txbTarget.Attributes.Add("onSelectItem", txbOnSelectItem.Text);
            ctrlGenericInputExtenderHtmlShow.Value = BuildControlInfo();
        }

        protected void btnClearOnSelectItem_Click(object sender, EventArgs e)
        {
            txbTarget.Attributes.Remove("onSelectItem");
            ctrlGenericInputExtenderHtmlShow.Value = BuildControlInfo();
        }

        protected void btnSetOnSelectedItem_Click(object sender, EventArgs e)
        {
            txbTarget.Attributes.Add("onSelectedItem", txbOnSelectedItem.Text);
            ctrlGenericInputExtenderHtmlShow.Value = BuildControlInfo();
        }

        protected void btnClearOnSelectedItem_Click(object sender, EventArgs e)
        {
            txbTarget.Attributes.Remove("onSelectedItem");
            ctrlGenericInputExtenderHtmlShow.Value = BuildControlInfo();
        }

        protected void btnSetOnChange_Click(object sender, EventArgs e)
        {
            txbTarget.Attributes.Add("onChange", txbOnChange.Text);
            ctrlGenericInputExtenderHtmlShow.Value = BuildControlInfo();
        }

        protected void btnClearOnChange_Click(object sender, EventArgs e)
        {
            txbTarget.Attributes.Remove("onChange");
            ctrlGenericInputExtenderHtmlShow.Value = BuildControlInfo();
        }
    }
}
