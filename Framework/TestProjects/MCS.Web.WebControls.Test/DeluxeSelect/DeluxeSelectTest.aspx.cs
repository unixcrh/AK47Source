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

namespace MCS.Web.WebControls.Test.DeluxeSelect
{
    public partial class DeluxeSelectTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SelectItem s = new SelectItem();
                s.SelectListBoxText = "t";
                s.SelectListBoxValue = "t";
                this.ctrlDeluxeSelect.SelectedItems.Add(s);
            }

            #region ��������
            //ccDeluxeSelect.ShowSelectButton;              //'ѡ��'��ť�Ƿ���ʾ
            //ccDeluxeSelect.SelectButtonText;              //'ѡ��'��ť��Text
            //ccDeluxeSelect.SelectButtonCssClass;          //'ѡ��'��ť��Css��ʽ

            //ccDeluxeSelect.CancelButtonText;              //'ȡ��'��ť��Text
            //ccDeluxeSelect.CancelButtonCssClass;          //'ȡ��'��ť��Css��ʽ

            //ccDeluxeSelect.ShowAllSelectButton;           //'ȫ��ѡ��'��ť�Ƿ���ʾ
            //ccDeluxeSelect.SelectAllButtonText;           //'ȫ��ѡ��'��ť��Text

            //ccDeluxeSelect.CancelAllButtonText;           //'ȫ��ȡ��'��ť��Text

            //ccDeluxeSelect.MoveButtonCssClass;            //'������'��ť��Css��ʽ

            //ccDeluxeSelect.AppendDataBoundItems;          //�Ƿ񸽼�Items
            //ccDeluxeSelect.ButtonItems;                   //��ť�����ݼ�
            //ccDeluxeSelect.ClientID;

            //ccDeluxeSelect.DataSourceResult;              //����Դ
            //ccDeluxeSelect.DataSourseSortField;           //����Դ��SortFiled
            //ccDeluxeSelect.DataSourseTextField;           //����Դ��TextFiled
            //ccDeluxeSelect.DataSourseValueField;          //����Դ��ValueFiled
            //ccDeluxeSelect.DataTextFormatString;          //����Դ��TextFiled��Format

            //ccDeluxeSelect.GetSelectDesignHTML();

            //ccDeluxeSelect.SelectedItems;                 //��ѡ���б�����ݼ�
            //ccDeluxeSelect.SelectedListCssClass;          //��ѡ���б��Css��ʽ
            //ccDeluxeSelect.SelectedListSortDirection;     //��ѡ���б�����ʽ�����򡢽���
            //ccDeluxeSelect.SelectedSelectionMode;         //��ѡ���б��ѡ��ģʽ����ѡ��ѡ��

            //ccDeluxeSelect.CandidateItems;                //��ѡ���б�����ݼ�
            //ccDeluxeSelect.CandidateListCssClass;         //��ѡ���б��Css��ʽ
            //ccDeluxeSelect.CandidateListSortDirection;    //��ѡ���б�����ʽ�����򡢽���
            //ccDeluxeSelect.CandidateSelectionMode;        //��ѡ���б��ѡ��ģʽ����ѡ��ѡ��

            //-------------------------------------------------------------------------------
            //ctrlDeluxeSelect.ButtonItems;                 ��ť������ݼ���
            //ctrlDeluxeSelect.GetSelectDesignHTML();

            //ctrlDeluxeSelect.ShowSelectButton;            ѡ��ť�Ƿ���ʾ
            //ctrlDeluxeSelect.ShowSelectAllButton;         ȫ��ѡ��ť�Ƿ���ʾ
            //ctrlDeluxeSelect.SelectButtonText;            ѡ��ť��Text
            //ctrlDeluxeSelect.SelectAllButtonText;         ȫ��ѡ��ť��Text
            //ctrlDeluxeSelect.CancelButtonText;            ȡ����ť��Text
            //ctrlDeluxeSelect.CancelAllButtonText;         ȫ��ȡ����ť��Text

            //ctrlDeluxeSelect.SelectButtonCssClass;        'ѡ��'��ť��Css��ʽ
            //ctrlDeluxeSelect.MoveButtonCssClass;          '������'��ť��Css��ʽ 

            //ctrlDeluxeSelect.MoveOption;                  �Ƿ������ƶ��б�������

            //ctrlDeluxeSelect.DataSourseSortField;         ����Դ��SortFiled
            //ctrlDeluxeSelect.DataSourseTextField;         ����Դ��TextFiled
            //ctrlDeluxeSelect.DataSourseValueField;        ����Դ��ValueFiled

            //ctrlDeluxeSelect.SelectedItems;               ��ѡ���б�����ݼ���
            //ctrlDeluxeSelect.SelectedListCssClass;        ��ѡ���б��Css��ʽ
            //ctrlDeluxeSelect.SelectedListSortDirection;   ��ѡ���б�����ʽ������\����
            //ctrlDeluxeSelect.SelectedSelectionMode;       ��ѡ���б��ѡ��ģʽ����ѡ\��ѡ��

            //ctrlDeluxeSelect.CandidateItems;              ��ѡ���б�����ݼ�
            //ctrlDeluxeSelect.CandidateListCssClass;       ��ѡ���б��Css��ʽ
            //ctrlDeluxeSelect.CandidateListSortDirection;  ��ѡ���б�����ʽ������\����
            //ctrlDeluxeSelect.CandidateSelectionMode;      ��ѡ���б��ѡ��ģʽ����ѡ\��ѡ��
            #endregion
        }

        private string BuildControlInfo()
        {
            StringBuilder strbInfo = new StringBuilder(512);

            strbInfo.Append("<cc1:DeluxeSelect ID=\"ctrlDeluxeSelect\" runat=\"server\"\n DataSourseSortField=\"\"\n DataSourseTextField=\"\"\n DataSourseValueField=\"\"");

            if (!ckbShowSelectButton.Checked)
            {
                strbInfo.Append("\n ShowSelectButton=\"" + ctrlDeluxeSelect.ShowSelectButton.ToString() + "\" ");
            }
            if (!ckbShowSelectAllButton.Checked)
            {
                strbInfo.Append("\n ShowSelectAllButton=\"" + ctrlDeluxeSelect.ShowSelectAllButton.ToString() + "\" ");
            }

            strbInfo.Append("\n SelectButtonText=\"" + ctrlDeluxeSelect.SelectButtonText.ToString() + "\" ");
            strbInfo.Append("\n SelectAllButtonText=\"" + ctrlDeluxeSelect.SelectAllButtonText.ToString() + "\" ");

            strbInfo.Append("\n CancelButtonText=\"" + ctrlDeluxeSelect.CancelButtonText.ToString() + "\" ");
            strbInfo.Append("\n CancelAllButtonText=\"" + ctrlDeluxeSelect.CancelAllButtonText.ToString() + "\" ");

            if (ddlSelectButtonCssClass.Text != "Default")
            {
                strbInfo.Append("\n SelectButtonCssClass=\"" + ctrlDeluxeSelect.SelectButtonCssClass.ToString() + "\" ");
            }
            if (ddlMoveButtonCssClass.Text != "Default")
            {
                strbInfo.Append("\n MoveButtonCssClass=\"" + ctrlDeluxeSelect.MoveButtonCssClass.ToString() + "\" ");
            }

            strbInfo.Append("\n MoveOption=\"" + ctrlDeluxeSelect.MoveOption.ToString() + "\" ");

            if (ddlSelectedListCssClass.Text != "Default")
            {
                strbInfo.Append("\n SelectedListCssClass=\"" + ctrlDeluxeSelect.SelectedListCssClass.ToString() + "\" ");
            }
            strbInfo.Append("\n SelectedListSortDirection=\"" + ctrlDeluxeSelect.SelectedListSortDirection.ToString() + "\" ");
            strbInfo.Append("\n SelectedSelectionMode=\"" + ctrlDeluxeSelect.SelectedSelectionMode.ToString() + "\" ");

            if (ddlCandidateListCssClass.Text != "Default")
            {
                strbInfo.Append("\n CandidateListCssClass=\"" + ctrlDeluxeSelect.CandidateListCssClass.ToString() + "\" ");
            }
            strbInfo.Append("\n CandidateListSortDirection=\"" + ctrlDeluxeSelect.CandidateListSortDirection.ToString() + "\" ");
            strbInfo.Append("\n CandidateSelectionMode=\"" + ctrlDeluxeSelect.CandidateSelectionMode.ToString() + "\" ");

            strbInfo.Append("\n/>");

            return strbInfo.ToString();
        }

        protected void btnSetProperties_Click(object sender, EventArgs e)
        {
            ctrlDeluxeSelect.ShowSelectButton = ckbShowSelectButton.Checked;
            ctrlDeluxeSelect.ShowSelectAllButton = ckbShowSelectAllButton.Checked;

            ctrlDeluxeSelect.SelectButtonText = ddlSelectButtonText.Text;
            ctrlDeluxeSelect.SelectAllButtonText = ddlSelectAllButtonText.Text;

            ctrlDeluxeSelect.CancelButtonText = ddlCancelButtonText.Text;
            ctrlDeluxeSelect.CancelAllButtonText = ddlCancelAllButtonText.Text;

            if (ddlSelectButtonCssClass.Text != "Default")
            {
                ctrlDeluxeSelect.SelectButtonCssClass = ddlSelectButtonCssClass.Text;
            }
            if (ddlMoveButtonCssClass.Text != "Default")
            {
                ctrlDeluxeSelect.MoveButtonCssClass = ddlMoveButtonCssClass.Text;
            }

            ctrlDeluxeSelect.MoveOption = ckbMoveOption.Checked;

            //��ѡ���б�
            if (ddlSelectedListCssClass.Text != "Default")
            {
                ctrlDeluxeSelect.SelectedListCssClass = ddlSelectedListCssClass.Text;
            }
            if (ddlSelectedListSortDirection.Text == "����")
            {
                ctrlDeluxeSelect.SelectedListSortDirection = System.ComponentModel.ListSortDirection.Ascending;
            }
            else if (ddlSelectedListSortDirection.Text == "����")
            {
                ctrlDeluxeSelect.SelectedListSortDirection = System.ComponentModel.ListSortDirection.Descending;
            }
            if (ddlSelectedSelectionMode.Text == "��ѡ")
            {
                ctrlDeluxeSelect.SelectedSelectionMode = false;
            }
            else if (ddlSelectedSelectionMode.Text == "��ѡ")
            {
                ctrlDeluxeSelect.SelectedSelectionMode = true;
            }

            //��ѡ���б�
            if (ddlCandidateListCssClass.Text != "Default")
            {
                ctrlDeluxeSelect.CandidateListCssClass = ddlCandidateListCssClass.Text;
            }
            if (ddlCandidateListSortDirection.Text == "����")
            {
                ctrlDeluxeSelect.CandidateListSortDirection = System.ComponentModel.ListSortDirection.Ascending;
            }
            else if (ddlCandidateListSortDirection.Text == "����")
            {
                ctrlDeluxeSelect.CandidateListSortDirection = System.ComponentModel.ListSortDirection.Descending;
            }
            if (ddlCandidateSelectionMode.Text == "��ѡ")
            {
                ctrlDeluxeSelect.CandidateSelectionMode = false;
            }
            else if (ddlCandidateSelectionMode.Text == "��ѡ")
            {
                ctrlDeluxeSelect.CandidateSelectionMode = true;
            }

            ctrlDeluxeSelectHtmlShow.Value = this.BuildControlInfo();
        }

        protected void btnSelectedAdd_Click(object sender, EventArgs e)
        {
            SelectItem sitemTrans = new SelectItem(); ;
            sitemTrans.SelectListBoxText = txbSelectedText.Text;
            sitemTrans.SelectListBoxValue = txbSelectedValue.Text;
            sitemTrans.SelectListBoxSortColumn = txbSelectedSort.Text;
            sitemTrans.Locked = ckbSelectedLocked.Checked;
            ctrlDeluxeSelect.SelectedItems.Add(sitemTrans);
        }

        protected void btnCandidateAdd_Click(object sender, EventArgs e)
        {
            SelectItem sitemTrans = new SelectItem(); ;
            sitemTrans.SelectListBoxText = txbCandidateText.Text;
            sitemTrans.SelectListBoxValue = txbCandidateValue.Text;
            sitemTrans.SelectListBoxSortColumn = txbCandidateSort.Text;
            sitemTrans.Locked = ckbCandidateLocked.Checked;
            ctrlDeluxeSelect.CandidateItems.Add(sitemTrans);
        }

        protected void btnSetDataSource_Click(object sender, EventArgs e)
        {
            DataTable dtTrans = new DataTable();
            dtTrans.Columns.Add("Column1", typeof(String));
            dtTrans.Columns.Add("Column2", typeof(String));
            dtTrans.Columns.Add("Column3", typeof(String));

            DataRow drTrans;
            drTrans = dtTrans.NewRow();
            drTrans["Column1"] = "��������";
            drTrans["Column2"] = "0001";
            drTrans["Column3"] = "1";
            dtTrans.Rows.Add(drTrans);

            drTrans = dtTrans.NewRow();
            drTrans["Column1"] = "��������";
            drTrans["Column2"] = "7200";
            drTrans["Column3"] = "2";
            dtTrans.Rows.Add(drTrans);

            drTrans = dtTrans.NewRow();
            drTrans["Column1"] = "��������";
            drTrans["Column2"] = "5700";
            drTrans["Column3"] = "3";
            dtTrans.Rows.Add(drTrans);

            ctrlDeluxeSelect.DataSource = dtTrans;

            ctrlDeluxeSelect.DataSourseTextField = "Column1";
            ctrlDeluxeSelect.DataSourseValueField = "Column2";
            ctrlDeluxeSelect.DataSourseSortField = "Column3";
        }

        protected void btnButtonItemAdd_Click(object sender, EventArgs e)
        {
            ButtonItem btnitmTrans = new ButtonItem();
            btnitmTrans.ButtonName = txbButtonName.Text;
            btnitmTrans.ButtonSortID = Convert.ToInt16(txbButtonSortID.Text);
            btnitmTrans.ButtonTypeMaxCount = Convert.ToInt16(txbButtonTypeMaxCount.Text);
            btnitmTrans.ButtonType = ButtonItem.ButtonTypeMode.Button;
            btnitmTrans.ButtonCssClass = ddlButtonCssClass.Text;
            ctrlDeluxeSelect.ButtonItems.Add(btnitmTrans);
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            this.Label1.Text =
                string.Format("InsertedItems {0} <br/> DeletedItems {1}",
                this.ctrlDeluxeSelect.DeltaItems.InsertedItems.Count.ToString(),
                this.ctrlDeluxeSelect.DeltaItems.DeletedItems.Count.ToString());
        }
    }
}
