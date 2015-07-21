using MCS.Library.Core;
using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Enums;
using MCS.Library.SOA.DataObjects.Dynamics.Instance;
using MCS.Library.SOA.DataObjects.Dynamics.Instance.ValueDefine;
using MCS.Library.SOA.DataObjects.Dynamics.Objects;
using MCS.Library.SOA.DataObjects.Dynamics.Test.Mock;
using MCS.Web.Library.Script;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MCS.Library.SOA.DataObjects.Dynamics.Test.Converter
{
    /// <summary>
    /// ConverterTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ConverterTest
    {
        public ConverterTest()
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

        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DESchemaObjectAdapter.Instance.ClearAllData();
            DESchemaObjectAdapter.Instance.InitAllData();
        }
        #endregion
        #region 单实体不带字表序列化和反序列化

        [TestCategory("EntitySerialize"), TestMethod]
        public void EntitySerialize()
        {
            var flag = true;

            DynamicEntity sourceEntity = MockData.CreateEntityWithReferenceEntity();
            string json = JSONSerializerExecute.Serialize(sourceEntity);

            sourceEntity.Fields.ForEach(f =>
            {
                if (!json.Contains(f.Name))
                {
                    flag = false;
                }
            });


            Assert.IsTrue(flag, "序列化实体出错");
        }

        [TestCategory("EntitySerialize"), TestMethod]
        public void EntityCollectionSerialize()
        {
            var flag = true;

            try
            {
                DynamicEntityCollection sourceEntity = MockData.CreateRelationDynamicEntityCollection();
                string json = JSONSerializerExecute.Serialize(sourceEntity);

                sourceEntity.ForEach(e => e.Fields.ForEach(f =>
                {
                    if (!json.Contains(f.Name))
                    {
                        flag = false;
                    }
                }));
            }
            catch (Exception)
            {
                flag = false;
            }

            Assert.IsTrue(flag, "序列化实体集合出错");
        }

        [TestCategory("EntitySerialize"), TestMethod]
        public void EntityDeSerialize()
        {
            var flag = true;

            try
            {
                DynamicEntity sourceEntity = MockData.CreateEntityWithReferenceEntity();
                string json = JSONSerializerExecute.Serialize(sourceEntity);

                DynamicEntity targetEntity = JSONSerializerExecute.Deserialize<DynamicEntity>(json);

                if (targetEntity == null || targetEntity.Fields == null ||
                    sourceEntity.Fields.Count != targetEntity.Fields.Count)
                {
                    flag = false;
                }
                else
                {
                    for (var i = 0; i < sourceEntity.Fields.Count; i++)
                    {
                        if (sourceEntity.Fields[i].Name != targetEntity.Fields[i].Name)
                        {
                            flag = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }

            Assert.IsTrue(flag, "反序列化实体出错");
        }
        #endregion

        #region 单实体带字标的序列化和反序列化
        [TestCategory("EntitySerialize"), TestMethod]
        public void EntityAndChildSerialize()
        {
            DynamicEntity entity = MockData.CreateEntityAndChildEntity();
            string json = JSONSerializerExecute.Serialize(entity);
            bool result = true;
            var entitys = entity.Fields.Where(p => p.FieldType == FieldTypeEnum.Collection);
            entitys.ForEach(p =>
            {
                if (!json.Contains(p.CodeName))
                {
                    result = false;
                }

            });
            if (!json.Contains(entity.CodeName))
            {
                result = false;
            }
            Assert.IsTrue(result, "序列化失败");
        }

        [TestCategory("EntitySerialize"), TestMethod]
        public void EntityAndChildDeSerialize()
        {
            DynamicEntity entity = MockData.CreateEntityAndChildEntity();
            string json = JSONSerializerExecute.Serialize(entity);
            DynamicEntity deDynamicEntity = JSONSerializerExecute.Deserialize<DynamicEntity>(json);
            bool result = true;
            string resultMessage = string.Empty;
            result = deDynamicEntity.CodeName.Equals(entity.CodeName);
            deDynamicEntity.Fields.Where(p => p.FieldType == FieldTypeEnum.Collection).ForEach(p =>
            {
                if (!entity.Fields.Where(s => s.FieldType == FieldTypeEnum.Collection && s.ReferenceEntity.CodeName == p.ReferenceEntity.CodeName).Any())
                {
                    resultMessage += p.Entity.CodeName;
                    result = false;
                }

            });
            Assert.IsTrue(result, "名字不一样 序列化失败了" + resultMessage);

        }

        #endregion

        //序列化实体实例
        [TestCategory("InstanceSerialize"), TestMethod]
        public void InstanceSerialize()
        {
            var flag = true;

            DynamicEntity entity = MockData.CreateEntityAndChildEntity();

            DEEntityInstanceBase instance = entity.CreateInstance();

            string json = JSONSerializerExecute.Serialize(instance);

            instance.Fields.ForEach(f =>
            {
                if (f.Definition.FieldType == FieldTypeEnum.Collection)
                {
                    foreach (var item in f.Definition.ReferenceEntity.Fields)
                    {
                        if (!json.Contains(item.Name))
                        {
                            flag = false;
                        }
                    }
                }
                if (!json.Contains(f.Definition.Name))
                {
                    flag = false;
                }
            });

            Assert.IsTrue(flag, "序列化实体实例出错");
        }

        //序列化实体实例，带数据
        [TestCategory("InstanceSerialize"), TestMethod]
        public void InstanceSerializeWithData()
        {
            bool flag = true;

            DEEntityInstanceBase instance = MockData.CreateEntityInstance();

            string json = JSONSerializerExecute.Serialize(instance);

            instance.Fields.ForEach(f =>
            {
                if (f.Definition.FieldType == FieldTypeEnum.Collection)
                {
                    foreach (var item in f.Definition.ReferenceEntity.Fields)
                    {
                        if (!json.Contains(item.Name) || !json.Contains(item.Name + "Value"))
                        {
                            flag = false;
                        }
                    }
                }
                else if (!json.Contains(f.Definition.Name) || !json.Contains(f.Definition.Name + "Value"))
                {
                    flag = false;
                }
            });

            Assert.IsTrue(flag, "序列化实体实例出错");
        }

        //反序列化实体实例
        [TestCategory("InstanceSerialize"), TestMethod]
        public void InstanceDeserialize()
        {
            var flag = true;

            DynamicEntity entity = MockData.CreateEntityAndChildEntity();

            DEEntityInstanceBase instance = entity.CreateInstance();

            //序列化之后的数据
            string json = JSONSerializerExecute.Serialize(instance);

            //反序列化
            DEEntityInstanceBase resultInstance = JSONSerializerExecute.Deserialize<DEEntityInstanceBase>(json);

            foreach (var field in instance.Fields)
            {
                if (!resultInstance.Fields.Where(p => p.Definition.Name == field.Definition.Name).Any())
                {
                    flag = false;
                }
                else
                {
                    if (field.Definition.FieldType == FieldTypeEnum.Collection)
                    {
                        EntityFieldValue entityFieldValue = resultInstance.Fields.Where(p => p.Definition.Name == field.Definition.Name).FirstOrDefault();
                        if (entityFieldValue.Definition.ReferenceEntityCodeName != field.Definition.ReferenceEntityCodeName)
                        {
                            flag = false;
                        }
                    }
                }
            }
            Assert.IsTrue(flag, "反序列化实体实例出错");
        }

        //反序列化实体实例，带数据
        [TestCategory("InstanceSerialize"), TestMethod]
        public void InstanceDeserializeWithData()
        {
            bool flag = true;

            DEEntityInstanceBase instance = MockData.CreateEntityInstance();

            string json = JSONSerializerExecute.Serialize(instance);

            DEEntityInstanceBase resultInstance = JSONSerializerExecute.Deserialize<DEEntityInstanceBase>(json);

            foreach (var field in resultInstance.Fields)
            {
                if (field.Definition.FieldType == FieldTypeEnum.Collection)
                {
                    DEEntityInstanceBaseCollection children = field.GetRealValue() as DEEntityInstanceBaseCollection;
                    foreach (var child in children)
                    {
                        foreach (var childField in child.Fields)
                        {
                            if (!childField.StringValue.Contains(childField.Definition.Name + "Value"))
                            {
                                flag = false;
                            }
                        }
                    }

                }
                else
                {
                    if (field.StringValue != field.Definition.Name + "Value")
                    {
                        flag = false;
                    }
                }
            }

            Assert.IsTrue(flag, "序列化实体实例出错");
        }
    }
}
