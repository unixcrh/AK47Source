<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ValidatorSelector.aspx.cs" Inherits="MCS.Dynamics.Web.PropertyEditors.ValidatorSelector" %>

<%@ Register Assembly="MCS.Web.WebControls" Namespace="MCS.Web.WebControls" TagPrefix="MCS" %>
<%@ Register Assembly="MCS.Library.SOA.Web.WebControls" Namespace="MCS.Web.WebControls" TagPrefix="SOA" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Css/basePage.css" rel="stylesheet" />
    <script src="../scripts/jquery-1.9.0.js"></script>
    <script src="../scripts/jquery.tmpl.js"></script>
    <script src="../scripts/json2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
        <div class="dialogHeader">选择验证器</div>
        <div class="dialogContent">
            <table class="dataList">
                <tr>
                    <td>已选择的验证器</td>
                    <td>验证器属性设置</td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <table>
                            <thead>
                                <tr>
                                    <th style="width: 30px; text-align: center;">序号</th>
                                    <th style="width: 300px; text-align: center;">验证器</th>
                                    <th style="width: 100px; text-align: center;">操作</th>
                                </tr>
                            </thead>
                            <tbody id="gridValidator">
                            </tbody>
                        </table>
                    </td>
                    <td style="vertical-align: top; width: 300px;">
                        <soa:HBDropDownList ID="dllValidator" runat="server" ClientIDMode="Static" Width="100%"></soa:HBDropDownList>
                        <soa:PropertyGrid runat="server" ID="propertyGrid" Width="100%"  Height="300px" OnClientEditorValidated="onClickValidated"
                            OnClientClickEditor="onClickEditor" DisplayOrder="ByCategory" ReadOnly="false" BackColor="White" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <input type="button" id="btnOK" value="保存选中" onclick="okClick()" />
                    </td>
                    <td style="text-align: center;">
                        <input type="button" id="btnSave" value="保存设置" onclick="saveClick()" />
                    </td>
                </tr>
            </table>
        </div>

        <script id="_tempValidator" type="text/x-jquery-tmpl">
            <tr>
                <td data-field="vrindex" style="text-align: center;"></td>
                <td style="text-align: left;">${Description}</td>
                <td style="text-align: center;">
                    <a href="javascript:void(0);" onclick="editValidatorProperty(this)">编辑</a> | 
                    <a href="javascript:void(0);" onclick="return deleteValidator(this);">删除</a>
                    <input type="hidden" name="hidValidatorJsonData" value='${$item.getJsonData()}' />
                </td>
            </tr>
        </script>
    </form>
</body>
</html>
<script type="text/javascript">
    function collectPropertiesValue()
    {
        var strB = new Sys.StringBuilder();
        var properties = $find("propertyGrid").get_properties();

        for (var i = 0; i < properties.length; i++) {
            if (strB.isEmpty() == false)
                strB.append("\n");

            var prop = properties[i];
            strB.append(prop.name + ": " + prop.value ? prop.value : 'no value');
        }

        return strB.toString();
    }

    function onShowPropertiesValueClick()
    {
        $get("propertyResult").innerText = collectPropertiesValue();
    }

    function onEnterEditor(sender, e)
    {
        $get("propertyResult").innerText += "\nEnter Property: " + e.propertyValue.value;
    }

    function onClickEditor(sender, e)
    {
        var activeEditor = sender.get_activeEditor();
    }

    function OnBindEditorDropdownList(sender, e)
    {
        var enumTypes = jQuery.evalJSON(jQuery('#enumTypeStore').val());
        var result = enumTypes[e.property.editorParams];

        if (result)
            e.enumDesc = result;
    }

    function onClickValidated(sender, e)
    {

    }

    function onPopertyClientShow(sender, e)
    {
        var activeEditor = sender;
    }

    $(document).ready(function ()
    {
        $("#dllValidator").bind("change", initPropertyGrid);
        //var validators = window.dialogArguments;
        //loadValidators(validators);
        initPropertyGrid();
    });

    //加载Validator列表
    function loadValidators(validators)
    {
        if (validators != null && validators != undefined) {
            $("#_tempValidator").tmpl(validators,
            {
                getJsonData: function ()
                {
                    return JSON.stringify(this.data);
                }
            }).appendTo('#gridValidator');
            setSequenceNumber();
        }
    }
    //删除Validator
    function deleteValidator(obj)
    {
        if (confirm("确定要删除该条数据吗？")) {
            $(obj).parent().parent().remove();
            setSequenceNumber();
            return true;
        }
        return false;
    }
    //设置序号
    function setSequenceNumber()
    {
        $("td[data-field='vrindex']").each(function (i, td)
        {
            td.innerText = i + 1;
        });
    }
    //编辑Validator属性设置
    function editValidatorProperty(obj)
    {
        var json = $(obj).nextAll("input[name='hidValidatorJsonData']").val();
        if (json != "")
        {
            var propertyGrid = $find("propertyGrid");
            var v = eval("(" + json + ")");
            $("#dllValidator option:selected").removeAttr("selected");
            $("#dllValidator option[value='" + v.ValidatorName + "']").attr("selected", true);
            propertyGrid.set_properties(v.PropertyValues);
            propertyGrid.dataBind();
        }
    }

    //初始化PropertyGrid
    function initPropertyGrid()
    {
        var v = $("#dllValidator").val();
        var propertyGrid = $find("propertyGrid");
        if (v == "")
        {
            if (propertyGrid != null)
            {
                propertyGrid.set_properties([]);
                propertyGrid.dataBind();
            }
        }
        else
        {
            if (propertyGrid != null)
            {
                var validator = v + "Properties";
                for (var i = 0; i < arrValidatorDefine.length; i++)
                {
                    if (arrValidatorDefine[i].ValidatorName == validator)
                    {
                        propertyGrid.set_properties(arrValidatorDefine[i].PropertyValues);
                        propertyGrid.dataBind();
                        break;
                    }
                }
            }
        }

    }

    //保存PropertyGrid内容
    function saveClick()
    {
        var name = $("#dllValidator").val();
        if (name != "")
        {
            var validator =
                   {
                       ValidatorName: name,
                       Description: $("#dllValidator option:selected").text(),
                       PropertyValues: $find("propertyGrid").get_properties()
                   };
            var isExsist = false;
            $("input[name='hidValidatorJsonData']").each(function (i, o)
            {
                $o = $(o);
                if ($o.val() != "") {
                    var v = eval("(" + $o.val() + ")");
                    if (v.ValidatorName != undefined && v.ValidatorName == validator.ValidatorName) {
                        $o.val(JSON.stringify(validator));
                        isExsist = true;
                    }
                }
            });

            if (!isExsist) {
                var validators = [];
                validators.push(validator);
                loadValidators(validators);
                initPropertyGrid();
            }
        }
       
    }
    //保存选中项，并关闭页面
    function okClick()
    {
        var validators = [];
        $("input[name='hidValidatorJsonData']").each(function (i, o)
        {
            $o = $(o);
            if ($o.val() != "")
            {
                var v = eval("(" + $o.val() + ")");
                validators.push(v);
            }
        });

        window.returnValue = validators;
        window.close();
    }
 
</script>
