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

namespace MCS.Web.WebControls.Test.DateTimeControl
{
    public partial class DeluxeDateTimeTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ctrlDeluxeDateTime.Animated;    //���������·�ת���Ķ���Ч��
            //ctrlDeluxeDateTime.DateAutoComplete;    //�Ƿ��Զ���������
            //ctrlDeluxeDateTime.EnabledOnClient;     //�Ƿ�������������
            //ctrlDeluxeDateTime.IsComplexHeader;     //�Ƿ��ṩ��������ѡ��
            //ctrlDeluxeDateTime.IsOnlyCurrentMonth;    //�Ƿ�ֻ��ʾ����
            //ctrlDeluxeDateTime.IsValidDateValue;    //�Ƿ�����������֤
            //ctrlDeluxeDateTime.IsValidTimeValue;    //�Ƿ�����ʱ����֤
            //ctrlDeluxeDateTime.ReadOnly;    //�Ƿ�ֻ��
            //ctrlDeluxeDateTime.ShowButton;      //�Ƿ��ṩ��ť��ѡ���Զ���ʱ���б�,����������������Դ
            //ctrlDeluxeDateTime.TimeAutoComplete;    //�Ƿ��Զ�����ʱ��


            //ctrlDeluxeDateTime.DateImageCssClass;   //ͼƬ��CssClass
            //ctrlDeluxeDateTime.DateImageStyle;  //ͼƬ��Style
            //ctrlDeluxeDateTime.DateImageUrl;    //��ťͼƬ��Src
            //ctrlDeluxeDateTime.DateTextCssClass;    //����������Css
            //ctrlDeluxeDateTime.TimeTextCssClass;    //ʱ��������Css

            //ctrlDeluxeDateTime.DateAutoCompleteValue;   //�ṩ�Զ������ʱ�䴮����������ȡϵͳ����
            //ctrlDeluxeDateTime.DateCurrentMessageError;     //��֤���ڵ���ʾ��Ϣ
            //ctrlDeluxeDateTime.DatePromptCharacter;     //���������ַ�
            //ctrlDeluxeDateTime.TimePromptCharacter;     //�����ַ�
            //ctrlDeluxeDateTime.TimeMask;    //ʱ��ķָ���
            //ctrlDeluxeDateTime.TimeAutoCompleteValue;   //�ṩ�Զ������ʱ�䴮����������ȡϵͳʱ��
            //ctrlDeluxeDateTime.TimeCurrentMessageError; //��֤ʱ�����ʾ��Ϣ

            //ctrlDeluxeDateTime.OnClientDateSelectionChanged;    //����ѡ��仯�󴥷��Ŀͻ����¼�
            //ctrlDeluxeDateTime.OnClientHidden;  //���������󴥷��Ŀͻ����¼�
            //ctrlDeluxeDateTime.OnClientHiding;  //��������ʱ�����Ŀͻ����¼�
            //ctrlDeluxeDateTime.OnClientShowing;     //��������ʱ�����Ŀͻ����¼�
            //ctrlDeluxeDateTime.OnClientShown;   //���������󴥷��Ŀͻ����¼�

            //ctrlDeluxeDateTime.Value;   //��������

            //ctrlDeluxeDateTime.OnFocusDateCssClass;     //���ڵõ�����ʱ�ı������ʽ
            //ctrlDeluxeDateTime.OnInvalidDateCssClass;   //������֤��ͨ��ʱ�ı������ʽ
            //ctrlDeluxeDateTime.OnTimeFocusCssClass;     //�õ�����ʱʱ���ı������ʽ
            //ctrlDeluxeDateTime.OnTimeInvalidCssClass;   //��֤��ͨ��ʱʱ���ı������ʽ
            //ctrlDeluxeDateTime.PopupCalendarCssClass;   //��������ʽ������ΪĬ����ʽ


            //ctrlDeluxeDateTime.DataSource;      //��ʱ�������Դ

            //ctrlDeluxeDateTime.DateTextStyle;   //������Style    //�Ѿ�������
            //ctrlDeluxeDateTime.TimeTextStyle;   //ʱ��������Style   //Ҳ������

            //ctrlDeluxeDateTime.FirstDayOfWeek;  //�Զ����һ���Ǵ��ܼ���ʼ

            
        }

        protected override void OnInit(EventArgs e)
        {
			//Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientDateSelectionChanged", @"<script>function funcOnClientDateSelectionChanged(){alert('Event OnClientDateSelectionChanged');} </script>", false);
			//Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientHidden", @"<script>function funcOnClientHidden(){alert('Event OnClientHidden');} </script>", false);
			//Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientHiding", @"<script>function funcOnClientHiding(){alert('Event OnClientHiding');} </script>", false);
			//Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientShowing", @"<script>function funcOnClientShowing(){alert('Event OnClientShowing');} </script>", false);
			//Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientShown", @"<script>function funcOnClientShown(){alert('Event OnClientShown');} </script>", false);
            //ctrlDeluxeDateTime.OnClientDateSelectionChanged = "funcOnClientDateSelectionChanged";
            //ctrlDeluxeDateTime.OnClientHidden = "funcOnClientHidden";
            //ctrlDeluxeDateTime.OnClientHiding = "funcOnClientHiding";
            //ctrlDeluxeDateTime.OnClientShowing = "funcOnClientShowing";
            //ctrlDeluxeDateTime.OnClientShown = "funcOnClientShown";
            base.OnInit(e);
        }

        protected void btnSetDataSource_Click(object sender, EventArgs e)
        {
            ListItem lsitOption=new ListItem();
            lsitOption.Text = txbDataSource1.Text;
            ctrlDeluxeDateTime.DataSource.Add(lsitOption);
        }


        private string BuildControlInfo()
        {
            StringBuilder strbInfo = new StringBuilder(512);

            strbInfo.Append("<cc1:DeluxeDateTime ID=\"ctrlDeluxeDateTime\"\n runat=\"server\"");

            strbInfo.Append("\n ReadOnly=\"" + ctrlDeluxeDateTime.ReadOnly.ToString() + "\"");
            //strbInfo.Append(" Value=\"" + ctrlDeluxeDateTime.Value.ToString() + "\"\n");

            //���ڲ���
            strbInfo.Append("\n DateAutoComplete=\"" + ctrlDeluxeDateTime.DateAutoComplete.ToString() + "\"");
            strbInfo.Append("\n IsValidDateValue=\"" + ctrlDeluxeDateTime.IsValidDateValue.ToString() + "\"");

            if (ddlDateTextCssClass.Text != "Default")
            {
                strbInfo.Append("\n DateTextCssClass=\"" + ctrlDeluxeDateTime.DateTextCssClass.ToString() + "\"");
            }
            if (txbDateAutoCompleteValue.Text != string.Empty)
            {
                strbInfo.Append("\n DateAutoCompleteValue=\"" + ctrlDeluxeDateTime.DateAutoCompleteValue.ToString() + "\"");
            }
            strbInfo.Append("\n DateCurrentMessageError=\"" + ctrlDeluxeDateTime.DateCurrentMessageError.ToString() + "\"");
            strbInfo.Append("\n DatePromptCharacter=\"" + ctrlDeluxeDateTime.DatePromptCharacter.ToString() + "\"");

            if (ddlOnFocusDateCssClass.Text != "Default")
            {
                strbInfo.Append("\n OnFocusDateCssClass=\"" + ctrlDeluxeDateTime.OnFocusDateCssClass.ToString() + "\"");
            }
            if (ddlOnInvalidDateCssClass.Text != "Default")
            {
                strbInfo.Append("\n OnInvalidDateCssClass=\"" + ctrlDeluxeDateTime.OnInvalidDateCssClass.ToString() + "\"");
            }
           
            //------------------------------------------------------------------------------------------------------

            strbInfo.Append("\n Animated=\"" + ctrlDeluxeDateTime.Animated.ToString() + "\"");
            strbInfo.Append("\n EnabledOnClient=\"" + ctrlDeluxeDateTime.EnabledOnClient.ToString() + "\"");
            strbInfo.Append("\n IsOnlyCurrentMonth=\"" + ctrlDeluxeDateTime.IsOnlyCurrentMonth.ToString() + "\"");
            strbInfo.Append("\n IsComplexHeader=\"" + ctrlDeluxeDateTime.IsComplexHeader.ToString() + "\"");

            if (ddlDateTextCssClass.Text != "Default")
            {
                strbInfo.Append("\n DateImageCssClass=\"" + ctrlDeluxeDateTime.DateImageCssClass.ToString() + "\"");
            }
            if (ddlDateImageStyle.Text != "Default")
            {
                strbInfo.Append("\n DateImageStyle=\"" + ctrlDeluxeDateTime.DateImageStyle.ToString() + "\"");
            }
           if (ddlDateImageUrl.Text != "Default")
            {
                strbInfo.Append("\n DateImageUrl=\"" + ctrlDeluxeDateTime.DateImageUrl.ToString() + "\"");
            }
            if (ddlOnTimeInvalidCssClass.Text != "Default")
            {
                strbInfo.Append("\n OnTimeInvalidCssClass=\"" + ctrlDeluxeDateTime.OnTimeInvalidCssClass.ToString() + "\"");
            }
            if (ddlPopupCalendarCssClass.Text != "Default")
            {
                strbInfo.Append("\n PopupCalendarCssClass=\"" + ctrlDeluxeDateTime.PopupCalendarCssClass.ToString() + "\"");
            }
            if (ddlDateImageCssClass.Text != "Default")
            {
                strbInfo.Append("\n DateImageCssClass=\"" + ctrlDeluxeDateTime.DateImageCssClass.ToString() + "\"");
            }
           


            //ʱ�䲿��
            strbInfo.Append("\n TimeAutoComplete=\"" + ctrlDeluxeDateTime.TimeAutoComplete.ToString() + "\"");
            strbInfo.Append("\n IsValidTimeValue=\"" + ctrlDeluxeDateTime.IsValidTimeValue.ToString() + "\"");

            if (ddlTimeTextCssClass.Text != "Default")
            {
                strbInfo.Append("\n TimeTextCssClass=\"" + ctrlDeluxeDateTime.TimeTextCssClass.ToString() + "\"");
            }
            if (txbTimeAutoCompleteValue.Text != string.Empty)
            {
                strbInfo.Append("\n TimeAutoCompleteValue=\"" + ctrlDeluxeDateTime.TimeAutoCompleteValue.ToString() + "\"");
            }
            strbInfo.Append("\n TimeCurrentMessageError=\"" + ctrlDeluxeDateTime.TimeCurrentMessageError.ToString() + "\"");
            strbInfo.Append("\n TimePromptCharacter=\"" + ctrlDeluxeDateTime.TimePromptCharacter.ToString() + "\"");

            if (ddlOnTimeFocusCssClass.Text != "Default")
            {
                strbInfo.Append("\n OnTimeFocusCssClass=\"" + ctrlDeluxeDateTime.OnTimeFocusCssClass.ToString() + "\"");
            }
            if (ddlOnTimeInvalidCssClass.Text != "Default")
            {
                strbInfo.Append("\n OnTimeInvalidCssClass=\"" + ctrlDeluxeDateTime.OnTimeInvalidCssClass.ToString() + "\"");
            }
           
            //-----------------------------------------------------------------------------------------------------

            strbInfo.Append("\n ShowButton=\"" + ctrlDeluxeDateTime.ShowButton.ToString() + "\"");

            strbInfo.Append("\n TimeMask=\"" + ctrlDeluxeDateTime.TimeMask.ToString() + "\"");

            ctrlDeluxeDateTime.FirstDayOfWeek = (FirstDayOfWeek)Enum.Parse(typeof(FirstDayOfWeek), this.ddlFirstDayOfWeek.Text.Trim());

            strbInfo.Append("\n>");

            return strbInfo.ToString();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
    
            ctrlDeluxeDateTime.ReadOnly = ckbReadOnly.Checked;
            //ctrlDeluxeDateTime.Value = Convert.ToDateTime(txbValue.Text.Trim());

            //���ڲ��ָ�ֵ
            ctrlDeluxeDateTime.DateAutoComplete = ckbDateAutoComplete.Checked;
            ctrlDeluxeDateTime.IsValidDateValue = ckbIsValidDateValue.Checked;

            if (ddlDateTextCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.DateTextCssClass = ddlDateTextCssClass.Text;
            }

            ctrlDeluxeDateTime.DateAutoCompleteValue = txbDateAutoCompleteValue.Text;
            ctrlDeluxeDateTime.DateCurrentMessageError = txbDateCurrentMessageError.Text;
            ctrlDeluxeDateTime.DatePromptCharacter = txbDatePromptCharacter.Text;

            if (ddlOnFocusDateCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.OnFocusDateCssClass = ddlOnFocusDateCssClass.Text;
            }
            if (ddlOnInvalidDateCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.OnInvalidDateCssClass = ddlOnInvalidDateCssClass.Text;
            }

            //------------------------------------------------------------------------
            ctrlDeluxeDateTime.Animated = ckbAnimated.Checked;
            ctrlDeluxeDateTime.EnabledOnClient = ckbEnabledOnClient.Checked;
            ctrlDeluxeDateTime.IsOnlyCurrentMonth = ckbIsOnlyCurrentMonth.Checked;
            ctrlDeluxeDateTime.IsComplexHeader = ckbIsComplexHeader.Checked;

            if (ddlDateImageCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.DateImageCssClass = ddlDateImageCssClass.Text;
            }
            if (ddlDateImageStyle.Text != "Default")
            {
                ctrlDeluxeDateTime.DateImageStyle = ddlDateImageStyle.Text;
            }
            if (ddlDateImageUrl.Text != "Default")
            {
                ctrlDeluxeDateTime.DateImageUrl = ddlDateImageUrl.Text;
            }
            if (ddlOnTimeInvalidCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.OnTimeInvalidCssClass = ddlOnTimeInvalidCssClass.Text;
            }
            if (ddlPopupCalendarCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.PopupCalendarCssClass = ddlPopupCalendarCssClass.Text;
            }


            //ʱ�䲿�ָ�ֵ
            ctrlDeluxeDateTime.TimeAutoComplete = ckbTimeAutoComplete.Checked;
            ctrlDeluxeDateTime.IsValidTimeValue = ckbIsValidTimeValue.Checked;
            if (ddlTimeTextCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.TimeTextCssClass = ddlTimeTextCssClass.Text;
            }

            //if (txbTimeAutoCompleteValue.Text != string.Empty)
            //{
                ctrlDeluxeDateTime.TimeAutoCompleteValue = txbTimeAutoCompleteValue.Text;
            //}
            ctrlDeluxeDateTime.TimeCurrentMessageError = txbTimeCurrentMessageError.Text;
            ctrlDeluxeDateTime.TimePromptCharacter = txbTimePromptCharacter.Text;

            if (ddlOnTimeFocusCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.OnTimeFocusCssClass = ddlOnTimeFocusCssClass.Text;
            }
            if (ddlOnTimeInvalidCssClass.Text != "Default")
            {
                ctrlDeluxeDateTime.OnTimeInvalidCssClass = ddlOnTimeInvalidCssClass.Text;
            }

            //-------------------------------------------------------------------------
            ctrlDeluxeDateTime.ShowButton = ckbShowButton.Checked;

            ctrlDeluxeDateTime.TimeMask = txbTimeMask.Text;

            ctrlDeluxeDateTime.FirstDayOfWeek = (FirstDayOfWeek)Enum.Parse(typeof(FirstDayOfWeek), this.ddlFirstDayOfWeek.Text.Trim());
           
            ctrlDeluxeDateTimeAttributeShow.Value = this.BuildControlInfo();
			if(this.ckb_ClientScript.Checked)
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientDateSelectionChanged", @"<script>function funcOnClientDateSelectionChanged(){$get('clientEvent').innerHTML += 'Event: OnClientDateSelectionChanged &nbsp;&nbsp;&nbsp;'} </script>", false);
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientHidden", @"<script>function funcOnClientHidden(){$get('clientEvent').innerHTML += 'Event: OnClientHidden &nbsp;&nbsp;&nbsp;';} </script>", false);
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientHiding", @"<script>function funcOnClientHiding(){$get('clientEvent').innerHTML += 'Event OnClientHiding &nbsp;&nbsp;&nbsp;';} </script>", false);
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientShowing", @"<script>function funcOnClientShowing(){$get('clientEvent').innerHTML += 'Event: OnClientShowing &nbsp;&nbsp;&nbsp;';} </script>", false);
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "funcOnClientShown", @"<script>function funcOnClientShown(){$get('clientEvent').innerHTML += 'Event: OnClientShown &nbsp;&nbsp;&nbsp;';} </script>", false);

				ctrlDeluxeDateTime.OnClientDateSelectionChanged = "funcOnClientDateSelectionChanged";
				ctrlDeluxeDateTime.OnClientHidden = "funcOnClientHidden";
				ctrlDeluxeDateTime.OnClientHiding = "funcOnClientHiding";
				ctrlDeluxeDateTime.OnClientShowing = "funcOnClientShowing";
				ctrlDeluxeDateTime.OnClientShown = "funcOnClientShown";
				
			}
        }

        protected void btnGetValue_Click(object sender, EventArgs e)
        {
            txbGetValue.Text = ctrlDeluxeDateTime.Value.ToString();
            txbSetValue.Text = txbGetValue.Text;
        }

        protected void btnSetValue_Click(object sender, EventArgs e)
        {
            ctrlDeluxeDateTime.Value=System.DateTime.Parse(txbSetValue.Text);
        }

       
    


    }
}
