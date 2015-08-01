using MCS.Library.Core;
using MCS.Library.Data.Mapping;
using MCS.Library.Data.Test.DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MCS.Library.Data.Test
{
    [TestClass]
    public class MappingTest
    {
        [TestMethod]
        public void PrimaryKeyValuePairsTest()
        {
            TestObject data = new TestObject();

            data.ID = UuidHelper.NewUuidString();

            Dictionary<string, object> pairs = ORMapping.GetPrimaryKeyValuePairs(data);

            Assert.AreEqual(data.ID, pairs["ID"]);
        }
    }
}
