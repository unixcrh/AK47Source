using MCS.Library.Net.SNTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MCS.Library.Test.SNTP
{
    [TestClass]
    public class SNTPTest
    {
        [TestMethod]
        public void GetNowTest()
        {
            DateTime now = DateTime.Now;
            DateTime sntpNow = SNTPClient.GetNow();

            Console.WriteLine("Local Now: {0}, SNTP Now: {1}",
                now.ToString("yyyy-MM-dd HH:mm.ss.fff"),
                sntpNow.ToString("yyyy-MM-dd HH:mm.ss.fff"));
        }

        [TestMethod]
        public void LocalOffsetTest()
        {
            Console.WriteLine(SNTPClient.LocalOffset);
            Thread.Sleep(2000);
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(SNTPClient.LocalOffset);
            Console.WriteLine(SNTPClient.AdjustedLocalTime);
            Console.WriteLine(SNTPClient.AdjustedUtcTime);
        }
    }
}
