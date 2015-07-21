using MCS.Library.OGUPermission;
using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Enums;
using MCS.Library.SOA.DataObjects.Dynamics.Executors;
using MCS.Library.SOA.DataObjects.Dynamics.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MCS.Library.SOA.DataObjects.Dynamics.Test
{


    /// <summary>
    ///这是 CopyEntitiesTest 的测试类，旨在
    ///包含所有 CopyEntitiesTest 单元测试
    ///</summary>
    [TestClass]
    public class CopyEntitiesTest
    {
        public CopyEntitiesTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
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

        [TestCategory("CopyObject"), TestMethod]
        public void CopyEntityMethod()
        {
            // /集团公司/管道板块/运输
            string categoryID = "48BE753C-630D-42F4-A02D-D2B50818F817";
            // /集团公司/销售板块/销售订单
            string terminalCategoryID = "EBABB15A-AFD8-4072-A5C9-03F1B0B5CDFF";

            var entity = creatEntity(categoryID);

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            List<string> entitiesIDs = new List<string>();
            entitiesIDs.Add(entity.ID);
            List<string> categoriesIDs = new List<string>();
            categoriesIDs.Add(terminalCategoryID);
            DEObjectOperations.InstanceWithoutPermissions.CopyEntities(entitiesIDs, categoriesIDs);

            DynamicEntity loadEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/销售板块/销售订单/" + entity.Name) as DynamicEntity;

            Assert.IsTrue(loadEntity != null);
        }

        [TestCategory("CopyObject"), TestMethod]
        public void CopyEntityWithChildMethod()
        {
            // /集团公司/管道板块/运输
            string categoryID = "48BE753C-630D-42F4-A02D-D2B50818F817";
            // /集团公司/销售板块/销售订单
            string terminalCategoryID = "EBABB15A-AFD8-4072-A5C9-03F1B0B5CDFF";

            var entity = creatEntity(categoryID);
            var childEntity = creatChildEntity(categoryID);
            //子表入库
            DEObjectOperations.InstanceWithoutPermissions.AddEntity(childEntity);
            //子表CodeName
            string childCodeName = childEntity.CodeName;
            //主表字段跟子表关联
            entity.Fields[0].FieldType = FieldTypeEnum.Collection;
            entity.Fields[0].ReferenceEntityCodeName = childCodeName;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            List<string> entitiesIDs = new List<string>();
            entitiesIDs.Add(entity.ID);
            entitiesIDs.Add(childEntity.ID);
            List<string> categoriesIDs = new List<string>();
            categoriesIDs.Add(terminalCategoryID);
            DEObjectOperations.InstanceWithoutPermissions.CopyEntities(entitiesIDs, categoriesIDs);

            DynamicEntity loadEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/销售板块/销售订单/" + entity.Name) as DynamicEntity;
            DynamicEntity loadChildEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/销售板块/销售订单/" + childEntity.Name) as DynamicEntity;

            bool mainEntityOK = loadEntity != null;
            bool childEntityOK = loadChildEntity != null;
            bool fieldOK = loadEntity.Fields[0].FieldType == FieldTypeEnum.Collection && loadEntity.Fields[0].ReferenceEntityCodeName == loadChildEntity.CodeName;

            Assert.IsTrue(mainEntityOK && childEntityOK && fieldOK);
        }

        [TestCategory("CopyObject"), TestMethod]
        public void CopyEntityWithOutEntityMethod()
        {
            // /集团公司/管道板块/运输
            string categoryID = "48BE753C-630D-42F4-A02D-D2B50818F817";
            // /集团公司/销售板块/销售订单
            string terminalCategoryID = "EBABB15A-AFD8-4072-A5C9-03F1B0B5CDFF";

            var entity = creatEntity(categoryID);
            var childEntity = creatChildEntity(categoryID);
            //子表入库
            DEObjectOperations.InstanceWithoutPermissions.AddEntity(childEntity);
            //子表CodeName
            string childCodeName = childEntity.CodeName;
            //主表字段跟子表关联
            var field = entity.Fields[0];
            field.FieldType = FieldTypeEnum.Collection;
            field.ReferenceEntityCodeName = childCodeName;

            //主表入库
            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);


            OuterEntityField outField = new OuterEntityField()
            {
                Name = "OutField",
                ID = Guid.NewGuid().ToString(),
                Description = "外部字段"
            };

            OuterEntity outEntity = new OuterEntity() { 
              ID = Guid.NewGuid().ToString(), Name = "OEntity", Description = "外部实体"
            };
            //实体字段与外部字段
            EntityFieldMapping fieldMapping = new EntityFieldMapping()
            {
                FieldID = field.ID,
                FieldDefaultValue = field.DefaultValue,
                FieldDesc = field.Description,
                FieldLength = field.Length,
                FieldName = field.Name,
                FieldTypeName = field.FieldType.ToString(),
                OuterFieldID = outField.ID,
                OuterFieldName = outField.Name,
                SortNo = field.SortNo
            };
            List<EntityFieldMapping> entityFieldMappingCollection = new List<EntityFieldMapping>();
            entityFieldMappingCollection.Add(fieldMapping);

            EntityMapping mapping = new EntityMapping()
            { 
                InnerEntity = entity, OuterEntityID = outEntity.ID, OuterEntityName = outEntity.Name, OuterEntityInType = Contract.InType.StandardInterface,
                EntityFieldMappingCollection = entityFieldMappingCollection
            };

            DEObjectOperations.InstanceWithoutPermissions.AddEntityMapping(mapping);

            List<string> entitiesIDs = new List<string>();
            entitiesIDs.Add(entity.ID);
            entitiesIDs.Add(childEntity.ID);
            List<string> categoriesIDs = new List<string>();
            categoriesIDs.Add(terminalCategoryID);
            DEObjectOperations.InstanceWithoutPermissions.CopyEntities(entitiesIDs, categoriesIDs);

            DynamicEntity loadEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/销售板块/销售订单/" + entity.Name) as DynamicEntity;
            DynamicEntity loadChildEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/销售板块/销售订单/" + childEntity.Name) as DynamicEntity;

            bool mainEntityOK = loadEntity != null;
            bool childEntityOK = loadChildEntity != null;
            bool fieldOK = loadEntity.Fields[0].FieldType == FieldTypeEnum.Collection && loadEntity.Fields[0].ReferenceEntityCodeName == loadChildEntity.CodeName;
            bool fieldMappingS = loadEntity.Fields[0].OuterEntityFields.Count == 1 && loadEntity.Fields[0].OuterEntityFields[0].Name == "OutField";
            bool entitymapping = loadEntity.OuterEntities[0].Name == "OEntity";
            Assert.IsTrue(mainEntityOK && childEntityOK && fieldOK && fieldMappingS && entitymapping);
        }

        #region 移动实体
        //移动实体，该实体没有子表，没有外部实体对应
        [TestCategory("MoveObject"), TestMethod]
        public void MoveOneEntityNoChild()
        {
            // /集团公司/管道板块/运输
            string categoryID = "48BE753C-630D-42F4-A02D-D2B50818F817";
            // /集团公司/销售板块/销售订单
            string terminalCategoryID = "EBABB15A-AFD8-4072-A5C9-03F1B0B5CDFF";

            var entity = creatEntity(categoryID);

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            List<string> entitiesIDs = new List<string>();
            entitiesIDs.Add(entity.ID);
            List<string> categoriesIDs = new List<string>();
            categoriesIDs.Add(terminalCategoryID);
            DEObjectOperations.InstanceWithoutPermissions.MoveEntities(entitiesIDs, categoriesIDs);

            DynamicEntity loadEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/销售板块/销售订单/" + entity.Name) as DynamicEntity;

            string errorMsg = string.Empty;
            try
            {
                DynamicEntity loadOldEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/管道板块/运输/" + entity.Name) as DynamicEntity;
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }

            string error = "不能找到CodeName为[/集团公司/管道板块/运输/" + entity.Name + "]的对象";
            Assert.IsTrue(loadEntity != null && !string.IsNullOrEmpty(errorMsg) && errorMsg.Contains(error));
        }

        //移动实体，该实体有子表，没有外部实体对应
        [TestCategory("MoveObject"), TestMethod]
        public void MoveOneEntityWithChild()
        {
            // /集团公司/管道板块/运输
            string categoryID = "48BE753C-630D-42F4-A02D-D2B50818F817";
            // /集团公司/销售板块/销售订单
            string terminalCategoryID = "EBABB15A-AFD8-4072-A5C9-03F1B0B5CDFF";

            var entity = creatEntity(categoryID);
            var childEntity = creatChildEntity(categoryID);

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(childEntity);

            entity.Fields[0].FieldType = FieldTypeEnum.Collection;
            entity.Fields[0].ReferenceEntityCodeName = childEntity.CodeName;
            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            List<string> entitiesIDs = new List<string>();
            entitiesIDs.Add(childEntity.ID);
            entitiesIDs.Add(entity.ID);
            List<string> categoriesIDs = new List<string>();
            categoriesIDs.Add(terminalCategoryID);
            DEObjectOperations.InstanceWithoutPermissions.MoveEntities(entitiesIDs, categoriesIDs);

            DynamicEntity loadEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/销售板块/销售订单/" + entity.Name) as DynamicEntity;
            DynamicEntity loadChildEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName("/集团公司/销售板块/销售订单/" + childEntity.Name) as DynamicEntity;

            string errorMsg = string.Empty;
            string childEntityMsg = string.Empty;
            try
            {
                DynamicEntity loadOldEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName(entity.CodeName) as DynamicEntity;
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            try
            {
                DynamicEntity loadOldChildEntity = DEDynamicEntityAdapter.Instance.LoadByCodeName(childEntity.CodeName) as DynamicEntity;
            }
            catch (Exception e)
            {
                childEntityMsg = e.Message;
            }
            string error = "不能找到CodeName为[" + entity.CodeName + "]的对象";
            string childError = "不能找到CodeName为[" + childEntity.CodeName + "]的对象";

            bool entityIsOk = loadEntity != null;
            bool childEntityIsOk = loadChildEntity != null;
            bool refChildIsOk = loadEntity.Fields[0].FieldType == FieldTypeEnum.Collection && loadEntity.Fields[0].ReferenceEntityCodeName == loadChildEntity.CodeName;

            bool oldEntityIsOk = errorMsg.Contains(error);
            bool oldChildEntityIsOk = childEntityMsg.Contains(childError);
            bool isSuccess = entityIsOk && childEntityIsOk && refChildIsOk && oldEntityIsOk && oldChildEntityIsOk;
            Assert.IsTrue(isSuccess);
        }

        #endregion

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DESchemaObjectAdapter.Instance.ClearAllData();
            DESchemaObjectAdapter.Instance.InitAllData();
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 创建实体字段
        /// </summary>
        /// <returns></returns>
        private DynamicEntityField creatEntityField(int sortNo, string flag = "new")
        {
            var field = new DynamicEntityField()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "字段",
                Description = "描述" + flag,
                Length = 2,
                DefaultValue = "默认值",
                FieldType = FieldTypeEnum.Decimal,
                Creator = (IUser)OguBase.CreateWrapperObject(new OguUser("22c3b351-a713-49f2-8f06-6b888a280fff")),//wangli5
                SortNo = sortNo
            };
            return field;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        private DynamicEntity creatEntity(string categoryID)
        {
            string entityId = Guid.NewGuid().ToString();

            var entity = new DynamicEntity
            {
                ID = entityId,
                Name = "实体1",
                Description = "描述",
                CreateDate = DateTime.Now,
                CategoryID = categoryID,//已存在的分类编码
                Fields = new DynamicEntityFieldCollection(),
                SortNo = 0,
                Creator = (IUser)OguBase.CreateWrapperObject(new OguUser("22c3b351-a713-49f2-8f06-6b888a280fff")),//wangli5
            };

            for (var i = 0; i < 2; i++)
            {
                var field = creatEntityField(i);
                entity.Fields.Add(field);
            }
            return entity;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        private DynamicEntity creatChildEntity(string categoryID)
        {
            string entityId = Guid.NewGuid().ToString();

            var entity = new DynamicEntity
            {
                ID = entityId,
                Name = "行项目实体1",
                Description = "描述",
                CreateDate = DateTime.Now,
                CategoryID = categoryID,//已存在的分类编码
                Fields = new DynamicEntityFieldCollection(),
                SortNo = 0,
                Creator = (IUser)OguBase.CreateWrapperObject(new OguUser("22c3b351-a713-49f2-8f06-6b888a280fff")),//wangli5
            };

            for (var i = 0; i < 2; i++)
            {
                var field = creatEntityField(i);
                entity.Fields.Add(field);
            }
            return entity;
        }

        /// <summary>
        /// 创建外部实体字段
        /// </summary>
        /// <returns></returns>
        private OuterEntityField creatChildEntity()
        {
            string fieldId = Guid.NewGuid().ToString();

            var newOuterEntityField = new OuterEntityField()
            {
                ID = fieldId,
                Name = "OuterEntityField",

            };
            return newOuterEntityField;
        }

        #endregion
    }
}
