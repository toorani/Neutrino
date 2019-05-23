using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Interfaces;
using Ninject.MockingKernel.Moq;
using Ninject;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Data.EntityFramework;
using Espresso.Core;

namespace Neutrino.Portal.Tests
{
    [TestClass]
    public class ReportPrmotion_Test : UnitTestBase
    {
        public ReportPrmotion_Test() : base()
        {
        }
        
        //[TestMethod]
        //public async Task LoadReportBranchSalesGoal()
        //{
        //    //Arrange
        //    var promotionBS = _kernel.Get<IPromotionBS>();
        //    DateTime startDate = new DateTime(2018, 12, 22);
        //    DateTime endDate = new DateTime(2019, 01, 20);
        //    int goalGoodsCategoryId = 5093; // جالینوس
        //    //Action
        //    var result = await promotionBS.LoadReportBranchSalesGoal(startDate,endDate,goalGoodsCategoryId);
        //    if (result.ReturnStatus == false)
        //    {
        //        Assert.Fail(result.ReturnMessage.ConcatAll());
        //    }
        //    Assert.IsTrue(result.ReturnStatus);
        //}
    }
}
