using MCS.Library.SOA.DataObjects;
using MCS.Web.Library.Script;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;

#region 嵌入资源
[assembly: WebResource("MCS.Dynamics.Web.PropertyEditors.ValidatorSelectorControl.js", "application/x-javascript")]
[assembly: WebResource("MCS.Dynamics.Web.PropertyEditors.ou.gif", "image/gif")]
#endregion

namespace MCS.Dynamics.Web
{
    /// <summary>
    /// 动态实体属性定义验证器选择控件
    /// </summary>
    [RequiredScript(typeof(ControlBaseScript))]
    [ToolboxData("<{0}:ValidatorSelectorControl runat=server></{0}:ValidatorSelectorControl>")]
    [ClientScriptResource("MCS.Dynamics.Web.ValidatorSelectorControl", "MCS.Dynamics.Web.PropertyEditors.ValidatorSelectorControl.js")]
    public class ValidatorSelectorControl : ScriptControlBase
    {
        public ValidatorSelectorControl()
            : base(true, HtmlTextWriterTag.Div)
        { }
        /// <summary>
        /// OnPreRender
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (Page.IsCallback == false)
            {
              
            }

            base.OnPreRender(e);
        }
        /// <summary>
        /// 选择验证器的图标
        /// </summary>
        [ScriptControlProperty]//设置此属性要输出到客户端
        [ClientPropertyName("selecotrImg")]//对应的客户端属性
        public string SelecotrImg
        {
            get
            {
                return GetPropertyValue<string>("SelecotrImg",
                    Page.ClientScript.GetWebResourceUrl(typeof(ValidatorSelectorControl),
                        "MCS.Dynamics.Web.PropertyEditors.ou.gif"));
            }
            set { SetPropertyValue<string>("SelecotrImg", value); }
        }
        /// <summary>
        /// 选中的Validator的JSON数据
        /// </summary>
        [ScriptControlProperty]
        [ClientPropertyName("selectedValidatorsJsonValue")]
        public string SelectedValidatorsJsonValue
        {
            get
            {
                return GetPropertyValue<string>("SelectedValidatorsJsonValue",string.Empty);
            }
            set { SetPropertyValue<string>("SelectedValidatorsJsonValue", value); }
        }

        /// <summary>
        /// 选中的Validator的在标签中显示的内容
        /// </summary>
        [ScriptControlProperty]
        [ClientPropertyName("displayText")]
        public string DisplayText
        {
            get
            {
                return GetPropertyValue<string>("DisplayText", string.Empty);
            }
            set { SetPropertyValue<string>("DisplayText", value); }
        }

        /// <summary>
        /// 弹出选择Validator页面的URL
        /// </summary>
        [ScriptControlProperty]
        [ClientPropertyName("popupSelectorURL")]
        public string PopupSelectorURL
        {
            get
            {
                return GetPropertyValue<string>("PopupSelectorURL", "/PropertyEditors/ValidatorSelector.aspx");
            }
        }
        /// <summary>
        /// 选中的Validator的属性定义集合
        /// </summary>
        private PropertyDefineCollection selectedValidatorsPropertyCollection = null;
        public PropertyDefineCollection SelectedValidatorsPropertyCollection
        {
            get
            {
                if (this.selectedValidatorsPropertyCollection == null)
                {
                    this.selectedValidatorsPropertyCollection = new PropertyDefineCollection();
                }
                return this.selectedValidatorsPropertyCollection;
            }
            set
            {
                this.selectedValidatorsPropertyCollection = value;
            }
        }
    }
}