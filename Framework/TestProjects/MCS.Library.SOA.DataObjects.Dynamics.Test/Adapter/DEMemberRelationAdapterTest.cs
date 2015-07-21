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
    /// DEMemberRelationAdapterTest 的摘要说明
    /// </summary> 
    [TestClass]
    public class DEMemberRelationAdapterTest
    {
        public DEMemberRelationAdapterTest()
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
        //
        //在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DESchemaObjectAdapter.Instance.ClearAllData();
            DESchemaObjectAdapter.Instance.InitAllData();
        }
        #endregion

        [TestCategory("DEMemberRelationAdapter"), TestMethod]
        public void LoadByMemberIDTest()
        {
            var newEntity = createEntity();

            var fieldDesc = "New Field Description";

            newEntity.Fields[0].Description = fieldDesc;

            var memberID = newEntity.Fields[0].ID;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            var relation = DEMemberRelationAdapter.Instance.LoadByMemberID(memberID).FirstOrDefault();

            Assert.IsTrue(((DynamicEntityField)relation.Member).Description.Equals(fieldDesc));
        }

        [TestCategory("DEMemberRelationAdapter"), TestMethod]
        public void LoadByMemberIDAndContainerSchemaTypeTest()
        {
            var newEntity = createEntity();

            var fieldDesc = "New Field Description";

            newEntity.Fields[0].Description = fieldDesc;

            var memberID = newEntity.Fields[0].ID;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            var relation = DEMemberRelationAdapter.Instance.LoadByMemberID(memberID, "DynamicEntity").FirstOrDefault();

            Assert.IsTrue(((DynamicEntityField)relation.Member).Description.Equals(fieldDesc));
        }

        [TestCategory("DEMemberRelationAdapter"), TestMethod]
        public void LoadByMemberIDAndTimeTest()
        {
            var newEntity = createEntity();

            var fieldDesc = "New Field Description";

            newEntity.Fields[0].Description = fieldDesc;

            var memberID = newEntity.Fields[0].ID;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            var relation = DEMemberRelationAdapter.Instance.LoadByMemberID(memberID, DateTime.MinValue).FirstOrDefault();

            Assert.IsTrue(((DynamicEntityField)relation.Member).Description.Equals(fieldDesc));
        }

        [TestCategory("DEMemberRelationAdapter"), TestMethod]
        public void LoadByContainerIDTest()
        {

            var newEntity = createEntity();

            var description = "New Description";

            newEntity.Description = description;

            var containerID = newEntity.ID;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            var relations = DEMemberRelationAdapter.Instance.LoadByContainerID(containerID);

            var relation = relations.FirstOrDefault();

            Assert.IsTrue(((DynamicEntity)relation.Container).Description.Equals(description));

        }


        [TestCategory("DEMemberRelationAdapter"), TestMethod]
        public void LoadByContainerIDAndMemberSchemaTypeTest()
        {

            var newEntity = createEntity();

            var description = "New Description";

            newEntity.Description = description;

            var containerID = newEntity.ID;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            var relations = DEMemberRelationAdapter.Instance.LoadByContainerID(containerID, "DynamicEntityField");

            var relation = relations.FirstOrDefault();

            Assert.IsTrue(((DynamicEntity)relation.Container).Description.Equals(description));

        }

        [TestCategory("DEMemberRelationAdapter"), TestMethod]
        public void LoadByContainerIDAndTimeTest()
        {

            var newEntity = createEntity();

            var description = "New Description";

            newEntity.Description = description;

            var containerID = newEntity.ID;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            var relations = DEMemberRelationAdapter.Instance.LoadByContainerID(containerID, DateTime.MinValue);

            var relation = relations.FirstOrDefault();

            Assert.IsTrue(((DynamicEntity)relation.Container).Description.Equals(description));

        }

        [TestCategory("DEMemberRelationAdapter"), TestMethod]
        public void LoadByID()
        {

            var newEntity = createEntity();

            var description = "New Description";

            var fieldDescription = "New Field Description";

            newEntity.Description = description;

            newEntity.Fields[0].Description = fieldDescription;

            var memberId = newEntity.Fields[0].ID;

            var containerID = newEntity.ID;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            var relation = DEMemberRelationAdapter.Instance.Load(containerID, memberId);

            Assert.IsTrue(relation.Container.Properties["Description"].StringValue.Equals(description) && relation.Member.Properties["Description"].StringValue.Equals(fieldDescription));

        }

        [TestCategory("DEMemberRelationAdapter"), TestMethod]
        public void LoadByIDAndTime()
        {

            var newEntity = createEntity();

            var description = "New Description";

            var fieldDescription = "New Field Description";

            newEntity.Description = description;

            newEntity.Fields[0].Description = fieldDescription;

            var memberId = newEntity.Fields[0].ID;

            var containerID = newEntity.ID;

            DEObjectOperations.InstanceWithoutPermissions.AddEntity(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            var relation = DEMemberRelationAdapter.Instance.Load(containerID, memberId, DateTime.MinValue);

            Assert.IsTrue(relation.Container.Properties["Description"].StringValue.Equals(description) && relation.Member.Properties["Description"].StringValue.Equals(fieldDescription));

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
