<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default2" CodeBehind="Default2.aspx.cs" %>

<%@ Register Assembly="MCS.Web.WebControls" Namespace="MCS.Web.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="MCS.Web.WebControls" Namespace="MCS.Web.WebControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>�ޱ���ҳ</title>
</head>
<body>
	<form id="serverForm" runat="server">
	<div>
		<asp:ScriptManager ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<asp:TextBox ID="TextBox1" runat="server" onSelectedItem="alert(this.get_items()[this.get_selectIndex()].Value);alert(this.get_items()[this.get_selectIndex()].Text);"
			BorderStyle="None"></asp:TextBox>&nbsp;
		<cc1:GenericInputExtender ID="GenericInputExtender1" runat="server" TargetControlID="TextBox1" />
		<input type=button value="Show Text..." onclick="alert($get('TextBox1').value)" />
		<br />
		��Ŀ�ı���<asp:TextBox ID="txtItemText" runat="server"></asp:TextBox>
		<asp:CheckBox ID="chkSelected" runat="server" Text="Ĭ��ѡ��" /><br />
		��Ŀֵ��<asp:TextBox ID="txtItemValue" runat="server"></asp:TextBox>
		<asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="���" Width="54px" />
		<asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="�����Ŀ" /><br />
		<br />
		�߿���ɫ��<asp:TextBox ID="txtBorderColor" runat="server"></asp:TextBox>
		<asp:Button ID="btnSetBorderColor" runat="server" OnClick="btnSetBorderColor_Click"
			Text="����" Width="54px" /><br />
		��Ŀ������ɫ��<asp:TextBox ID="txtFontColor" runat="server"></asp:TextBox>
		<asp:Button ID="btnSetFontColor" runat="server" OnClick="btnSetFontColor_Click" Text="����"
			Width="54px" /><br />
		<br />
		����ƶ�����Ŀ������ɫ��<asp:TextBox ID="txtSelectItemFontColor" runat="server"></asp:TextBox>
		<asp:Button ID="btnSelectItemFontColor" runat="server" OnClick="btnSelectItemFontColor_Click"
			Text="����" Width="54px" /><br />
		����ƶ�����Ŀ������ɫ��<asp:TextBox ID="txtHoveBGColor" runat="server"></asp:TextBox>
		<asp:Button ID="btnHoveBGColor" runat="server" OnClick="btnHoveBGColor_Click" Text="����"
			Width="54px" /><br />
		<br />
		������ͷ���򱳾�ɫ��<asp:TextBox ID="txtBgColor" runat="server"></asp:TextBox>
		<asp:Button ID="btnSetBgColor" runat="server" OnClick="btnSetBgColor_Click" Text="����"
			Width="54px" /><br />
	</div>
	</form>
</body>
</html>
