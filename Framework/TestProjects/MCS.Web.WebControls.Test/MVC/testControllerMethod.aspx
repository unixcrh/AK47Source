﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testControllerMethod.aspx.cs"
	Inherits="MCS.Web.WebControls.Test.MVC.testControllerMethod" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Controller Method Test</title>
	<script type="text/javascript">
		function onChangeMethod(urlParams) {
			var url = urlParams;

			frameContainer.innerHTML = "<iframe id='controllerMethodContainer' src='controllerMethodContainer.ashx" + url +
				"' style='width: 100%;height: 100%'></iframe>";
		}
	</script>
</head>
<body>
	<form id="serverForm" runat="server">
	<div>
		<input type="button" value="Default Method" onclick="onChangeMethod('')" />
		<input type="button" value="One Param Method" onclick="onChangeMethod('?name=ShenZheng')" />
		<input type="button" value="Two Params Method" onclick="onChangeMethod('?name=ShenZheng&isMale=true')" />
		<input type="button" value="Ignore Params Method" onclick="onChangeMethod('?name=ShenZheng&activityID=activity')" />
		<input type="button" value="Class Params Method" onclick="onChangeMethod('?name=ShenZheng&age=39')" />
	</div>
	<div id="frameContainer" style="width: 520px; height: 480px">
		<iframe id="controllerMethodContainer" src="controllerMethodContainer.ashx" style="width: 100%;
			height: 100%"></iframe>
	</div>
	</form>
</body>
</html>
