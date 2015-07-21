using MCS.Library.OGUPermission;
using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Enums;
using MCS.Library.SOA.DataObjects.Dynamics.Executors;
using MCS.Library.SOA.DataObjects.Dynamics.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MCS.Library.SOA.DataObjects.Dynamics.Test.Adapter
{
    /// <summary>
    /// DESchemaObjectAdapterTest 的摘要说明
    /// </summary>
    [TestClass]
    public class DESchemaObjectAdapterTest
    {
        public DESchemaObjectAdapterTest()
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

        [TestCategory("DESchemaObjectAdapter"), TestMethod]
        public void LoadByIDTest()
        {
            var entity = createEntity();

            var description = "New Description";

            entity.Description = description;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            var loadedEntity = DESchemaObjectAdapter.Instance.Load(entity.ID) as DynamicEntity;

            Assert.IsTrue(loadedEntity.Description.Equals(description));
        }

        [TestCategory("DESchemaObjectAdapter"), TestMethod]
        public void LoadByIDAndTimeTest()
        {
            var entity = createEntity();

            var description = "New Description";

            entity.Description = description;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            var loadedEntity = DESchemaObjectAdapter.Instance.Load(entity.ID, DateTime.MinValue) as DynamicEntity;

            Assert.IsTrue(loadedEntity.Description.Equals(description));
        }

        [TestCategory("DESchemaObjectAdapter"), TestMethod]
        public void LoadBySchemaTypeTest()
        {
          //  DESchemaObjectAdapter.Instance.ClearAllData();

            var entity = createEntity();

            var description = "New Description";

            entity.Description = description;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            string[] schemaTypes = { "DynamicEntity", "DynamicEntityField" };

            var loadObjects = DESchemaObjectAdapter.Instance.LoadBySchemaType(schemaTypes, DateTime.MinValue);

            Assert.IsTrue(loadObjects.Count() == 3);

        }

        [TestCategory("DESchemaObjectAdapter"), TestMethod]
        public void UpdateTest()
        {
            var entity = createEntity();

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            var loadedEntity = DESchemaObjectAdapter.Instance.Load(entity.ID, DateTime.MinValue) as DynamicEntity;

            var description = "Update Description";

            loadedEntity.Description = description;

            DESchemaObjectAdapter.Instance.Update(loadedEntity);

            var result = DESchemaObjectAdapter.Instance.Load(entity.ID, DateTime.MinValue) as DynamicEntity;

            Assert.IsTrue(result.Description.Equals(description));

        }

        [TestCategory("DESchemaObjectAdapter"), TestMethod]
        public void UpdateStatusTest()
        {
            var entity = createEntity();

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(entity);

            var loadedEntity = DESchemaObjectAdapter.Instance.Load(entity.ID) as DynamicEntity;

            DESchemaObjectAdapter.Instance.UpdateStatus(loadedEntity, DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Deleted);

            var result = DESchemaObjectAdapter.Instance.Load(entity.ID, false) as DynamicEntity;

            Assert.IsTrue(result.Status == DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Deleted);

        }

        #region 辅助方法

        /// <summary>
        /// 创建实体字段
        /// </summary>
        /// <returns></returns>
        private DynamicEntityField createEntityField(string flag = "new")
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
        private DynamicEntity createEntity()
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
                Fields = new DynamicEntityFieldCollection(),
                Creator = (IUser)OguBase.CreateWrapperObject(new OguUser("22c3b351-a713-49f2-8f06-6b888a280fff")),//wangli5
                SortNo = 0
            };

            for (var i = 0; i < 2; i++)
            {
                var field = createEntityField();
                entity.Fields.Add(field);
            }
            return entity;
        }

        #endregion
    }
}
