using MCS.Library.OGUPermission;
using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Enums;
using MCS.Library.SOA.DataObjects.Dynamics.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MCS.Library.SOA.DataObjects.Dynamics.Test.Adapter
{
    [TestClass]
    public class SchemaObjectAdapterTest
    {
        public SchemaObjectAdapterTest() 
        {
        }

        //在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DESchemaObjectAdapter.Instance.ClearAllData();
            DESchemaObjectAdapter.Instance.InitAllData();
        }

        #region 操作实体
        [TestCategory("SchemaObjectAdapter"), TestMethod]
        public void TestSchemaObjectAdapterAddEntityMethod()
        {
            var newEntity = createEntity();

            DESchemaObjectAdapter.Instance.Update(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID);

            Assert.IsTrue(loadEntity != null, "实体添加失败！");
        }

        [TestCategory("SchemaObjectAdapter"), TestMethod]
        public void TestSchemaObjectAdapterUpdateEntityMethod()
        {
            var newEntity = createEntity();

            DESchemaObjectAdapter.Instance.Update(newEntity);

            //重新设置时间点
            DBTimePointActionContext.Clear();

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            loadEntity.Description = "Update Entity";

            DESchemaObjectAdapter.Instance.Update(loadEntity);

            var resultEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            Assert.IsTrue(resultEntity.Description == "Update Entity", "实体更新失败！");
        }

        [TestCategory("SchemaObjectAdapter"), TestMethod]
        public void TestSchemaObjectAdapterLoadEntityMethod()
        {
            var newEntity = createEntity();

            DESchemaObjectAdapter.Instance.Update(newEntity);

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID) as DynamicEntity;

            Assert.IsTrue(loadEntity != null, "实体加载失败！");
        }


        [TestCategory("SchemaObjectAdapter"), TestMethod]
        public void TestSchemaObjectAdapterDeleteEntityMethod()
        {
            var newEntity = createEntity();

            DESchemaObjectAdapter.Instance.Update(newEntity);

            //重新设置时间点
            DBTimePointActionContext.Clear();

            var loadEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID);

            DESchemaObjectAdapter.Instance.UpdateStatus(loadEntity, DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Deleted);

            var resultEntity = DESchemaObjectAdapter.Instance.Load(newEntity.ID, false);

            Assert.IsTrue(resultEntity.Status == DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Deleted, "实体删除失败！");
        }
        #endregion

        #region 操作实体属性
        [TestCategory("SchemaObjectAdapter"), TestMethod]
        public void TestSchemaObjectAdapterAddFieldMethod()
        {
            var newEntityField = createEntityField();

            DESchemaObjectAdapter.Instance.Update(newEntityField);

            var loadEntityField = DESchemaObjectAdapter.Instance.Load(newEntityField.ID);

            Assert.IsTrue(loadEntityField != null, "实体属性添加失败！");
        }

        [TestCategory("SchemaObjectAdapter"), TestMethod]
        public void TestSchemaObjectAdapterUpdateFieldMethod()
        {
            var newEntityField = createEntityField();

            DESchemaObjectAdapter.Instance.Update(newEntityField);

            //重新设置时间点
            DBTimePointActionContext.Clear();

            var loadEntityField = DESchemaObjectAdapter.Instance.Load(newEntityField.ID);

            loadEntityField.Status = DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Unspecified;

            DESchemaObjectAdapter.Instance.Update(loadEntityField);

            var resultEntityField = DESchemaObjectAdapter.Instance.Load(loadEntityField.ID, false);

            Assert.IsTrue(resultEntityField.Status == DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Unspecified, "实体属性更新失败！");
        }

        [TestCategory("SchemaObjectAdapter"), TestMethod]
        public void TestSchemaObjectAdapterLoadFieldMethod()
        {
            var newEntityField = createEntityField();

            DESchemaObjectAdapter.Instance.Update(newEntityField);

            var loadEntityField = DESchemaObjectAdapter.Instance.Load(newEntityField.ID) as DynamicEntityField;

            Assert.IsTrue(loadEntityField != null, "实体属性加载失败！");
        }


        [TestCategory("SchemaObjectAdapter"), TestMethod]
        public void TestSchemaObjectAdapterDeleteFieldMethod()
        {
            var newEntityField = createEntityField();

            DESchemaObjectAdapter.Instance.Update(newEntityField);

            //重新设置时间点
            DBTimePointActionContext.Clear();//.Current.TimePoint = DateTime.MinValue;

            var loadEntityField = DESchemaObjectAdapter.Instance.Load(newEntityField.ID) as DynamicEntityField;

            loadEntityField.Name = "changed Name";

            DESchemaObjectAdapter.Instance.UpdateStatus(loadEntityField, DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Deleted);

            var resultEntityField = DESchemaObjectAdapter.Instance.Load(loadEntityField.ID, false);

            Assert.IsTrue(resultEntityField.Status == DataObjects.Schemas.SchemaProperties.SchemaObjectStatus.Deleted, "实体属性删除失败！");

        }
        #endregion


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
                FieldType = FieldTypeEnum.String,
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
