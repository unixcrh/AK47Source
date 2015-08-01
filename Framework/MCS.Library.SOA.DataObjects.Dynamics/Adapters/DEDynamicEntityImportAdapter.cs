using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml.Linq;
using System.Xml.XPath;
using MCS.Library.Caching;
using MCS.Library.Core;
using MCS.Library.Data;
using MCS.Library.Data.Builder;
using MCS.Library.Data.Mapping;
using MCS.Library.Principal;
using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Enums;
using MCS.Library.SOA.DataObjects.Dynamics.Executors;
using MCS.Library.SOA.DataObjects.Dynamics.Objects;
using MCS.Library.SOA.DataObjects.Dynamics.Schemas;
using MCS.Web.Library.Script;
using MCS.Library.SOA.DataObjects.Schemas.SchemaProperties;
using MCS.Library.SOA.DataObjects.Schemas.Adapters;

namespace MCS.Library.SOA.DataObjects.Dynamics.Adapters
{
    /// <summary>
    /// 实体导入Adapter
    /// </summary>
    public class DEDynamicEntityImportAdapter
    {
        /// <summary>
        /// <see cref="DESchemaObjectAdapter"/>的实例，此字段为只读
        /// </summary>
        public static readonly DEDynamicEntityImportAdapter Instance = new DEDynamicEntityImportAdapter();

        private DEDynamicEntityImportAdapter()
        {
        }

        /// <summary>
        /// 导入实体及映射关系
        /// </summary>
        /// <param name="element"></param>
        /// <param name="categoryID"></param>
        /// <param name="msg"></param>
        public void Import(XElement element, string categoryID, out string msg)
        {
            element.NullCheck("导入XML对象不能为Null");
            categoryID.CheckStringIsNullOrEmpty<ArgumentNullException>("导入分类编码");

            StringBuilder strB = new StringBuilder();

            try
            {
                CategoryAdapter.Instance.Exists(categoryID).FalseThrow("导入的分类{0}不存在", categoryID);

                //已导入的实体CodeName(因为存在关系导入)
                HashSet<string> importedCodeNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                //遍历每一个Entity
                element.XPathSelectElements("Entity").ForEach(xEntity =>
                {
                    string entityName = xEntity.AttributeValue("Name");
                    string entityCodeName = xEntity.AttributeValue("CodeName");

                    //判断是否已经导入过，一般情况是当有实体引用时关联导入过了
                    if (entityCodeName.IsNotEmpty() && importedCodeNames.Contains(entityCodeName) == false)
                    {
                        try
                        {
                            using (TransactionScope scope = TransactionScopeFactory.Create())
                            {
                                this.ImportSingleEntity(xEntity, categoryID, element, importedCodeNames);

                                strB.AppendLine(string.Format("{0}导入成功!", entityName));
                                scope.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            strB.AppendLine(string.Format("{0}导入失败！[{1}]", entityName, ex.Message));
                        }
                    }
                });
            }
            catch (System.Exception ex)
            {
                strB.AppendLine(string.Format("导入失败！[{0}]", ex.Message));
            }

            msg = strB.ToString();
        }

        //递归导入实体
        private string ImportSingleEntity(XElement xEntity, string categoryID, XElement elementAll, HashSet<string> importedCodeNames)
        {
            DynamicEntity entity = new DynamicEntity();
            entity.FromXElement(xEntity);

            //实体及实体字段入库
            #region

            //递归导入关联实体
            #region 递归导入关联实体
            //引用实体
            entity.Fields.Where(f => f.FieldType == FieldTypeEnum.Collection && f.ReferenceEntityCodeName.IsNotEmpty()).ForEach(
                field =>
                {
                    //所有引用类型需要在同一事务中入库
                    XElement referenceXElement = FilterXElement(field.ReferenceEntityCodeName, elementAll);
                    referenceXElement.NullCheck<ArgumentNullException>(string.Format("在XML找不到{0}的实体引用!", field.ReferenceEntityCodeName));

                    //递归调用
                    string newCodeName = this.ImportSingleEntity(referenceXElement, categoryID, elementAll, importedCodeNames);
                    if (newCodeName.IsNotEmpty())
                        field.ReferenceEntityCodeName = newCodeName;
                });
            #endregion

            //先记录原CodeName再更新CodeName，这点很重要
            string oldCodeName = entity.CodeName;

            if (importedCodeNames.Contains(entity.CodeName))
                return string.Empty;

            //importList.Add(entity.CodeName);
            entity.ID = Guid.NewGuid().ToString();
            entity.CategoryID = categoryID;
            entity.VersionStartTime = DateTime.MinValue;
            entity.VersionEndTime = DateTime.MinValue;
            entity.CreateDate = DateTime.Now.SimulateTime();
            //entity.BuidCodeName();

            //字段ID编码，更新前后的结果集
            List<KeyValuePair<string, string>> idMapping = new List<KeyValuePair<string, string>>();

            entity.Fields.ForEach(f =>
            {
                KeyValuePair<string, string> mapping = new KeyValuePair<string, string>(Guid.NewGuid().ToString(), f.ID);
                idMapping.Add(mapping);
                f.ID = mapping.Key;
                f.VersionStartTime = DateTime.MinValue;
                f.VersionEndTime = DateTime.MinValue;
                f.CreateDate = DateTime.Now.SimulateTime();
            });

            DEObjectOperations.InstanceWithoutPermissions.DoOperation(SCObjectOperationMode.Add, entity, null);
            //如果成功才把当前实体放入已导入列表中
            importedCodeNames.Add(oldCodeName);
            #endregion

            //外部实体及映射关系入库
            #region
            //当前实体
            xEntity.XPathSelectElements("OuterEntities/OuterEntity").ForEach(oe =>
            {
                OuterEntity outerEntity = new OuterEntity();
                outerEntity.FromXElement(oe);

                EntityMapping mapping = new EntityMapping();
                mapping.InnerEntity = entity;

                mapping.EntityFieldMappingCollection.ForEach(fieldMapping =>
                {
                    var outerField = oe.XPathSelectElements("OuterFields/OuterField")
                        .FirstOrDefault(of =>
                            of.AttributeValue("MappingFieldID") == idMapping.FirstOrDefault(id => id.Key == fieldMapping.FieldID).Value);

                    if (outerField != null)
                    {
                        fieldMapping.OuterFieldID = Guid.NewGuid().ToString();          //outerField.AttributeValue("ID");
                        fieldMapping.OuterFieldName = outerField.AttributeValue("Name");
                    }
                });

                //重新生成编码等信息
                outerEntity.BuildNewEntity();

                mapping.OuterEntityID = outerEntity.ID;
                mapping.OuterEntityInType = outerEntity.CustomType;
                mapping.OuterEntityName = outerEntity.Name;

                DEObjectOperations.InstanceWithoutPermissions.AddEntityMapping(mapping);
            });
            #endregion

            return entity.CodeName;
        }

        private static XElement FilterXElement(string codeName, XElement elementAll)
        {
            XElement result = null;

            elementAll.XPathSelectElements("Entity").ForEach(xEntity =>
            {
                if (xEntity.AttributeValue("CodeName") == codeName)
                {
                    result = xEntity;
                }
            });

            return result;
        }
    }
}
