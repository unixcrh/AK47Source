using MCS.Library.SOA.DataObjects.Dynamics.Adapters;
using MCS.Library.SOA.DataObjects.Dynamics.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace MCS.Library.SOA.DataObjects.Dynamics.Test.SchemaDefine
{
    /// <summary>
    /// SchemaDefineTest 的摘要说明
    /// </summary>
    [TestClass]
    public class SchemaDefineTest
    {
        public SchemaDefineTest()
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

        /// <summary>
        /// 带SortNo的对象构造测试
        /// </summary>
        [TestMethod]
        public void CreateObjectWithSortNo()
        {
            try
            {
                //var assembly = Assembly.Load("MCS.Library.SOA.DataObjects.Dynamics.Objects.DynamicEntityField, MCS.Library.SOA.DataObjects.Dynamics");
                //var assembly = Assembly.GetAssembly();
                var type = typeof(DynamicEntityField);

                BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;


                var aaaa = Activator.CreateInstance(type, bf, null, new object[0], null);

                var bbbb = Activator.CreateInstance(type, new object[0]);



                DynamicEntityField entity = SchemaExtensions.CreateObject("DynamicEntityField") as DynamicEntityField;

                int length = entity.Length;

                Assert.IsTrue(entity.SortNo == 0);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /*
        /// <summary>
        /// 获取ETLEntity实体信息
        /// </summary>
        [TestMethod]
        public void CreateETLEntityObject()
        {
            try
            {
                //string str = Guid.NewGuid().ToString();
                //ETLEntity eTLentity = SchemaExtensions.CreateObject("ETLEntity") as ETLEntity;
                //SetValue(eTLentity);
                //Assert.AreEqual(eTLentity.ServerAddress, "aa");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void CreateETLEntityFieldObject()
        {
            try
            {
                //ETLEntityField eTLentity = SchemaExtensions.CreateObject("ETLEntityField") as ETLEntityField;
                //SetValue(eTLentity);
                //Assert.AreEqual(eTLentity.RefTable, "aa");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void CreateOutETLEntityObject()
        {
            try
            {
                //OuterETLEntity eTLentity = SchemaExtensions.CreateObject("OuterETLEntity") as OuterETLEntity;
                //eTLentity.Name = "测试的表名";
                //SetValue(eTLentity);
                //Assert.AreEqual(eTLentity.Name, "aa");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void CreateOutETLEntityFieldObject()
        {
            try
            {
                //OuterETLEntityField eTLentity = SchemaExtensions.CreateObject("OuterETLEntityField") as OuterETLEntityField;
                ////eTLentity.IsPrimaryKey = true;
                //eTLentity.Name = "测试的name";
                //SetValue(eTLentity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool SetValue(object obj)
        {
            try
            {
                Type typeObj = obj.GetType();

                PropertyInfo[] pros = typeObj.GetProperties();

                foreach (PropertyInfo pro in pros)
                {
                    if (pro.PropertyType == typeof(string))
                    {
                        pro.SetValue(obj, "aa", null);
                    }
                    if (pro.PropertyType == typeof(bool))
                    {
                        pro.SetValue(obj, true, null);
                    }
                    if (pro.PropertyType == typeof(int))
                    {
                        pro.SetValue(obj, 12, null);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        */
    }
}
