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
using MCS.Web.Library.Script;

namespace MCS.Web.WebControls.Test.AutoComplete
{
    public partial class AutoCompleteTest : System.Web.UI.Page
    {
        #region ����������
        //ctrlAutoCompleteExtender.AutoCallBack;                New     //�Ƿ��Զ��ص�

        //ctrlAutoCompleteExtender.IsAutoComplete;                      //�Ƿ���Զ���ɹ���
        //ctrlAutoCompleteExtender.RequireValidation;                   //�ؼ��Ƿ�������֤��Ĭ��false����������
        //ctrlAutoCompleteExtender.EnableCaching;                       //���ÿͻ��˻��棬Ĭ��true��������

        //ctrlAutoCompleteExtender.MinimumPrefixLength;                 //������ٸ��ַ���ʼ�Զ���ɣ�Ĭ��Ϊ3
        //ctrlAutoCompleteExtender.CompletionInterval;                  //�Զ���ɼ������λ���룬Ĭ��1000����
        //ctrlAutoCompleteExtender.MaxCompletionRecordCount;            //�ؼ��Զ���ɳ������б�����ʾ������¼������Ĭ��-1����ʾ��ʾȫ������
        //ctrlAutoCompleteExtender.MaxPopupWindowHeight;                //�ؼ��Զ���ɵ�����ѡ�񴰿ڵ����߶ȣ�Ĭ��Ϊ260px

        //ctrlAutoCompleteExtender.CompletionBodyBorderColor;   Delete
        //ctrlAutoCompleteExtender.CompletionBodyCssClass;      New
        //ctrlAutoCompleteExtender.ItemFontColor;               Delete
        //ctrlAutoCompleteExtender.ItemCssClass;                        //�Զ���ɵ���Ŀ��CssClass��Ĭ��Ϊ��
        //ctrlAutoCompleteExtender.ItemHoverFontColor;          Delete
        //ctrlAutoCompleteExtender.ItemHoverBackgroundColor;    Delete
        //ctrlAutoCompleteExtender.ItemHoverCssClass;                   //����ƶ����Զ���ɵ���Ŀ�ϵ�CssClass��Ĭ��Ϊ��
        //ctrlAutoCompleteExtender.ErrorCssClass;                       //������������֤����������������޷���ȫƥ�䵽����Դ�е�ĳһ����CssClass��Ĭ��Ϊ��

        //ctrlAutoCompleteExtender.DataTextFieldList;                   //�ṩ�ı���ʾ������Դ���Լ���(�ر�����)
        //ctrlAutoCompleteExtender.DataTextFormatString;                //ָ����ʾ��ʽ���ַ���
        //ctrlAutoCompleteExtender.DataValueField;                      //ָ����Ŀ��Valueֵ���ֶ�����Ψһ��ʶ��Ŀ
        //ctrlAutoCompleteExtender.CompareFieldName;                    //������֤���ֶμ���(�ر�����)��ֻҪ��һ���ֶη�����������Ϊ��ƥ��ɹ�
        //ctrlAutoCompleteExtender.InvokeMethod;                Delete  //�ص�ҳ��ķ�������

        //ctrlAutoCompleteExtender.DataList;                            //��������Դ
        //ctrlAutoCompleteExtender.Text;                                //��ȡ����������λ�õ�ֵ
        //ctrlAutoCompleteExtender.SelectValue;                         //�����ѡ����򱣴�ѡ���ֵ

        //ctrlAutoCompleteExtender.CallBackPageMethod();
        #endregion
		protected override void OnInit(EventArgs e)
		{
			StaticCallBackProxy.Instance.TargetControlLoaded += new StaticCallBackProxyControlLoadedEventHandler(Instance_TargetControlLoaded);
			base.OnInit(e);
		}

		void Instance_TargetControlLoaded(Control targetControl)
		{
			//throw new NotImplementedException();
			string a = string.Empty;
		}
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtTrans = new DataTable();
            dtTrans.Columns.Add("ID", typeof(string));
            dtTrans.Columns.Add("Text", typeof(string));
            dtTrans.Columns.Add("Value", typeof(string));
            DataRow datarowTrans;
            for (int i = 0; i < 20; i++)
            {
                datarowTrans = dtTrans.NewRow();
                datarowTrans["ID"] = i.ToString();
                datarowTrans["Text"] = "Text" + i.ToString();
                datarowTrans["Value"] = "Value" + i.ToString();
                dtTrans.Rows.Add(datarowTrans);
            }
            ctrlAutoCompleteExtender.DataSource = dtTrans;
        }

        #region �ص�ҳ��ķ�������(��ɾ��)
        //public System.Collections.IEnumerable InvokeMethodA(string currentPrefix, int maxCompletionRecordCount)
        //{
        //    DataTable dtTrans = new DataTable();
        //    dtTrans.Columns.Add("id", typeof(string));
        //    dtTrans.Columns.Add("Text", typeof(string));
        //    dtTrans.Columns.Add("Value", typeof(string));

        //    DataRow datarowTrans;
        //    if (maxCompletionRecordCount == -1)
        //    {
        //        maxCompletionRecordCount = 27;
        //    }
        //    for (int i = 0; i < maxCompletionRecordCount; i++)
        //    {
        //        datarowTrans = dtTrans.NewRow();
        //        datarowTrans["id"] = i.ToString();
        //        datarowTrans["Text"] = currentPrefix + i.ToString();
        //        datarowTrans["Value"] = "ValueA_" + i.ToString();
        //        dtTrans.Rows.Add(datarowTrans);
        //    }

        //    ctrlAutoCompleteExtender.DataSource = dtTrans;
        //    return ctrlAutoCompleteExtender.DataList;
        //}

        //public System.Collections.IEnumerable InvokeMethodB(string currentPrefix, int maxCompletionRecordCount)
        //{
        //    DataTable dtTrans = new DataTable();
        //    dtTrans.Columns.Add("id", typeof(string));
        //    dtTrans.Columns.Add("Text", typeof(string));
        //    dtTrans.Columns.Add("Value", typeof(string));

        //    DataRow datarowTrans;
        //    if (maxCompletionRecordCount == -1)
        //    {
        //        maxCompletionRecordCount = 27;
        //    }
        //    for (int i = 0; i < maxCompletionRecordCount; i++)
        //    {
        //        datarowTrans = dtTrans.NewRow();
        //        datarowTrans["id"] = i.ToString();
        //        datarowTrans["Text"] = currentPrefix + Convert.ToChar(i + 97);
        //        datarowTrans["Value"] = "ValueB_" + i.ToString();
        //        dtTrans.Rows.Add(datarowTrans);
        //    }

        //    ctrlAutoCompleteExtender.DataSource = dtTrans;
        //    return ctrlAutoCompleteExtender.DataList;
        //}
        #endregion        

        private string GetColorString(string colorString)
        {
            int i = colorString.IndexOf("[");
            int j = colorString.IndexOf("]");
            return colorString.Substring(7, j - i - 1);
        }

        private string BuildControlInfo()
        {
            StringBuilder strbInfo = new StringBuilder(512);

            strbInfo.Append("<cc1:AutoCompleteExtender ID=\"ctrlAutoCompleteExtender\" runat=\"server\"\n TargetControlID=\"txbTarget\" ");
            strbInfo.Append("\n AutoCallBack=\"" + ctrlAutoCompleteExtender.AutoCallBack.ToString() + "\" ");
            strbInfo.Append("\n IsAutoComplete=\"" + ctrlAutoCompleteExtender.IsAutoComplete.ToString() + "\" ");
            strbInfo.Append("\n RequireValidation=\"" + ctrlAutoCompleteExtender.RequireValidation.ToString() + "\" ");
            strbInfo.Append("\n EnableCaching=\"" + ctrlAutoCompleteExtender.EnableCaching.ToString() + "\" ");
            strbInfo.Append("\n MinimumPrefixLength=\"" + ctrlAutoCompleteExtender.MinimumPrefixLength.ToString() + "\" ");
            strbInfo.Append("\n CompletionInterval=\"" + ctrlAutoCompleteExtender.CompletionInterval.ToString() + "\" ");
            strbInfo.Append("\n MaxCompletionRecordCount=\"" + ctrlAutoCompleteExtender.MaxCompletionRecordCount.ToString() + "\" ");
            strbInfo.Append("\n MaxPopupWindowHeight=\"" + ctrlAutoCompleteExtender.MaxPopupWindowHeight.ToString() + "\" ");
            if (ddlCompletionBodyCssClass.Text != "Default")
            {
                //strbInfo.Append("\n CompletionBodyCssClass=\"" + ctrlAutoCompleteExtender.CompletionBodyCssClass.ToString() + "\" ");
            }
            if (ddlItemCssClass.Text != "Default")
            {
                strbInfo.Append("\n ItemCssClass=\"" + ctrlAutoCompleteExtender.ItemCssClass.ToString() + "\" ");
            }
            if (ddlItemHoverCssClass.Text != "Default")
            {
                strbInfo.Append("\n ItemHoverCssClass=\"" + ctrlAutoCompleteExtender.ItemHoverCssClass.ToString() + "\" ");
            }
            if (ddlErrorCssClass.Text != "Default")
            {
                strbInfo.Append("\n ErrorCssClass=\"" + ctrlAutoCompleteExtender.ErrorCssClass.ToString() + "\" ");
            }
            if (ddlDataTextFormatString.Text != "Default")
            {
                strbInfo.Append("\n DataTextFormatString=\"" + ctrlAutoCompleteExtender.DataTextFormatString.ToString() + "\" ");
            }
            if (ddlDataValueField.Text != "Default")
            {
                strbInfo.Append("\n DataValueField=\"" + ctrlAutoCompleteExtender.DataValueField.ToString() + "\" ");
            }
            strbInfo.Append("\n/>\n");
            string[] strsTrans = ctrlAutoCompleteExtender.DataTextFieldList;
            strbInfo.Append("\n DataTextFieldList={\""+strsTrans[0]+"\"");
            for (int i = 1; i < strsTrans.Length; i++)
            {
                strbInfo.Append(",\""+strsTrans[i]+"\"");
            }
            strbInfo.Append("}\n");
            strsTrans = ctrlAutoCompleteExtender.CompareFieldName;
            strbInfo.Append("\n CompareFieldName={\"" + strsTrans[0] + "\"");
            for (int i = 1; i < strsTrans.Length; i++)
            {
                strbInfo.Append(",\"" + strsTrans[i] + "\"");
            }
            strbInfo.Append("}\n");
            return strbInfo.ToString();
        }

        protected void btnSetProperties_Click(object sender, EventArgs e)
        {
            ctrlAutoCompleteExtender.AutoCallBack = ckbAutoCallBack.Checked;
            ctrlAutoCompleteExtender.IsAutoComplete = ckbIsAutoComplete.Checked;
            ctrlAutoCompleteExtender.RequireValidation = ckbRequireValidation.Checked;
            ctrlAutoCompleteExtender.EnableCaching = ckbEnableCaching.Checked;
            ctrlAutoCompleteExtender.MinimumPrefixLength = Convert.ToInt32(txbMinimumPrefixLength.Text);
            ctrlAutoCompleteExtender.CompletionInterval = Convert.ToInt32(txbCompletionInterval.Text);
            ctrlAutoCompleteExtender.MaxCompletionRecordCount = Convert.ToInt32(txbMaxCompletionRecordCount.Text);
            ctrlAutoCompleteExtender.MaxPopupWindowHeight = Convert.ToInt32(txbMaxPopupWindowHeight.Text);
            if (ddlCompletionBodyCssClass.Text != "Default")
            {
                //ctrlAutoCompleteExtender.CompletionBodyCssClass = ddlCompletionBodyCssClass.Text;
            }
            if (ddlItemCssClass.Text != "Default")
            {
                ctrlAutoCompleteExtender.ItemCssClass = ddlItemCssClass.Text;
            }
            if (ddlItemHoverCssClass.Text != "Default")
            {
                ctrlAutoCompleteExtender.ItemHoverCssClass = ddlItemHoverCssClass.Text;
            }
            if (ddlErrorCssClass.Text != "Default")
            {
                ctrlAutoCompleteExtender.ErrorCssClass = ddlErrorCssClass.Text;
            }
            ctrlAutoCompleteExtender.DataTextFieldList = ddlDataTextFieldList.Text.Split(',');
            if (ddlDataTextFormatString.Text == "Default")
            {
                ctrlAutoCompleteExtender.DataTextFormatString = "";
            }
            else
            {
                ctrlAutoCompleteExtender.DataTextFormatString = ddlDataTextFormatString.Text;                
            }
            if (ddlDataValueField.Text == "Default")
            {
                ctrlAutoCompleteExtender.DataValueField = "";
            }
            else
            {
                ctrlAutoCompleteExtender.DataValueField = ddlDataValueField.Text;
            }
            ctrlAutoCompleteExtender.CompareFieldName = ddlCompareFieldName.Text.Split(',');

            ctrlAutoCompleteExtenderHtmlShow.Value = BuildControlInfo();

            //���Ƶ��ڶ���AutoCompleteExtender�ؼ�
            ctrlAutoCompleteExtender2.AutoCallBack = ctrlAutoCompleteExtender.AutoCallBack;
            ctrlAutoCompleteExtender2.IsAutoComplete = ctrlAutoCompleteExtender.IsAutoComplete;
            ctrlAutoCompleteExtender2.RequireValidation = ctrlAutoCompleteExtender.RequireValidation;
            ctrlAutoCompleteExtender2.EnableCaching = ctrlAutoCompleteExtender.EnableCaching;
            ctrlAutoCompleteExtender2.MinimumPrefixLength = ctrlAutoCompleteExtender.MinimumPrefixLength;
            ctrlAutoCompleteExtender2.CompletionInterval = ctrlAutoCompleteExtender.CompletionInterval;
            ctrlAutoCompleteExtender2.MaxCompletionRecordCount = ctrlAutoCompleteExtender.MaxCompletionRecordCount;
            ctrlAutoCompleteExtender2.MaxPopupWindowHeight = ctrlAutoCompleteExtender.MaxPopupWindowHeight;
            //ctrlAutoCompleteExtender2.CompletionBodyCssClass = ctrlAutoCompleteExtender.CompletionBodyCssClass;
            ctrlAutoCompleteExtender2.ItemCssClass = ctrlAutoCompleteExtender.ItemCssClass;
            ctrlAutoCompleteExtender2.ItemHoverCssClass = ctrlAutoCompleteExtender.ItemHoverCssClass;
            ctrlAutoCompleteExtender2.ErrorCssClass = ctrlAutoCompleteExtender.ErrorCssClass;

            ctrlAutoCompleteExtender2.DataTextFieldList = ctrlAutoCompleteExtender.DataTextFieldList;
            ctrlAutoCompleteExtender2.DataTextFormatString = ctrlAutoCompleteExtender.DataTextFormatString;
            ctrlAutoCompleteExtender2.DataValueField = ctrlAutoCompleteExtender.DataValueField;
            ctrlAutoCompleteExtender2.CompareFieldName = ctrlAutoCompleteExtender.CompareFieldName;
        }

        protected void btnGetData_Click(object sender, EventArgs e)
        {
            txbGetText.Text = ctrlAutoCompleteExtender.Text;
            txbSetText.Text = txbGetText.Text;
            txbGetSelectValue.Text = ctrlAutoCompleteExtender.SelectValue;
        }

        protected void btnSetData_Click(object sender, EventArgs e)
        {
            ctrlAutoCompleteExtender.Text = txbSetText.Text;
        }

        protected void ctrlAutoCompleteExtender_GetDataSource(string sPrefix, int iCount, object context, ref IEnumerable result)
        {
            DataTable dtTrans = new DataTable();
            dtTrans.Columns.Add("ID", typeof(string));
            dtTrans.Columns.Add("Text", typeof(string));
            dtTrans.Columns.Add("Value", typeof(string));
            DataRow datarowTrans;
            if (iCount == -1)
            {
                iCount = 26;
            }
            for (int i = 0; i < iCount; i++)
            {
                datarowTrans = dtTrans.NewRow();
                datarowTrans["ID"] = i.ToString();
                datarowTrans["Text"] = sPrefix + Convert.ToChar(i + 97);
                datarowTrans["Value"] = "Value_" + i.ToString();
                dtTrans.Rows.Add(datarowTrans);
            }
            //ctrlAutoCompleteExtender.DataSource = dtTrans;
            result = (IEnumerable)dtTrans.DefaultView;
        }

        protected void ctrlAutoCompleteExtender2_GetDataSource(string sPrefix, int iCount, object context, ref IEnumerable result)
        {
            DataTable dtTrans = new DataTable();
            dtTrans.Columns.Add("ID", typeof(string));
            dtTrans.Columns.Add("Text", typeof(string));
            dtTrans.Columns.Add("Value", typeof(string));
            DataRow datarowTrans;
            if (iCount == -1)
            {
                iCount = 26;
            }
            for (int i = 0; i < iCount; i++)
            {
                datarowTrans = dtTrans.NewRow();
                datarowTrans["ID"] = i.ToString();
                datarowTrans["Text"] = sPrefix + Convert.ToChar(i + 65);
                datarowTrans["Value"] = "Value_" + i.ToString();
                dtTrans.Rows.Add(datarowTrans);
            }
            result = (IEnumerable)dtTrans.DefaultView;
        }
    }
}
