using MCS.Library.Data;
using MCS.Library.Logging;
using MCS.Library.SOA.DataObjects.Schemas.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MCS.Library.SOA.DataObjects.Dynamics.Test
{
    /// <summary>
    /// UnitTest1 的摘要说明
    /// </summary>
    [TestClass]
    public class SettingTest
    {
        public SettingTest()
        {
            //
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

        #endregion

        [TestCategory("SettingTest"), TestMethod]
        public void ObjectSchemaSettingsTestMethod()
        {
            var setting = ObjectSchemaSettings.GetConfig();
            Assert.IsTrue(setting.Schemas.Count != 0, "ObjectSchemaSettings配置节出错！");
        }

        [TestCategory("Setting"), TestMethod]
        public void LoggingSectionTest()
        {
            var config = LoggingSection.GetConfig();

            Assert.IsTrue(config.Loggers != null);
        }

        [TestCategory("Setting"), TestMethod]
        public void GetConnectionStringTest()
        {
            var config = DbConnectionManager.GetConnectionString("DynamicsEntity");

            Assert.IsTrue(config != null);
        }

        [TestCategory("Setting"), TestMethod]
        public void SchemaObjectUpdateActionSettingsTest()
        {
            var deObjectSnapshotAction = SchemaObjectUpdateActionSettings.GetConfig().GetActions("DEObjectSnapshotAction");

            var deObjectUpdateStatusAction = SchemaObjectUpdateActionSettings.GetConfig().GetActions("DEObjectUpdateStatusAction");

            Assert.IsTrue(deObjectSnapshotAction != null && deObjectUpdateStatusAction != null);
        }

        [TestCategory("Setting"), TestMethod]
        public void ObjectSchemaSettingsTest()
        {
            var schemas = ObjectSchemaSettings.GetConfig().Schemas;

            Assert.IsTrue(schemas != null && schemas.Count != 0);
        }

        [TestCategory("Setting"), TestMethod]
        public void SchemaPropertyGroupSettingsTest()
        {
            var groupSettings = SchemaPropertyGroupSettings.GetConfig();

            Assert.IsTrue(groupSettings != null && groupSettings.Groups != null && groupSettings.Groups.Count > 0);
        }
    }
}
