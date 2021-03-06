﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using MCS.Library.Core;
using MCS.Web.Library.Script;
using MCS.Web.Library.Resources;
using MCS.Library.SOA.DataObjects;
using System.ComponentModel;
using MCS.Web.Library;
using MCS.Library.Validation;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using MCS.Library.Data.DataObjects;
using System.Runtime.Serialization;

[assembly: WebResource("MCS.Web.WebControls.PropertyEditorBase.invalid_line.gif", "image/gif")]

namespace MCS.Web.WebControls
{
    [PersistChildren(false)]
    [ParseChildren(true)]
    [RequiredScript(typeof(ControlBaseScript), 1)]
    [RequiredScript(typeof(ClientMsgResources), 2)]
    [RequiredScript(typeof(ClientPropertyEditorControlBaseResources), 3)]
    public abstract class PropertyEditorControlBase : ScriptControlBase
    {
        [Serializable]
        public class CheckBoxStateCollection : EditableKeyedDataObjectCollectionBase<string, KeyValuePair<string, bool>>
        {
            protected override string GetKeyForItem(KeyValuePair<string, bool> item)
            {
                return item.Key;
            }
        }

        protected PropertyValueCollection _Properties = new PropertyValueCollection();
        protected CheckBoxStateCollection _checkBoxes = new CheckBoxStateCollection();

        public PropertyEditorControlBase()
            : base(true, HtmlTextWriterTag.Div)
        {
            PropertyEditorHelper.AttachToPage(WebUtility.GetCurrentPage());

            JSONSerializerExecute.RegisterConverter(typeof(PropertyValueConverter));
            JSONSerializerExecute.RegisterConverter(typeof(EnumItemPropertyDescriptionConverter));
            JSONSerializerExecute.RegisterConverter(typeof(PropertyValidatorParameterDescriptorConverter));
            JSONSerializerExecute.RegisterConverter(typeof(PropertyValidatorDescriptorConverter));
            JSONSerializerExecute.RegisterConverter(typeof(EditorParamsDefineConverter));
            JSONSerializerExecute.RegisterConverter(typeof(ControlPropertyDefineConverter));
            //JSONSerializerExecute.RegisterConverter(typeof(ClientVdtDataConverter));
        }

        [PersistenceMode(PersistenceMode.InnerProperty), Description("数据源属性定义")]
        [MergableProperty(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DefaultValue((string)null)]
        [Browsable(false)]
        [ScriptControlProperty, ClientPropertyName("properties")]
        public PropertyValueCollection Properties
        {
            get
            {
                return this._Properties;
            }
        }

        [ScriptControlProperty]
        [ClientPropertyName("autoSaveClientState")]
        [DefaultValue(true)]
        public bool AutoSaveClientState
        {
            get { return GetPropertyValue("AutoSaveClientState", true); }
            set { SetPropertyValue("AutoSaveClientState", value); }
        }


        [ScriptControlProperty]
        [ClientPropertyName("invalidLineImage")]
        [Browsable(false)]
        public string InvalidLineImage
        {
            get
            {
                return this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "MCS.Web.WebControls.PropertyEditorBase.invalid_line.gif");
            }
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        [DefaultValue(false)]
        [ScriptControlProperty]
        [ClientPropertyName("readOnly")]
        [Category("Appearance")]
        [Description("是否只读")]
        public override bool ReadOnly
        {
            get { return GetPropertyValue<bool>("ReadOnly", false); }
            set { SetPropertyValue<bool>("ReadOnly", value); }
        }

        [DefaultValue(false)]
        [ScriptControlProperty]
        [ClientPropertyName("showCheckBoxes")]
        [Category("Appearance")]
        [Description("是否为每项属性提供复选框(仅客户端可用)")]
        public bool ShowCheckBoxes
        {
            get { return GetPropertyValue<bool>("ShowCheckBoxes", false); }
            set { SetPropertyValue<bool>("ShowCheckBoxes", value); }

        }

        #region "event"
        [DefaultValue("")]
        [ScriptControlEvent]
        [ClientPropertyName("editorValidating")]
        public string OnClientEditorValidating
        {
            get
            {
                return GetPropertyValue("OnClientEditorValidating", string.Empty);
            }
            set
            {
                SetPropertyValue("OnClientEditorValidating", value);
            }
        }

        [DefaultValue("")]
        [ScriptControlEvent]
        [ClientPropertyName("editorValidate")]
        public string OnClientEditorValidate
        {
            get
            {
                return GetPropertyValue("OnClientEditorValidate", string.Empty);
            }
            set
            {
                SetPropertyValue("OnClientEditorValidate", value);
            }
        }

        [DefaultValue("")]
        [ScriptControlEvent]
        [ClientPropertyName("editorValidated")]
        public string OnClientEditorValidated
        {
            get
            {
                return GetPropertyValue("OnClientEditorValidated", string.Empty);
            }
            set
            {
                SetPropertyValue("OnClientEditorValidated", value);
            }
        }

        [DefaultValue("")]
        [ScriptControlEvent]
        [ClientPropertyName("enterEditor")]
        [Bindable(true), Category("ClientEventsHandler"), Description("客户端进入某个编辑器的事件")]
        public string OnClientEnterEditor
        {
            get
            {
                return GetPropertyValue("OnClientEnterEditor", string.Empty);
            }
            set
            {
                SetPropertyValue("OnClientEnterEditor", value);
            }
        }

        [DefaultValue("")]
        [ScriptControlEvent]
        [ClientPropertyName("clickEditor")]
        [Bindable(true), Category("ClientEventsHandler"), Description("客户端点击编辑器内按钮")]
        public string OnClientClickEditor
        {
            get
            {
                return GetPropertyValue("OnClientClickEditor", string.Empty);
            }
            set
            {
                SetPropertyValue("OnClientClickEditor", value);
            }
        }

        [DefaultValue("")]
        [ScriptControlEvent]
        [ClientPropertyName("bindEditorDropdownList")]
        [Bindable(true), Category("ClientEventsHandler"), Description("客户端点击编辑器内按钮")]
        public string OnBindEditorDropdownList
        {
            get
            {
                return GetPropertyValue("OnBindEditorDropdownList", string.Empty);
            }
            set
            {
                SetPropertyValue("OnBindEditorDropdownList", value);
            }
        }
        #endregion

        /// <summary>
        /// 预定义的枚举类型
        /// </summary>
        [Browsable(false)]
        public List<string> PredefinedEnumTypes
        {
            get
            {
                List<string> result = (List<string>)ViewState["PredefinedEnumTypes"];

                if (result == null)
                {
                    result = new List<string>();
                    ViewState["PredefinedEnumTypes"] = result;
                }

                return result;
            }
        }

        #region "override Method"

        protected override void OnInit(EventArgs e)
        {
            this.Page.LoadComplete += new EventHandler(Page_LoadComplete);
            this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
            PropertyEditorHelper.AttachToPage(this.Page);
            base.OnInit(e);
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            foreach (PropertyValue prop in this.Properties)
            {
                if (prop.Definition.EditorKey.IsNotEmpty())
                {
                    PropertyEditorHelper.AllEditors.ContainsKey(prop.Definition.EditorKey).FalseThrow("不能找到预定义好的属性编辑器{0}", prop.Definition.EditorKey);
                    PropertyEditorBase propEditor = PropertyEditorHelper.AllEditors[prop.Definition.EditorKey];
                    propEditor.SetControlsPropertyDefineFromEditorParams(prop.Definition);
                }
            }
        }

        protected override void OnPagePreRenderComplete(object sender, EventArgs e)
        {
            base.OnPagePreRenderComplete(sender, e);
        }

        protected override void OnPagePreLoad(object sender, EventArgs e)
        {
            base.OnPagePreLoad(sender, e);
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            foreach (PropertyValue prop in this.Properties)
            {
                foreach (PropertyValidatorDescriptor propValidator in prop.Definition.Validators)
                {
                    Validator vali = propValidator.GetValidator();
                    if (vali is IClientValidatable)
                    {
                        ClientVdtAttribute cvArt = propValidator.ToClientVdtAttribute();

                        if (string.IsNullOrEmpty(cvArt.ClientValidateMethodName) == false)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), cvArt.ClientValidateMethodName, ((IClientValidatable)vali).GetClientValidateScript(), true);
                        }
                    }
                }
            }
        }

        protected void RegCheckScript()
        {
            CustomValidator vcontrol = new CustomValidator();
            vcontrol.ClientValidationFunction = "$HGRootNS.PropertyEditorControlBase.ValidateProperties";
            this.Controls.Add(vcontrol);
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //}

        protected override void CreateChildControls()
        {
            this.RegCheckScript();
            base.CreateChildControls();
        }

        protected override string SaveClientState()
        {
            //JSONSerializerExecute.RegisterConverter(typeof(EditorParamsDefineConverter));
            List<EnumTypePropertyDescription> enumDefinitions = CollectEnumDescriptions();

            return JSONSerializerExecute.Serialize(enumDefinitions);
        }

        private List<EnumTypePropertyDescription> CollectEnumDescriptions()
        {
            Dictionary<string, EnumTypePropertyDescription> enumDespDict = new Dictionary<string, EnumTypePropertyDescription>();

            foreach (string enumParams in PredefinedEnumTypes)
            {
                EnumTypePropertyDescription etpd = EnumTypePropertyDescription.FromEnumTypeName(enumParams);

                if (etpd != null)
                    enumDespDict.Add(etpd.EnumTypeName, etpd);
            }

            foreach (PropertyValue pv in this.Properties)
            {
                if (pv.Definition.DataType == PropertyDataType.Enum && pv.Definition.EditorParams.IsNotEmpty())
                {
                    string editorParamName = pv.Definition.EditorParams;
                    if (Regex.IsMatch(pv.Definition.EditorParams, @"\{[^{}]*}") == true)
                    {
                        try
                        {
                            EditorParamsDefine editorParams = JSONSerializerExecute.Deserialize<EditorParamsDefine>(pv.Definition.EditorParams);
                            editorParamName = editorParams.EnumTypeName;
                            //将配置文件里的数据源转换成下拉列表数据源
                            if (editorParams.ContainsKey("dropDownDataSourceID") == true)
                            {
                                string sourceID = editorParams["dropDownDataSourceID"];
                                if (PropertyEditorHelper.AllDropdownPropertyDataSource.ContainsKey(sourceID) == true)
                                {
                                    EnumTypePropertyDescription etpd = PropertyEditorHelper.GenerateEnumTypePropertyDescription(PropertyEditorHelper.AllDropdownPropertyDataSource[sourceID]);
                                    if (etpd != null)
                                        enumDespDict.Add(etpd.EnumTypeName, etpd);
                                }
                            }
                        }
                        catch (Exception)
                        {   //这里吃掉异常，主要原因是兼容老已上线的流程配置文件
                            editorParamName = pv.Definition.EditorParams;
                        }
                    }

                    if (editorParamName.IsNotEmpty())
                        if (enumDespDict.ContainsKey(editorParamName) == false)
                        {
                            EnumTypePropertyDescription etpd = EnumTypePropertyDescription.FromEnumTypeName(editorParamName);

                            if (etpd != null)
                                enumDespDict.Add(etpd.EnumTypeName, etpd);
                        }
                }
            }

            List<EnumTypePropertyDescription> result = new List<EnumTypePropertyDescription>();

            foreach (KeyValuePair<string, EnumTypePropertyDescription> kp in enumDespDict)
            {
                result.Add(kp.Value);
            }

            return result;
        }
        #endregion
    }
}
