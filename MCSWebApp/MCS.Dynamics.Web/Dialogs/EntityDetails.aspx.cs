using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MCS.Library.Core;
using MCS.Library.OGUPermission;
using MCS.Library.Principal;
using MCS.Library.SOA.DataObjects;
using MCS.Library.SOA.DataObjects.Dynamics;
using MCS.Library.SOA.DataObjects.Dynamics.Actions;
using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Converters;
using MCS.Library.SOA.DataObjects.Dynamics.Enums;
using MCS.Library.SOA.DataObjects.Dynamics.Executors;
using MCS.Library.SOA.DataObjects.Dynamics.Objects;
using MCS.Library.SOA.DataObjects.Schemas.SchemaProperties;
using MCS.Web.Library;
using MCS.Web.Library.Script;
using MCS.Web.WebControls;
using MCS.Library.SOA.DataObjects.Dynamics.Schemas;
using MCS.Web.Library.MVC;

namespace MCS.Dynamics.Web.Dialogs
{
    [SceneUsage("~/App_Data/PropertyEditScene.xml", "PropertyEdit")]
    public partial class EntityDetails : System.Web.UI.Page, ITimeSceneDescriptor, INormalSceneDescriptor
    {
        private bool sceneDirty = true;
        private bool enabled = false;
        private PropertyEditorSceneAdapter sceneAdapter = null;

        string ITimeSceneDescriptor.NormalSceneName
        {
            get { return this.EditEnabled ? "Normal" : "ReadOnly"; }
        }

        string ITimeSceneDescriptor.ReadOnlySceneName
        {
            get { return "ReadOnly"; }
        }

        protected bool EditEnabled
        {
            get
            {
                if (this.sceneDirty)
                {
                    this.enabled = TimePointContext.Current.UseCurrentTime;

                    if (this.enabled && Util.SuperVisiorMode == false && this.sceneAdapter != null)
                    {
                        this.enabled = this.sceneAdapter.IsEditable();
                    }

                    this.sceneDirty = false;
                }

                return this.enabled;
            }
        }

        private DESchemaObjectBase Data
        {
            get;
            set;
        }

        protected override void OnPreInit(EventArgs e)
        {
            //注册序列化器
            //JSONSerializerExecute.RegisterConverter(typeof(DynamicEntityFieldConverter));
            // JSONSerializerExecute.RegisterConverter(typeof(SchemaObjectSimpleConverter));
            base.OnPreInit(e);
        }

        //操作类型
        private SCObjectOperationMode OperationMode
        {
            get
            {
                return WebControlUtility.GetViewStateValue(this.ViewState, "OperationMode", SCObjectOperationMode.Add);
            }
            set
            {
                WebControlUtility.SetViewStateValue(this.ViewState, "OperationMode", value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!TimePointContext.Current.UseCurrentTime)
            {
                this.grid.ReadOnly = true;
            }
            //给隐藏域赋值
            Dictionary<string, string> enumValueKey = new Dictionary<string, string>();
            Dictionary<string, string> enumKeyValue = new Dictionary<string, string>();

            Type fileTypes = typeof(FieldTypeEnum);
            foreach (string s in Enum.GetNames(fileTypes))
            {
                FieldTypeEnum myEnum = (FieldTypeEnum)Enum.Parse(typeof(FieldTypeEnum), s);
                enumValueKey.Add(((int)myEnum).ToString(), s);
                enumKeyValue.Add(s, ((int)myEnum).ToString());
            }

            HF_EnumValueKey.Value = JSONSerializerExecute.Serialize(enumValueKey, typeof(object));
            HF_EnumKeyValue.Value = JSONSerializerExecute.Serialize(enumKeyValue, typeof(object));
            if (this.IsPostBack == false && this.IsCallback == false)
                ControllerHelper.ExecuteMethodByRequest(this);

            this.PropertyEditorRegister();

            WebUtility.RequiredScript(typeof(ClientGrid));
            this.bindingControl.Data = InitEntity(Request.QueryString["ID"], Request.QueryString["CategoryID"]);

            if (!IsPostBack)
            {
                //绑定字段类型
                this.ddl_FieldType.BindData(EnumItemDescriptionAttribute.GetDescriptionList(typeof(FieldTypeEnum)), "Name", "Description");
            }


        }

        //初始化实体
        private DynamicEntity InitEntity(string entityId, string categoryId)
        {
            DynamicEntity result;

            if (entityId.IsNotEmpty())
            {

                result = (DynamicEntity)DESchemaObjectAdapter.Instance.Load(entityId);


                OperationMode = SCObjectOperationMode.Update;
            }
            else
            {
                categoryId.CheckStringIsNullOrEmpty("[类别编码]");

                result = (DynamicEntity)SchemaExtensions.CreateObject(DEStandardObjectSchemaType.DynamicEntity.ToString());
                result.CategoryID = categoryId;
                OperationMode = SCObjectOperationMode.Add;
                result.Fields = new DynamicEntityFieldCollection();

            }

            return result;
        }

        //保存
        protected void btn_Save_Click(object sender, EventArgs e)
        {

            StringBuilder error = new StringBuilder();

            if (!Util.CheckOperationSafe())
                return;

            //取值
            this.bindingControl.CollectData();
            DynamicEntity entity = (DynamicEntity)this.bindingControl.Data;

            PropertyValueCollection pvc = JSONSerializerExecute.Deserialize<PropertyValueCollection>(Request.Form.GetValue("properties", string.Empty));

            entity.Properties.FromPropertyValues(pvc);

            //检查重复项
            entity.Fields.GroupBy(p => p.Name).ForEach(p => { if (p.Count() > 1)error.Append(p.Key + "有重复项!\r\n"); });
            //entity.SortNo = 0;
            //entity.Fields.ForEach(p => p.SortNo = 0);

            bool needCheckExist = false;
            #region
            if (DESchemaObjectAdapter.Instance.Exist(entity.ID, DateTime.Now.SimulateTime()))
            {
                //更新实体
                DynamicEntity old = DESchemaObjectAdapter.Instance.Load(entity.ID, DateTime.Now.SimulateTime()) as DynamicEntity;
                if (!old.Name.Equals(entity.Name))
                    needCheckExist = true;
            }
            else
                needCheckExist = true;


            if (needCheckExist)
            {
                //生成CodeName
                entity.BuidCodeName();

                if (DEDynamicEntityAdapter.Instance.ExistByCodeName(entity.CodeName, DateTime.Now.SimulateTime()))
                    error.AppendFormat("已存在名称为[{0}]的实体!\r\n", entity.Name);
            }

            (error.Length > 0).TrueThrow(error.ToString());
            #endregion
            //入库
            DEObjectOperations.InstanceWithPermissions.DoOperation(this.OperationMode, entity, null);
            entity.ClearCacheData();
            HttpContext.Current.Response.Write("<script>window.returnValue=true;window.close()</script>");
        }

        private void InitPropertyForm(DESchemaObjectBase data, bool readOnly)
        {
            this.propertyForm.ReadOnly = readOnly;

            data.Properties.ForEach(p =>
            {
                if (Request.QueryString[p.Definition.Name].IsNotEmpty())
                {
                    p.StringValue = Request.QueryString[p.Definition.Name].Trim();
                }
            });

            this.propertyForm.Properties.CopyFrom(data.Properties.ToPropertyValues());


            var layouts = new PropertyLayoutSectionCollection();
            layouts.LoadLayoutSectionFromConfiguration("DefalutLayout");

            this.propertyForm.Layouts.InitFromLayoutSectionCollection(layouts);

            this.propertyForm.Style["width"] = "100%";
            //this.propertyForm.Style["height"] = "400";

        }

        protected override void OnPreRender(EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (this.IsPostBack == false && this.IsCallback == false)
            {
                this.InitPropertyForm(this.Data, this.EditEnabled == false);
            }

            base.OnPreRender(e);

            //if (this.Data.Status != SchemaObjectStatus.Normal)
            //    this.okButton.Visible = false;
        }

        public void AfterNormalSceneApplied()
        {
            //this.okButton.Visible = this.Data != null && this.Data.Status == SchemaObjectStatus.Normal;
        }

        public string NormalSceneName
        {
            get { return this.EditEnabled ? "Normal" : "ReadOnly"; }
        }

        public string ReadOnlySceneName
        {
            get { return "ReadOnly"; }
        }

        private void PropertyEditorRegister()
        {
            PropertyEditorHelper.RegisterEditor(new StandardPropertyEditor());
            PropertyEditorHelper.RegisterEditor(new BooleanPropertyEditor());
            PropertyEditorHelper.RegisterEditor(new EnumPropertyEditor());
            PropertyEditorHelper.RegisterEditor(new ObjectPropertyEditor());
            PropertyEditorHelper.RegisterEditor(new DatePropertyEditor());
            PropertyEditorHelper.RegisterEditor(new DateTimePropertyEditor());
            PropertyEditorHelper.RegisterEditor(new CodeNameUniqueEditor());
            PropertyEditorHelper.RegisterEditor(new GetPinYinEditor());
            PropertyEditorHelper.RegisterEditor(new ImageUploaderPropertyEditor());
            PropertyEditorHelper.RegisterEditor(new PObjectNameEditor());

            //PropertyEditorHelper.RegisterEditor(new EntityFieldPropertyEditorSceneAdapter());

        }
        [ControllerMethod(true)]
        protected void CreateNewObject()
        {
            this.CreateNewObject("DynamicEntity");
        }

        [ControllerMethod]
        protected void CreateNewObject(string schemaType)
        {
            if (this.sceneAdapter == null)
            {
                this.sceneAdapter = PropertyEditorSceneAdapter.Create(schemaType);
                this.sceneAdapter.Mode = SCObjectOperationMode.Add;
            }

            this.CreateNewObjectBySchemaType(schemaType);

        }

        [ControllerMethod]
        protected void LoadObject(string id)
        {
            DynamicEntity result;

            result = (DynamicEntity)DESchemaObjectAdapter.Instance.Load(id);
            this.Data = result;

            this.sceneAdapter = PropertyEditorSceneAdapter.Create(this.Data.SchemaType);
            this.sceneAdapter.ObjectID = this.Data.ID;
            this.sceneAdapter.Mode = SCObjectOperationMode.Update;
            this.currentSchemaType.Value = this.Data.SchemaType;
        }

        [ControllerMethod]
        protected void LoadObject(string id, long reserved, string time)
        {
            TimePointContext.Current.UseCurrentTime = false;
            TimePointContext.Current.SimulatedTime = DateTime.Parse(time).ToLocalTime();
            this.LoadObject(id);
        }

        private void CreateNewObjectBySchemaType(string schemaType)
        {
            this.Data = SchemaExtensions.CreateObject(schemaType);

            // this.CurrentSchemaType = category;
            this.currentSchemaType.Value = schemaType;
            this.Data.ID = UuidHelper.NewUuidString();
        }
    }
}