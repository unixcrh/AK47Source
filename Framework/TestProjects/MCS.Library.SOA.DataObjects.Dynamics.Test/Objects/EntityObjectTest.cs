using MCS.Library.OGUPermission;
using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Enums;
using MCS.Library.SOA.DataObjects.Dynamics.Executors;
using MCS.Library.SOA.DataObjects.Dynamics.Objects;
using MCS.Library.SOA.DataObjects.Dynamics.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MCS.Library.SOA.DataObjects.Dynamics.Test.Objects
{
    /// <summary>
    /// EntityObjectTest 的摘要说明
    /// </summary>
    [TestClass]
    public class EntityObjectTest
    {
        public EntityObjectTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DESchemaObjectAdapter.Instance.ClearAllData();
            DESchemaObjectAdapter.Instance.InitAllData();
        }
        #endregion

        #region 操作实体
        [TestCategory("EntityObject"), TestMethod]
        public void AddEntityMethod()
        {
            var entity = creatEntity();
            List<string> fieldIDs = new List<string>();
            entity.Fields.ForEach(p => fieldIDs.Add(p.ID));

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            var dbResult = DESchemaObjectAdapter.Instance.Load(entity.ID) as DynamicEntity;
            //field
            bool fieldSuccess = fieldIDs.Count == dbResult.Fields.Count;
            foreach (var field in dbResult.Fields)
            {
                if (!fieldIDs.Contains(field.ID))
                {
                    fieldSuccess = false;
                }
            }
            //relation
            bool relationSuccess = dbResult.AllMembersRelations.Count == fieldIDs.Count;
            foreach (var relation in dbResult.AllMembersRelations)
            {
                if (relation.ContainerID != dbResult.ID)
                {
                    relationSuccess = false;
                }
                if (!fieldIDs.Contains(relation.ID))
                {
                    relationSuccess = false;
                }
            }

            Assert.IsTrue(dbResult.ID.Equals(entity.ID) && fieldSuccess && relationSuccess);
        }

        [TestCategory("EntityObject"), TestMethod]
        public void UpdateEntityMethod()
        {
            var deleteFieldID = "";
            var newFieldID = "";
            string updatedDesc = "Test Update";

            var entity = creatEntity();

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            var entityUpdate = DESchemaObjectAdapter.Instance.Load(entity.ID) as DynamicEntity;

            entityUpdate.Description = updatedDesc;

            if (entityUpdate.Fields.Count() > 0)
            {
                deleteFieldID = entityUpdate.Fields[0].ID;
                entityUpdate.Fields.RemoveAt(0);
            }
            var updateField = creatEntityField("update");

            newFieldID = updateField.ID;

            entityUpdate.Fields.Add(updateField);


            DEObjectOperations.InstanceWithoutPermissions.UpdateEntity(entityUpdate);

            var entityUpdated = DESchemaObjectAdapter.Instance.Load(entity.ID) as DynamicEntity;

            Assert.IsTrue(entityUpdated.Description.Equals(updatedDesc) && (entityUpdated.Fields.ToIDArray().Contains(newFieldID) && !entityUpdated.Fields.ToIDArray().Contains(deleteFieldID)), "Update Error!");
        }

        [TestCategory("EntityObject"), TestMethod]
        public void DeleteEntityMethod()
        {
            var entityAdd = creatEntity();

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entityAdd);

            string entityId = entityAdd.ID;

            var entity = DESchemaObjectAdapter.Instance.Load(entityId) as DynamicEntity;

            DEObjectOperations.InstanceWithoutPermissions.DeleteEntity(entity);

            var dbResult = DESchemaObjectAdapter.Instance.Load(entityId, false);


            Assert.IsTrue(dbResult.Status == DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Deleted, "Delete Error!");
        }

        [TestCategory("EntityObject"), TestMethod]
        public void DeleteEntityFromCollectionMethod()
        {
            DynamicEntityCollection entities = new DynamicEntityCollection();

            var entity = creatEntity();

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            var dbResult = DESchemaObjectAdapter.Instance.Load(entity.ID);

            Assert.IsTrue(dbResult.ID.Equals(entity.ID));
        }

        #endregion

        #region 操作实体字段

        [TestCategory("EntityObject"), TestMethod]
        public void AddEntityFieldMethod()
        {
            var field = creatEntityField();

            DEObjectOperations.InstanceWithoutPermissions.AddEntityField(field);

            var dbResult = DESchemaObjectAdapter.Instance.Load(field.ID);

            Assert.IsTrue(dbResult.ID.Equals(field.ID));
        }

        [TestCategory("EntityObject"), TestMethod]
        public void UpdateEntityFieldMethod()
        {
            var field = creatEntityField();

            DEObjectOperations.InstanceWithoutPermissions.AddEntityField(field);

            var dbResult = DESchemaObjectAdapter.Instance.Load(field.ID) as DynamicEntityField;

            var defaultValue = "default update";
            dbResult.DefaultValue = defaultValue;

            DEObjectOperations.InstanceWithoutPermissions.UpdateEntityField(dbResult);

            var result = DESchemaObjectAdapter.Instance.Load(dbResult.ID) as DynamicEntityField;

            Assert.IsTrue(result.DefaultValue.Equals(defaultValue));
        }

        [TestCategory("EntityObject"), TestMethod]
        public void DeleteEntityFieldMethod()
        {
            var field = creatEntityField();

            DEObjectOperations.InstanceWithoutPermissions.AddEntityField(field);

            var dbResult = DESchemaObjectAdapter.Instance.Load(field.ID) as DynamicEntityField;

            DEObjectOperations.InstanceWithoutPermissions.DeleteEntityField(dbResult);

            var result = DESchemaObjectAdapter.Instance.Load(dbResult.ID, false) as DynamicEntityField;

            Assert.IsTrue(result.Status == DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Deleted);
        }

        #endregion

        #region 操作实体集合
        [TestCategory("EntityObject"), TestMethod]
        public void EntitiyCollectionToStringTest()
        {
            DynamicEntityCollection TestData = new DynamicEntityCollection();

            //创建测试数据
            TestData.CopyFrom(MockData.CreateRelationDynamicEntityCollection());

            var result = TestData.ToString();

            Assert.IsTrue(!string.IsNullOrEmpty(result));

        }

        [TestCategory("EntityObject"), TestMethod]
        public void EntityCollectionFromXElement()
        {
            XElement element = getXElement();

            string msg = string.Empty;
            string CategoryID = "48BE753C-630D-42F4-A02D-D2B50818F817";//集团公司/管道板块/运输
            DEDynamicEntityImportAdapter.Instance.Import(element, CategoryID, out msg);

            Assert.IsTrue(!msg.Contains("失败"));
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 创建实体字段
        /// </summary>
        /// <returns></returns>
        private DynamicEntityField creatEntityField(string flag = "new")
        {
            var field = new DynamicEntityField()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "字段",
                Description = "描述" + flag,
                Length = 2,
                DefaultValue = "默认值",
                FieldType = FieldTypeEnum.Decimal,
                //CodeName = "Field",
                Creator = (IUser)OguBase.CreateWrapperObject(new OguUser("22c3b351-a713-49f2-8f06-6b888a280fff")),//wangli5
                SortNo = 0
            };
            return field;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        private DynamicEntity creatEntity()
        {
            string entityId = Guid.NewGuid().ToString();

            var entity = new DynamicEntity
            {
                ID = entityId,
                //CodeName = "Entity1",
                Name = "实体1",
                Description = "描述",
                CreateDate = DateTime.Now,
                CategoryID = "763DF7AB-4B69-469A-8A01-041DDEAB19F7",//已存在的分类编码
                SortNo = 0,
                Fields = new DynamicEntityFieldCollection(),
                Creator = (IUser)OguBase.CreateWrapperObject(new OguUser("22c3b351-a713-49f2-8f06-6b888a280fff")),
            };

            for (var i = 0; i < 2; i++)
            {
                var field = creatEntityField();
                entity.Fields.Add(field);
            }
            return entity;
        }

        private XElement getXElement()
        {
            string str = @"<DynamicEntities>
                              <Entity SchemaType='DynamicEntity' ID='6512c32d-b3a8-4891-bac2-e69bfb8839a3' CodeName='/集团公司/管道板块/运输/销售单明细' Name='销售单明细' Description='销售单明细' SortNo='1' CategoryID='48BE753C-630D-42F4-A02D-D2B50818F817'>
                                <Fields>
                                  <Field SchemaType='DynamicEntityField' ID='d9c58562-bb77-495d-9849-c6659c1494b2' CodeName='物料名称' Name='物料名称' Description='' SortNo='0' Length='255' FieldType='String' ReferenceEntityCodeName='' DefaultValue='' />
                                  <Field SchemaType='DynamicEntityField' ID='8c59763b-7be4-4334-898b-1b6b7950e381' CodeName='物料数量' Name='物料数量' Description='' SortNo='1' Length='4' FieldType='Int' ReferenceEntityCodeName='' DefaultValue='' />
                                  <Field SchemaType='DynamicEntityField' ID='466af2af-ed8c-4de6-a01f-cdd313ffcd76' CodeName='单价' Name='单价' Description='' SortNo='2' Length='18' FieldType='Decimal' ReferenceEntityCodeName='' DefaultValue='' />
                                </Fields>
                                <OuterEntities>
                                  <OuterEntity SchemaType='OuterEntity' ID='6ec461e5-e0f2-4ccd-aa4d-2e3cf3e9ea68' CodeName='' Name='Tcode_test_Item' Description='' SortNo='' CustomType='StandardInterface'>
                                    <OuterFields>
                                      <OuterField SchemaType='OuterEntityField' ID='13a4a795-b96b-43dc-9177-c94b6713b131' CodeName='' Name='物料名称_Mapping' Description='' SortNo='' MappingFieldID='d9c58562-bb77-495d-9849-c6659c1494b2' />
                                      <OuterField SchemaType='OuterEntityField' ID='be1528d2-74dc-4632-8f68-8a87a9687212' CodeName='' Name='单价_Mapping' Description='' SortNo='' MappingFieldID='466af2af-ed8c-4de6-a01f-cdd313ffcd76' />
                                      <OuterField SchemaType='OuterEntityField' ID='f0603a84-7103-4542-aa5d-2aad2f8dad28' CodeName='' Name='物料数量_Mapping' Description='' SortNo='' MappingFieldID='8c59763b-7be4-4334-898b-1b6b7950e381' />
                                    </OuterFields>
                                  </OuterEntity>
                                </OuterEntities>
                              </Entity>
                              <Entity SchemaType='DynamicEntity' ID='04eaf2d0-0455-41ba-bccb-ba2fee11125b' CodeName='/集团公司/管道板块/运输/销售单' Name='销售单' Description='销售单' SortNo='0' CategoryID='48BE753C-630D-42F4-A02D-D2B50818F817'>
                                <Fields>
                                  <Field SchemaType='DynamicEntityField' ID='b76bf66d-3388-4e33-a7dc-af6b6fb16966' CodeName='总金额' Name='总金额' Description='' SortNo='0' Length='18' FieldType='Decimal' ReferenceEntityCodeName='' DefaultValue='' />
                                  <Field SchemaType='DynamicEntityField' ID='c91ae5ca-fc42-485d-85a8-c2f9dec71550' CodeName='销售明细' Name='销售明细' Description='' SortNo='1' Length='999' FieldType='Collection' ReferenceEntityCodeName='/集团公司/管道板块/运输/销售单明细' DefaultValue='' />
                                </Fields>
                                <OuterEntities>
                                  <OuterEntity SchemaType='OuterEntity' ID='def2f3f9-ac31-4da4-a935-cde68337e6c4' CodeName='' Name='Tcode_test' Description='' SortNo='' CustomType='StandardInterface'>
                                    <OuterFields>
                                      <OuterField SchemaType='OuterEntityField' ID='20db4fee-7f5f-4ba1-a52d-44c2d2f3b4b5' CodeName='' Name='总金额_Mapping' Description='' SortNo='' MappingFieldID='b76bf66d-3388-4e33-a7dc-af6b6fb16966' />
                                      <OuterField SchemaType='OuterEntityField' ID='d98147f3-adce-4f1e-b0de-bb137cf6f47e' CodeName='' Name='Tcode_test_Item' Description='' SortNo='' MappingFieldID='c91ae5ca-fc42-485d-85a8-c2f9dec71550' />
                                    </OuterFields>
                                  </OuterEntity>
                                </OuterEntities>
                              </Entity>
                            </DynamicEntities>";

            XElement element = XElement.Parse(str);

            return element;
        }

        #endregion
    }
}
