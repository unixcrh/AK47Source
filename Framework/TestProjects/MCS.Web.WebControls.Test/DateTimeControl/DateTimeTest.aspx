<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DateTimeTest.aspx.cs" Inherits="MCS.Web.WebControls.Test.DateTimeControl.DateTimeTest" %>

<%@ Register Assembly="MCS.Web.WebControls" Namespace="MCS.Web.WebControls"
    TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>DateTime Test</title>
    <link rel="stylesheet" href="dateTime.css" type="text/css" /> 
</head>
<body>

    <form id="form1" runat="server">
    <div style="text-align:left">
    
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true" runat="server">
    </asp:ScriptManager>
    <asp:CheckBox ID="ckbEnabled" runat="server" Text="Enabled" Checked="True" TextAlign="Left" /><br />
    <asp:Label ID="lblEnabled" runat="server" Font-Size="small" Font-Italic="true" Text="�������ÿؼ��Ƿ����"></asp:Label><br />
    <asp:CheckBox ID="ckbReadOnly" runat="server" Text="ReadOnly" TextAlign="Left" /><br />
    <asp:Label ID="Label20" runat="server" Font-Size="small" Font-Italic="true" Text="�������ÿؼ��Ƿ�ֻ��"></asp:Label><br />
    <hr />
    <table>
        <tr style="vertical-align:top">
            <td style="width:500px">
                  ��������
                <br />
                <asp:CheckBox ID="ckbIsValidDateValue" runat="server" Text="IsValidDateValue" TextAlign="Left" Checked="True" /><br />
                <asp:Label ID="Label12" runat="server" Font-Size="small" Font-Italic="true" Text="�����Ƿ�����������֤"></asp:Label><br />
                <asp:CheckBox ID="ckbDateAutoComplete" runat="server" Text="DateAutoComplete" TextAlign="Left" Checked="True" /><br />
                <asp:Label ID="Label13" runat="server" Font-Size="small" Font-Italic="true" Text="�����Ƿ��Զ���������"></asp:Label><br />
                DateAutoCompleteValue
                <asp:DropDownList ID="ddlDateAutoCompleteValue" runat="server">
                    <asp:ListItem>SystemDate</asp:ListItem>
                    <asp:ListItem>1981-12-14</asp:ListItem>
                    <asp:ListItem>2000-01-01</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label25" runat="server" Font-Size="small" Font-Italic="true" Text="�����ṩ�Զ���������ڴ�����������ȡϵͳ����"></asp:Label>
                <br />                    
                DatePromptCharacter
                <asp:TextBox ID="txbDatePromptCharacter" runat="server">%</asp:TextBox><br />
                <asp:Label ID="Label29" runat="server" Font-Size="small" Font-Italic="true" Text="�������������ַ�"></asp:Label><br />
                DateCurrentMessageError
                <asp:DropDownList ID="ddlDateCurrentMessageError" runat="server">
                    <asp:ListItem>���ڵĸ�ʽ������Ҫ��</asp:ListItem>
                    <asp:ListItem>�����ڲ����ϸ�ʽҪ��</asp:ListItem>
                    <asp:ListItem>�����ϸ�ʽҪ�������</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label26" runat="server" Font-Size="small" Font-Italic="true" Text="������֤������Ч�Ե���ʾ��Ϣ"></asp:Label><br />
                DateTextStyle
                <asp:DropDownList ID="ddlDateTextStyle" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>width: 100px; background-color:Aqua; text-align:left</asp:ListItem>
                    <asp:ListItem>width: 200px; font-size:larger; text-align:center</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label14" runat="server" Font-Size="small" Font-Italic="true" Text="��������������Style��������DateTextCssClass"></asp:Label><br />
                DateTextCssClass
                <asp:DropDownList ID="ddlDateTextCssClass" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>DateTextCssClass_Demo1</asp:ListItem>
                    <asp:ListItem>DateTextCssClass_Demo2</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label19" runat="server" Font-Size="small" Font-Italic="true" Text="��������������Css"></asp:Label><br />
                OnFocusDateCssClass
                <asp:DropDownList ID="ddlOnFocusDateCssClass" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>OnFocusDateCssClass_Demo1</asp:ListItem>
                    <asp:ListItem>OnFocusDateCssClass_Demo2</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label27" runat="server" Font-Size="small" Font-Italic="true" Text="�������ڵõ�����ʱ�ı������ʽ"></asp:Label><br />
                OnInvalidDateCssClass
                <asp:DropDownList ID="ddlOnInvalidDateCssClass" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>OnInvalidDateCssClass_Demo1</asp:ListItem>
                    <asp:ListItem>OnInvalidDateCssClass_Demo2</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label28" runat="server" Font-Size="small" Font-Italic="true" Text="����������֤��ͨ��ʱ�ı������ʽ"></asp:Label><br />
                <hr />
                <asp:CheckBox ID="ckbEnabledOnClient" runat="server" Text="EnabledOnClient" Checked="True" TextAlign="Left" /><br />
                <asp:Label ID="Label1" runat="server" Font-Size="small" Font-Italic="true" Text="�����Ƿ�������������"></asp:Label><br />
                <asp:CheckBox ID="ckbIsComplexHeader" runat="server" Text="IsComplexHeader" Checked="True" TextAlign="Left" /><br />
                <asp:Label ID="Label2" runat="server" Font-Size="small" Font-Italic="true" Text="�����Ƿ��ṩ��������ѡ��"></asp:Label><br />
                <asp:CheckBox ID="ckbIsOnlyCurrentMonth" runat="server" Text="IsOnlyCurrentMonth" Checked="True" TextAlign="Left" /><br />
                <asp:Label ID="Label3" runat="server" Font-Size="small" Font-Italic="true" Text="����ֻ��ʾ���£���������ͷ����β�Ĳ���"></asp:Label><br />
                FirstDayOfWeek
                <asp:DropDownList ID="ddlFirstDayOfWeek" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>Friday</asp:ListItem>
                    <asp:ListItem>Monday</asp:ListItem>
                    <asp:ListItem>Saturday</asp:ListItem>
                    <asp:ListItem>Sunday</asp:ListItem>
                    <asp:ListItem>Thursday</asp:ListItem>
                    <asp:ListItem>Tuesday</asp:ListItem>
                    <asp:ListItem>Wednesday</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label4" runat="server" Font-Size="small" Font-Italic="true" Text="������һ���Ǵ��ܼ���ʼ"></asp:Label><br />        
                PopupCalendarCssClass
                <asp:DropDownList ID="ddlPopupCalendarCssClass" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>PopupCalendarCssClass_Demo1</asp:ListItem>
                    <asp:ListItem>PopupCalendarCssClass_Demo2</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label9" runat="server" Font-Size="small" Font-Italic="true" Text="��������ʱ��ؼ���ʾģʽ"></asp:Label><br />
                DateImageStyle
                <asp:DropDownList ID="ddlDateImageStyle" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>width: 10px; background-color:Aqua; text-align:left</asp:ListItem>
                    <asp:ListItem>width: 20px; font-size:larger; text-align:center</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label31" runat="server" Font-Size="Small" Font-Italic="True" Text="����ͼƬ��Style��������DateImageCssClass"></asp:Label><br />
                DateImageCssClass
                <asp:DropDownList ID="ddlDateImageCssClass" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>DateImageCssClass_Demo1</asp:ListItem>
                    <asp:ListItem>DateImageCssClass_Demo2</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label30" runat="server" Font-Size="small" Font-Italic="true" Text="����ͼƬ��CssClass"></asp:Label><br />
                DateImageUrl
                <asp:DropDownList ID="ddlDateImageUrl" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>arrow-left</asp:ListItem>
                    <asp:ListItem>arrow-right</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label32" runat="server" Font-Size="small" Font-Italic="true" Text="������ťͼƬ��Src"></asp:Label>
            </td>
            <td style="width:500px">
                ���벿��<br />
                <asp:CheckBox ID="ckbIsValidTimeValue" runat="server" Text="IsValidTimeValue" TextAlign="Left" Checked="True" /><br />
                <asp:Label ID="Label10" runat="server" Font-Size="Small" Font-Italic="True" Text="�����Ƿ�����ʱ����֤"></asp:Label><br />
                <asp:CheckBox ID="ckbTimeAutoComplete" runat="server" Text="TimeAutoComplete" TextAlign="Left" Checked="True" /><br />
                <asp:Label ID="Label11" runat="server" Font-Size="small" Font-Italic="true" Text="�����Ƿ��Զ�����ʱ��"></asp:Label><br />
                TimeAutoCompleteValue
                <asp:DropDownList ID="ddlTimeAutoCompleteValue" runat="server">
                    <asp:ListItem>SystemTime</asp:ListItem>
                    <asp:ListItem>12:34</asp:ListItem>
                    <asp:ListItem>22:22:22</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label55" runat="server" Font-Size="small" Font-Italic="true" Text="�����ṩ�Զ������ʱ�䴮����������ȡϵͳʱ��"></asp:Label>
                <br />                    
                TimePromptCharacter
                <asp:TextBox ID="txbTimePromptCharacter" runat="server">^</asp:TextBox><br />
                <asp:Label ID="Label5" runat="server" Font-Size="Small" Font-Italic="True" Text="����ʱ�������ַ�"></asp:Label><br />
                TimeCurrentMessageError
                <asp:DropDownList ID="ddlTimeCurrentMessageError" runat="server">
                    <asp:ListItem>ʱ��ĸ�ʽ������Ҫ��</asp:ListItem>
                    <asp:ListItem>��ʱ�䲻���ϸ�ʽҪ��</asp:ListItem>
                    <asp:ListItem>�����ϸ�ʽҪ���ʱ��</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label8" runat="server" Font-Size="small" Font-Italic="true" Text="������֤ʱ����Ч�Ե���ʾ��Ϣ"></asp:Label><br />
                TimeTextStyle
                <asp:DropDownList ID="ddlTimeTextStyle" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>width: 100px; background-color:Aqua; text-align:left</asp:ListItem>
                    <asp:ListItem>width: 200px; font-size:larger; text-align:center</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label15" runat="server" Font-Size="Small" Font-Italic="True" Text="����ʱ��������Style��������TimeTextCss"></asp:Label><br />
                TimeTextCss
                <asp:DropDownList ID="ddlTimeTextCss" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>TimeTextCss_Demo1</asp:ListItem>
                    <asp:ListItem>TimeTextCss_Demo1</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label18" runat="server" Font-Size="Small" Font-Italic="True" Text="����ʱ��������Css"></asp:Label><br />
                OnTimeFocusCssClass
                <asp:DropDownList ID="ddlOnTimeFocusCssClass" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>OnTimeFocusCssClass_Demo1</asp:ListItem>
                    <asp:ListItem>OnTimeFocusCssClass_Demo2</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label17" runat="server" Font-Size="Small" Font-Italic="True" Text="����ʱ��õ�����ʱ�ı������ʽ"></asp:Label><br />
                OnTimeInvalidCssClass
                <asp:DropDownList ID="ddlOnTimeInvalidCssClass" runat="server">
                    <asp:ListItem>Default</asp:ListItem>
                    <asp:ListItem>OnTimeInvalidCssClass_Demo1</asp:ListItem>
                    <asp:ListItem>OnTimeInvalidCssClass_Demo2</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label16" runat="server" Font-Size="Small" Font-Italic="True" Text="����ʱ����֤��ͨ��ʱ�ı������ʽ"></asp:Label><br />
                <hr />
                <asp:CheckBox ID="ckbShowButton" runat="server" Text="ShowButton" TextAlign="Left" /><br />
                <asp:Label ID="Label6" runat="server" Font-Size="small" Font-Italic="true" Text="�����Ƿ��ṩ��ť��ѡ���Զ���ʱ���б�"></asp:Label><br />
                TimeMask
                <asp:DropDownList ID="ddlTimeMask" runat="server">
                    <asp:ListItem>99:99</asp:ListItem>
                    <asp:ListItem>99:99:99</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label7" runat="server" Font-Size="small" Font-Italic="true" Text="����ʱ���ʽ��,���������ĸ�ʽ"></asp:Label><br />
                DataSource<br />
                <asp:Label ID="Label33" runat="server" Text="����.Text"></asp:Label>
                <asp:TextBox ID="txbText" runat="server" Width="100px">MorningTime</asp:TextBox>
                <asp:Label ID="Label34" runat="server" Text=".Value"></asp:Label>
                <asp:TextBox ID="txbValue" runat="server" Width="100px">8:30:00</asp:TextBox>
                <asp:Button ID="btnSetDataSource" runat="server" Text="Set" OnClick="btnSetDataSource_Click" /><br />
                <asp:Label ID="Label23" runat="server" Font-Size="small" Font-Italic="true" Text="����ʱ��ؼ��󶨵�����Դ��ΪListitem����"></asp:Label><br />
            </td>
        </tr>
    </table>
    <hr />
    <table>
        <tr>
            <td style="width:400px">
                <asp:Button ID="btnRun" runat="server" Text="Set Properties" OnClick="btnRun_Click" />
                <br />
                <br />
                <cc1:DeluxeDateTime ID="dateTime" runat="server" TimeMask="99:99" IsOnlyCurrentMonth="True" ShowButton="False" DatePromptCharacter="123"/>
                <br />
                <br />
                <asp:Button ID="btnGetValue" runat="server" Text="Get Value" OnClick="btnGetValue_Click" />
                <asp:TextBox ID="txbGetValue" runat="server" Enabled="False"></asp:TextBox><br />
                <asp:Label ID="lblGetValue" runat="server" Font-Size="small" Font-Italic="true" Text="����ȡ������ʱ��ؼ���Valueֵ����DateTime���ͣ��˴����÷�ʽ��"></asp:Label>
                <asp:Label ID="Label21" runat="server" Text="dateTime.Value.ToString()" Font-Bold="true"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btnSetValue" runat="server" Text="Set Value" OnClick="btnSetValue_Click"/>
                <asp:TextBox ID="txbSetValue" runat="server"></asp:TextBox><br />
                 <asp:Label ID="Label22" runat="server" Font-Size="small" Font-Italic="true" Text="������������ʱ��ؼ���Valueֵ����������Ч��Datetime���ͣ��˴����÷�ʽ��"></asp:Label>
                <asp:Label ID="Label24" runat="server" Text="System.DateTime.Parse(txbSetValue.Text)" Font-Bold="true"></asp:Label></td>
            <td>
                <textarea id="dateTimeHtmlShow" runat="server" style="width: 600px; height: 300px;" enableviewstate="true"></textarea></td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
