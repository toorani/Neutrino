using Espresso.Core;
using Espresso.Entites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Portal.Tests
{
    [TestClass]
    public class Commission_Test : UnitTestBase
    {
        public Commission_Test()
            : base()
        {

        }

        [TestMethod]
        public async Task AddPromotionWithSpecifiedDate()
        {
            //Arrange
            int month = 10;
            int year = 1397;

            //create a promotion entity
            Promotion entity = new Promotion
            {
                Month = month,
                Year = year
            };
            //promotion business
            var promotionBS = _kernel.Get<IPromotionBS>();

            //branchReceipt business
            var branchReceiptBS = _kernel.Get<IBranchReceiptBS>();

            //goalFulfillment business
            var goalFulfillmentBS = _kernel.Get<IFulfillmentPercentBS>();

            //BranchSalesBS business
            var branchSalesBS = _kernel.Get<IBranchSalesBS>();


            //Action
            var result_addpromotion = await promotionBS.AddPromotionAsync(entity);

            //var result_branchReceipt = await branchReceiptBS.LoadBranchReceiptListAsync(year, month);

            //var result_fulFillment = await goalFulfillmentBS.LoadFulfillmentListAsync(year, month);

            //var result_branchSales = await branchSalesBS.LoadTotalSalesPerBranchAsync(year, month);

            //Assert
            Assert.IsTrue(result_addpromotion.ReturnStatus, result_addpromotion.ReturnMessage.ConcatAll());
            //Assert.IsTrue(result_branchReceipt.ReturnStatus, result_branchReceipt.ReturnMessage.ConcatAll());
            //Assert.IsTrue(result_fulFillment.ReturnStatus, result_fulFillment.ReturnMessage.ConcatAll());
            //Assert.IsTrue(result_branchSales.ReturnStatus, result_branchSales.ReturnMessage.ConcatAll());

            Assert.AreNotEqual(result_addpromotion.ResultValue.Id, 0);
            Assert.AreNotEqual(result_addpromotion.ResultValue.BranchPromotions.Count, 0);

            //Assert.AreNotEqual(result_branchReceipt.ResultValue.Count, 0);

            //Assert.AreNotEqual(result_fulFillment.ResultValue.Count, 0);

            //Assert.AreEqual(result_addpromotion.ResultValue.BranchPromotions.Count, result_branchReceipt.ResultValue.Count);
            //Assert.AreEqual(result_branchReceipt.ResultValue.Count + result_branchSales.ResultValue.Count, result_fulFillment.ResultValue.Count);


        }
        [TestMethod]
        public async Task PutInProcessQueue()
        {
            //Arrange
            int month = 10;
            int year = 1397;


            //promotion business
            var promotionBS = _kernel.Get<IPromotionBS>();
            var result_load_entity = await promotionBS.EntityLoader.LoadAsync(x => x.Month == month && x.Year == year);
            var result_put = await promotionBS.PutInProcessQueueAsync(year, month);

            Assert.IsTrue(result_load_entity.ReturnStatus);

        }
        [TestMethod]
        public async Task CalculateSalesGoals()
        {
            //Arrange
            int month = 10;
            int year = 1397;


            //promotion business
            var promotionBS = _kernel.Get<IPromotionBS>();
            var result_load_entity = await promotionBS.EntityLoader.LoadAsync(x => x.Month == month && x.Year == year);



            var result = await promotionBS.CalculateSalesGoalsAsync(result_load_entity.ResultValue);

            Assert.IsTrue(result.ReturnStatus, result.ReturnMessage.ConcatAll());
        }
        [TestMethod]
        public async Task CalculateReceiptGoals()
        {
            //Arrange
            int month = 10;
            int year = 1397;


            //promotion business
            var promotionBS = _kernel.Get<IPromotionBS>();
            var result_load_entity = await promotionBS.EntityLoader.LoadAsync(x => x.Month == month && x.Year == year);
            if (result_load_entity.ReturnStatus == false)
            {
                Assert.Fail(result_load_entity.ReturnMessage.ConcatAll());
            }
            var result = await promotionBS.CalculateReceiptGoalsAsync(result_load_entity.ResultValue);

            Assert.IsTrue(result.ReturnStatus, result.ReturnMessage.ConcatAll());
        }
        [TestMethod]
        public async Task CalculatePromotion()
        {
            //Arrange
            int month = 10;
            int year = 1397;

            var promotionBS = _kernel.Get<IPromotionBS>();
            var loaderResult = await promotionBS.EntityLoader.LoadAsync(x => x.Month == month && x.Year == year);
            if (loaderResult.ReturnStatus == false)
            {
                Assert.Fail(loaderResult.ReturnMessage.ConcatAll());
            }
            var result = await promotionBS.CalculateGoalsAsync(loaderResult.ResultValue);

            Assert.IsTrue(result.ReturnStatus, result.ReturnMessage.ConcatAll());
            Assert.IsTrue(loaderResult.ResultValue.IsReceiptCalculated);
            Assert.IsTrue(loaderResult.ResultValue.IsSalesCalculated);
            Assert.AreEqual(loaderResult.ResultValue.StatusId, PromotionStatusEnum.GoalCalculated);
        }

        [TestMethod]
        public async Task LoadBranchPromotionDetail_Test()
        {
            //Arrange
            int branchId = 2397;


            var promotionBS = _kernel.Get<IPromotionBS>();
            var loaderResult = await promotionBS.LoadBranchPromotionDetail(branchId);
           
            loaderResult.ResultValue.Where(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Single)
                .ToList()
                .ForEach(x => x.GoalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.Group);
            var rs = loaderResult.ResultValue
                 .GroupBy(x => x.GoalGoodsCategoryTypeId
                 , (key, result) =>
                 {
                     return new BranchPromotionDetailViewModel
                     {
                         GoalTypeTitle = key == GoalGoodsCategoryTypeEnum.Group ? " هدف فروش" : key.GetEnumDescription(),
                         TotalFinalPromotion = result.Sum(x => x.FinalPromotion),
                         BranchId = result.FirstOrDefault().BranchId,
                         BranchName = result.FirstOrDefault().BranchName,
                         PositionPromotions = key != GoalGoodsCategoryTypeEnum.Group ? result.Select(x => new PositionPromotion
                         {
                             PositionTitle = x.PositionTitle,
                             Promotion = x.PositionPromotion.Value
                         }).ToList() : null
                     };
                 }).ToList();


            Assert.IsTrue(loaderResult.ReturnStatus, loaderResult.ReturnMessage.ConcatAll());

        }
    }
}
