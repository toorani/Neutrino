using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using Neutrino.External.Sevices;

namespace Neutrino.ServiceTests
{
    [TestClass]
    public class nLogTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public nLogTest()
        {
            logger.LoggerReconfigured += (sender, e) =>
            {

            };
        }

        

        [TestMethod]
        public void TestWarnLog()
        {
            logger.Warn(ExternalServices.BranchSales, "this is Warning");
        }

        [TestMethod]
        public void TestErrorLog()
        {
            logger.Error(ExternalServices.Company, "this is Error");
        }
        [TestMethod]
        public void TestFatalLog()
        {
            logger.Fatal("this is Fatal");
        }
        [TestMethod]
        public void TestInfolLog()
        {
            logger.Info(ExternalServices.Company, "this is Info");
        }
    }
}
