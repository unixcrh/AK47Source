using MCS.Library.Core;
using MCS.Library.SOA.DataObjects;
using MCS.Web.Library.Script;
using MCS.Web.WebControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCS.Dynamics.Web.PropertyEditors
{
    public partial class ValidatorSelector : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            PropertyEditorRegister();
            ValidatorPropertisRegister();
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack == false)
            {
                BindValidators();
            }
        }

        private void PropertyEditorRegister()
        {
            PropertyEditorHelper.RegisterEditor(new StandardPropertyEditor());
            //PropertyEditorHelper.RegisterEditor(new BooleanPropertyEditor());
            //PropertyEditorHelper.RegisterEditor(new EnumPropertyEditor());
            //PropertyEditorHelper.RegisterEditor(new ObjectPropertyEditor());
            //PropertyEditorHelper.RegisterEditor(new DatePropertyEditor());
            //PropertyEditorHelper.RegisterEditor(new DateTimePropertyEditor());
            //PropertyEditorHelper.RegisterEditor(new ImageUploaderPropertyEditor());
            //PropertyEditorHelper.RegisterEditor(new ImageUploaderPropertyEditorForGrid());
            //PropertyEditorHelper.RegisterEditor(new OUUserInputPropertyEditor());
            //PropertyEditorHelper.RegisterEditor(new RoleGraphPropertyEditor());
        }

        public void ValidatorPropertisRegister()
        {
            PropertyGroupSettings settings = PropertyGroupSettings.GetConfig();
            PropertyGroupConfigurationElementCollection groups = settings.Groups;
            List<ValidatorDefine> validatorDefineList = new List<ValidatorDefine>();
            foreach (PropertyGroupConfigurationElement element in groups)
            {
                PropertyValueCollection pvc = new PropertyValueCollection();
                PropertyDefineCollection pdc = new PropertyDefineCollection();
                pdc.LoadPropertiesFromConfiguration(element);
                pvc.InitFromPropertyDefineCollection(pdc);
                validatorDefineList.Add(new ValidatorDefine { ValidatorName = element.Name, PropertyValues = pvc });
            }
            string script = string.Format("var arrValidatorDefine={0};", JSONSerializerExecute.Serialize(validatorDefineList));

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "RegisterValidatorDefineScript", script, true);
        }

        public void BindValidators()
        {
            ValidatorTypeConfigurationElementCollection validators = ValidatorSettings.GetConfig().Validators;
            this.dllValidator.DataSource = validators;
            this.dllValidator.DataTextField = "Description";
            this.dllValidator.DataValueField = "Name";
            this.dllValidator.DataBind();
            this.dllValidator.Items.Insert(0,new ListItem("请选择",""));
            this.dllValidator.SelectedIndex = 0;
        }

        protected void bT_Click(object sender, EventArgs e)
        {
            var imgProStr = propertyGrid.Properties.GetValue("Image1", "");
            if (imgProStr != "")
            {
                ImageProperty imgPro = JSONSerializerExecute.Deserialize<ImageProperty>(imgProStr);
                if (imgPro.Changed)
                {
                    ImagePropertyAdapter.Instance.UpdateWithContent(imgPro);
                    propertyGrid.Properties.SetValue("Image1", JSONSerializerExecute.Serialize(imgPro));
                }
            }

            imgProStr = propertyGrid.Properties.GetValue("Image2", "");
            if (imgProStr != "")
            {
                ImageProperty imgPro = JSONSerializerExecute.Deserialize<ImageProperty>(imgProStr);
                if (imgPro.Changed)
                {
                    ImagePropertyAdapter.Instance.UpdateWithContent(imgPro);
                    propertyGrid.Properties.SetValue("Image2", JSONSerializerExecute.Serialize(imgPro));
                }
            }
        }

    }
    [Serializable]
    public class ValidatorDefine
    {
        public string ValidatorName { get; set; }
        public PropertyValueCollection PropertyValues { get; set; }
    }
}