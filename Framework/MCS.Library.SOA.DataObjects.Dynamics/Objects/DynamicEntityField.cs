using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using MCS.Library.Core;
using MCS.Library.Data.Mapping;
using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Enums;
using MCS.Library.SOA.DataObjects.Dynamics.Instance.ValueDefine;
using MCS.Library.SOA.DataObjects.Dynamics.Organizations;
using MCS.Library.SOA.DataObjects.Dynamics.Schemas;
using MCS.Library.SOA.DataObjects.Schemas.SchemaProperties;

namespace MCS.Library.SOA.DataObjects.Dynamics.Objects
{
    /// <summary>
    /// 动态实体字段
    /// </summary>
    [Serializable]
    public class DynamicEntityField : DEBase, IDEMemberObject, IDEContainerObject, IXElementSerializable
    {
        public DynamicEntityField()
            : base(DEStandardObjectSchemaType.DynamicEntityField.ToString())
        {
        }

        public DynamicEntityField(string schemaType)
            : base(schemaType)
        {

        }


        /// <summary>
        /// 长度
        /// </summary>
        [NoMapping]
        public int Length
        {
            get
            {
                return this.Properties.GetValue("Length", 0);
            }
            set
            {
                this.Properties.SetValue("Length", value);
            }
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        [NoMapping]
        public FieldTypeEnum FieldType
        {
            get
            {
                return this.Properties.GetValue("FieldType", FieldTypeEnum.String);
            }
            set
            {
                this.Properties.SetValue("FieldType", value);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        [NoMapping]
        public string DefaultValue
        {
            get
            {
                return this.Properties.GetValue("DefaultValue", string.Empty);
            }
            set
            {
                this.Properties.SetValue("DefaultValue", value);
            }
        }


        private DynamicEntity _ReferenceEntity = null;
        public DynamicEntity ReferenceEntity
        {
            get
            {
                if (this._ReferenceEntity == null && ReferenceEntityCodeName.IsNotEmpty())
                {
                    _ReferenceEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName(this.ReferenceEntityCodeName) as DynamicEntity;
                }

                return _ReferenceEntity;
            }
        }

        /// <summary>
        /// 引用实体CodeName
        /// </summary>
        [NoMapping]
        public string ReferenceEntityCodeName
        {
            get
            {
                return this.Properties.GetValue("ReferenceEntityCodeName", string.Empty);
            }
            set
            {
                this.Properties.SetValue("ReferenceEntityCodeName", value);
            }
        }

        /// <summary>
        /// 参数方向
        /// </summary>
        [NoMapping]
        public ParamDirectionEnum ParamDirection
        {
            get
            {
                return this.Properties.GetValue("ParamDirection", ParamDirectionEnum.NotKnown);
            }
            set
            {
                this.Properties.SetValue("ParamDirection", value);
            }
        }

        /// <summary>
        /// 是否结构
        /// </summary>
        [NoMapping]
        public bool IsStruct
        {
            get
            {
                return this.Properties.GetValue("IsStruct", false);
            }
            set
            {
                this.Properties.SetValue("IsStruct", value);
            }
        }

        #region IDEMemberObject
        [NonSerialized]
        private DEObjectContainerRelationCollection _AllMemberOfRelations = null;

        /// <summary>
        /// 获取用户的所有成员关系的集合
        /// </summary>
        /// <value> 一个<see cref="DEObjectContainerRelationCollection"/></value>
        [ScriptIgnore]
        [NoMapping]
        public DEObjectContainerRelationCollection AllMemberOfRelations
        {
            get
            {
                if (this._AllMemberOfRelations == null && this.ID.IsNotEmpty())
                    this._AllMemberOfRelations = DEMemberRelationAdapter.Instance.LoadByMemberID(this.ID);

                return this._AllMemberOfRelations;
            }
        }

        /// <summary>
        /// 获取一个<see cref="DEObjectContainerRelationCollection"/>，表示当前用户的成员关系
        /// </summary>
        [ScriptIgnore]
        [NoMapping]
        public DEObjectContainerRelationCollection CurrentMemberOfRelations
        {
            get
            {
                return (DEObjectContainerRelationCollection)AllMemberOfRelations.FilterByStatus(DESchemaObjectStatusFilterTypes.Normal);
            }
        }

        public DEObjectContainerRelationCollection GetCurrentMemberOfRelations()
        {
            return this.CurrentMemberOfRelations;
        }

        #endregion

        private OuterEntityFieldCollection _outerEntityFields = null;
        public OuterEntityFieldCollection OuterEntityFields
        {
            get
            {
                if (this._outerEntityFields == null && this.CurrentMembers != null)
                {
                    var oEntities = new DESchemaObjectCollection();

                    oEntities.CopyFrom(this.CurrentMembers.Where(p => p.SchemaType.Trim() == DEStandardObjectSchemaType.OuterEntityField.ToString()));

                    this._outerEntityFields = OuterEntityFieldCollection.FromSchemaObjects(oEntities);
                }
                return this._outerEntityFields;
            }
            set
            {
                _outerEntityFields = value;
            }
        }

        [NonSerialized]
        private DEObjectMemberRelationCollection _AllMembersRelations = null;

        [ScriptIgnore]
        [NoMapping]
        public DEObjectMemberRelationCollection AllMembersRelations
        {
            get
            {
                if (this._AllMembersRelations == null && this.ID.IsNotEmpty())
                    this._AllMembersRelations = DEMemberRelationAdapter.Instance.LoadByContainerID(this.ID);

                return this._AllMembersRelations;
            }
        }
        [NonSerialized]
        private DESchemaObjectCollection _CurrentMembers = null;

        /// <summary>
        /// 获取动态实体的字段
        /// </summary>
        [ScriptIgnore]
        [NoMapping]
        public DESchemaObjectCollection CurrentMembers
        {
            get
            {
                if (this._CurrentMembers == null && this.ID.IsNotEmpty())
                {
                    this._CurrentMembers = DESchemaObjectAdapter.Instance.Load(CurrentMembersRelations.ToMemberIDsBuilder());
                }

                return _CurrentMembers;
            }
        }
        /// <summary>
        /// 获取一个<see cref="DEObjectMemberRelationCollection"/>，表示当前实体的成员
        /// </summary>
        [ScriptIgnore]
        [NoMapping]
        public DEObjectMemberRelationCollection CurrentMembersRelations
        {
            get
            {
                return (DEObjectMemberRelationCollection)AllMembersRelations.FilterByStatus(DESchemaObjectStatusFilterTypes.Normal);
            }
        }
        public DEObjectMemberRelationCollection GetCurrentMembersRelations()
        {
            return this.CurrentMembersRelations;
        }
        public DESchemaObjectCollection GetCurrentMembers()
        {
            return this.CurrentMembers;
        }

        public void Serialize(System.Xml.Linq.XElement node, XmlSerializeContext context)
        {
            throw new NotImplementedException();
            //参考MCS.Library.SOA.DataObjects.PropertyDefine
        }

        public void Deserialize(System.Xml.Linq.XElement node, XmlDeserializeContext context)
        {
            throw new NotImplementedException();
            //参考MCS.Library.SOA.DataObjects.PropertyDefine
        }

        /// <summary>
        /// 获取与某外部实体对应的字段
        /// </summary>
        /// <param name="outerEntityID"></param>
        /// <returns></returns>
        public OuterEntityField GetOuterEntityFieldByOuterEntityID(string outerEntityID)
        {
            outerEntityID.CheckStringIsNullOrEmpty<ArgumentNullException>("outerEntityID");

            return this.OuterEntityFields.FirstOrDefault(p => p.OuterEntity.ID.Equals(outerEntityID));
        }

        public DynamicEntity Entity
        {
            get
            {
                var relation = this.AllMemberOfRelations.Where(p => p.ContainerSchemaType == "DynamicEntity").FirstOrDefault();
                DynamicEntity entity = null;
                if (relation != null)
                {
                    entity = relation.Container as DynamicEntity;
                }
                return entity;

            }
        }

        //  Add  by Che Qiang  2014.05.14
        /// <summary>
        /// 引用的ETL，作为绑定数据源使用
        /// </summary>
        [NoMapping]
        public string ReferenceETLEntityCodeName
        {
            get
            {
                return this.Properties.GetValue("ReferenceETLEntityCodeName", string.Empty);
            }
            set
            {
                this.Properties.SetValue("ReferenceETLEntityCodeName", value);
            }
        }
        #region 需求变更 配置实体不需要关联ETL实体字段


        ///// <summary>
        ///// 引用的ETL实体字段Key，作为绑定数据源使用
        ///// </summary>
        //[NoMapping]
        //public string ReferenceETLEntityFieldKey
        //{
        //    get
        //    {
        //        return this.Properties.GetValue("ReferenceETLEntityFieldKey", string.Empty);
        //    }
        //    set
        //    {
        //        this.Properties.SetValue("ReferenceETLEntityFieldKey", value);
        //    }
        //}

        ///// <summary>
        ///// 引用的ETL实体字段Value，作为绑定数据源使用
        ///// </summary>
        //[NoMapping]
        //public string ReferenceETLEntityFieldVale
        //{
        //    get
        //    {
        //        return this.Properties.GetValue("ReferenceETLEntityFieldVale", string.Empty);
        //    }
        //    set
        //    {
        //        this.Properties.SetValue("ReferenceETLEntityFieldVale", value);
        //    }
        //}
        #endregion
        /// <summary>
        /// 查看是否是数据源字段
        /// </summary>
        [NoMapping]
        public bool IsDataSource
        {
            get
            {
                return this.ReferenceETLEntityCodeName.IsNotEmpty();
            }
        }

        //  Add End
    }

    /// <summary>
    ///  表示<see cref="DynamicEntityField"/>的集合
    /// </summary>
    [Serializable]
    public class DynamicEntityFieldCollection : DESchemaObjectEditableKeyedCollectionBase<DynamicEntityField, DynamicEntityFieldCollection>
    {
        public SchemaPropertyValueCollection ToProperties()
        {
            SchemaPropertyValueCollection result = new SchemaPropertyValueCollection();

            this.ForEach(p =>
            {
                SchemaPropertyDefine pd = new SchemaPropertyDefine
                {
                    Name = p.Name,
                    Description = p.Description,
                    DefaultValue = p.DefaultValue
                };

                result.Add(new SchemaPropertyValue(pd));
            });

            return result;
        }

        public EntityFieldValueCollection ToFieldValueCollection()
        {
            EntityFieldValueCollection result = new EntityFieldValueCollection();

            this.ForEach(p =>
            {
                result.Add(new EntityFieldValue(p));
            });

            return result;
        }

        public static DynamicEntityFieldCollection FromSchemaObjects(DESchemaObjectCollection schemaObjectCollection)
        {
            schemaObjectCollection.NullCheck<ArgumentNullException>("schemaObjectCollection");

            var temp = new DynamicEntityFieldCollection();
            schemaObjectCollection.ForEach(p => temp.Add((DynamicEntityField)p));

            var result = new DynamicEntityFieldCollection();

            result.CopyFrom(temp.OrderBy(p => p.SortNo));

            return result;
        }

        public DESchemaObjectCollection ToSchemaObjects()
        {
            DESchemaObjectCollection result = new DESchemaObjectCollection();

            this.ForEach(u => result.Add(u));

            return result;
        }

        /// <summary>
        /// 创建过滤器结果集合
        /// </summary>
        /// <returns></returns>
        protected override DynamicEntityFieldCollection CreateFilterResultCollection()
        {
            return new DynamicEntityFieldCollection();
        }

        /// <summary>
        /// 获取集合中指定的<see cref="DynamicEntityField"/>的键。
        /// </summary>
        /// <param name="item">获取其键的<see cref="DynamicEntityField"/></param>
        /// <returns>表示键的字符串</returns>
        protected override string GetKeyForItem(DynamicEntityField item)
        {
            return item.ID;
        }
    }
}
