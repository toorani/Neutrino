using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Entities;
using Ninject;
using Neutrino.Interfaces;
using Ninject.MockingKernel.Moq;
using Ninject.MockingKernel;
using Neutrino.Business;
using Espresso.BusinessService.Interfaces;
using Espresso.BusinessService;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using System.Threading.Tasks;
using Neutrino.Portal.WebApiControllers;
using System.Web;
using Moq;
using System.Net.Http;
using Neutrino.Portal.Models;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Portal.Tests
{
    [TestClass]
    public class QuantityConditionServiceController_Test
    {
        private readonly MoqMockingKernel _kernel;

        public QuantityConditionServiceController_Test()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind(typeof(IBusinessResultValue<>)).To(typeof(BusinessResultValue<>));
            _kernel.Bind<IQuantityConditionBS>().To<QuantityConditionBS>();
            _kernel.Bind<IQuantityConditionDS>().To<QuantityConditionDataService>();
            _kernel.Bind<IUnitOfWork>().To<NeutrinoUnitOfWork>();

        }

        [TestInitialize]
        public void Initialize_Test()
        {
            _kernel.Reset();
        }

        [TestMethod]
        public async Task GetQuantityConditionAsync_CreateMockData_ByCreateInstance_Test()
        {
            //Arrange
            QuantityCondition quantityCondition = new QuantityCondition();
            quantityCondition.Goal = new Goal()
            {
                Id = 10,
                GoalGoodsCategory = new GoalGoodsCategory()
                {
                    Name = "GoalGoodsCategory_Test"
                }
            };
            quantityCondition.GoalId = 10;
            quantityCondition.Quantity = 20;
            quantityCondition.ExtraEncouragePercent = 1.3M;


            GoodsQuantityCondition qoodsQuantityCondition = new GoodsQuantityCondition();
            qoodsQuantityCondition.Quantity = 30;
            qoodsQuantityCondition.Goods = new Goods()
            {
                EnName = "Drug No 1"
            };

            qoodsQuantityCondition.BranchQuantityConditions.Add(new BranchQuantityCondition()
            {
                Quantity = 40,
                Branch = new Branch()
                {
                    Name = "Branch 1"
                }
            });

            qoodsQuantityCondition.BranchQuantityConditions.Add(new BranchQuantityCondition()
            {
                Quantity = 50,
                Branch = new Branch()
                {
                    Name = "Branch 2"
                }
            });

            quantityCondition.GoodsQuantityConditions.Add(qoodsQuantityCondition);

            var quantityConditionBusinessService = _kernel.GetMock<IQuantityConditionBS>();
            var businessResultValue = _kernel.Get<IBusinessResultValue<QuantityCondition>>();
            businessResultValue.ReturnStatus = true;
            businessResultValue.ResultValue = quantityCondition;
            quantityConditionBusinessService.Setup(x => x.LoadQuantityConditionAsync(It.IsAny<int>()))
               .ReturnsAsync(businessResultValue);


            QuantityConditionServiceController controller = new QuantityConditionServiceController(quantityConditionBusinessService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new System.Web.Http.HttpConfiguration());
            //Act
            var result = await controller.GetQuantityConditionAsync(10);
            QuantityConditionViewModel vModel = new QuantityConditionViewModel();
            bool checkGetResult = result.TryGetContentValue(out vModel);

            //Assert
            Assert.IsTrue(checkGetResult);
            Assert.AreEqual(vModel.GoalCategoryName, quantityCondition.Goal.GoalGoodsCategory.Name);
            Assert.AreEqual(vModel.GoodsQuantityConditions.Count, quantityCondition.GoodsQuantityConditions.Count);

        }

        [TestMethod]
        public async Task GetQuantityConditionAsync_RealData_ByCreateInstance_Test()
        {
            //Arrange
            
            var quantityConditionBS = _kernel.Get<IQuantityConditionBS>();

            QuantityConditionServiceController controller = new QuantityConditionServiceController(quantityConditionBS);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new System.Web.Http.HttpConfiguration());

            //Act
            var result = await controller.GetQuantityConditionAsync(3045);
            QuantityConditionViewModel vModel = new QuantityConditionViewModel();
            bool checkGetResult = result.TryGetContentValue(out vModel);

            //Assert
            Assert.IsTrue(checkGetResult);
            Assert.AreEqual(vModel.GoalCategoryName, "16 قلم اکتور کو");
            Assert.AreEqual(vModel.GoodsQuantityConditions.Count, 15);
            Assert.AreEqual(vModel.GoodsQuantityConditions[0].BranchQuantityConditions.Count, 25);
            Assert.AreEqual(vModel.GoodsQuantityConditions[0].QuantityConditionId, vModel.Id);
            Assert.AreEqual(vModel.GoodsQuantityConditions[0].BranchQuantityConditions[0].GoodsQuantityConditionId, vModel.GoodsQuantityConditions[0].Id);
        }
    }
}
