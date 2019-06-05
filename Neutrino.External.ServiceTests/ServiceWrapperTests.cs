using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.External.Sevices;
using Neutrino.Interfaces;
using Ninject;
using Ninject.MockingKernel.Moq;

namespace Neutrino.ServiceTests
{
    [TestClass()]
    public class ServiceWrapperTests
    {
        private readonly MoqMockingKernel _kernel;

        #region [ Constructor(s) ]
        public ServiceWrapperTests()
        {
            _kernel = new MoqMockingKernel();

            _kernel.Bind<IUnitOfWork>().To<NeutrinoUnitOfWork>();
            _kernel.Bind(typeof(IEntityRepository<>)).To(typeof(RepositoryBase<>));
            _kernel.Bind(typeof(IEntityLoader<>)).To(typeof(GeneralEntityLoader<>));
            _kernel.Bind(typeof(IEntityListLoader<>)).To(typeof(GeneralEntityListLoader<>));
            _kernel.Bind(typeof(IEntityCreator<>)).To(typeof(SimpleEntityCreator<>));

            //BranchReceipt
            _kernel.Bind<IBranchReceiptBS>().To<BranchReceiptBS>();
            _kernel.Bind<IBranchReceiptDS>().To<BranchReceiptDataService>();
        }
        #endregion

        [TestMethod()]
        public void LoadBranchSalesTest()
        {
            List<Goods> lstGoods = new List<Goods>() {
                new Goods() { CompanyId = 3480, GoodsCode = "114101",Id = 31985,RefId = 22351,CompanyRefId= 770  },
                new Goods() { CompanyId = 3480, GoodsCode = "114102",Id = 31986,RefId = 22352,CompanyRefId= 770  }
            };

            var lstBranch = new List<Branch>()
            {
                new Branch() { RefId = 12,Id=2400 },
            };

            var reuslt = ServiceWrapper.Instance.LoadBranchSalesAsync(1396, 6, lstGoods, lstBranch).Result;
            Assert.AreNotEqual(reuslt.Count, 0);
            Assert.AreNotEqual(reuslt[0].TotalNumber, 0);
            
        }

        [TestMethod]
        public async Task NothingBranchReceipt_GetLatestYearMonth()
        {
            //Arrange
            var branchReceiptBS = _kernel.Get<IBranchReceiptBS>();

            //Action
            var lastMonthYear = await branchReceiptBS.LoadLatestYearMonthAsync();

            //Assert
            Assert.IsNotNull(lastMonthYear);
            Assert.IsTrue(lastMonthYear.ReturnStatus);
            Assert.AreEqual(lastMonthYear.ResultValue.Year, 0);
            Assert.AreEqual(lastMonthYear.ResultValue.Month, 0);


        }

    }
}