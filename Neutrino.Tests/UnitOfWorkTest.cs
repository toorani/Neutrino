using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.Entites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.WebApiControllers;
using Ninject;
using Ninject.MockingKernel.Moq;

namespace Neutrino.Portal.Tests
{
    [TestClass]
    public class UnitOfWorkTest
    {
        private readonly MoqMockingKernel _kernel;

        public UnitOfWorkTest()
        {
            _kernel = new MoqMockingKernel();
            //_kernel.Bind<NeutrinoContext>().ToSelf().InRequestScope();
            //_kernel.Bind<DbContext>().To<NeutrinoContext>().InRequestScope();
            //_kernel.Bind<NeutrinoUnitOfWork>().ToSelf().InRequestScope();
            //_kernel.Bind(typeof(IEntityRepository<>)).To(typeof(NeutrinoRepositoryBase<>));

            SharedModule.Bind(_kernel);
            MainModule.Bind(_kernel);
            EliteServicesModule.Bind(_kernel);
            EspressoModule.Bind(_kernel);


        }

        [TestInitialize]
        public void Initialize()
        {
            _kernel.Reset();
        }


        [TestMethod]
        public void ReturnGetQueryTest()
        {
            var uow = _kernel.Get<NeutrinoUnitOfWork>();
            var count = uow.AppSettingDataService.GetQuery().Count();
            Assert.AreEqual(count, uow.AppSettingDataService.GetCount());
        }

        [TestMethod]
        public void SameDbContextinUOWAndRepository()
        {
            var uow = _kernel.Get<NeutrinoUnitOfWork>();

            Goal goal = new Goal();
            goal.ComputingTypeId = ComputingTypeEnum.Amount;
            goal.GoalGoodsCategoryId = 5098;
            goal.GoalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.Group;
            goal.GoalTypeId = GoalTypeEnum.Distributor;
            //2019-01-20
            goal.StartDate = new System.DateTime(2018, 12, 22);
            goal.EndDate = new System.DateTime(2019, 1, 20);

            IGoalBS goalBusiness = _kernel.Get<IGoalBS>();
            //uow.GoalDataService.Insert(goal);
            //goalBusiness.CreateGoalAsync(goal);

            GoalServiceController goalServiceController = new GoalServiceController(goalBusiness);
            //await goalServiceController.Add(new Models.GoalViewModel()
            //{
            //    ComputingTypeId = 2,
            //    GoalGoodsCategoryId = 5098,
            //    GoalGoodsCategoryTypeId = 1,
            //    GoalTypeId = 1,
            //    //2019-01-20
            //    StartDate = "1397/10/01",
            //    EndDate = "1397/10/30"
            //});


            foreach (var item in uow.context.ChangeTracker.Entries())
            {

            }


            Assert.AreNotEqual(goal.Id, 0);
        }
        [TestMethod]
        public void CreateInastanceRewardTypeService()
        {
            ISimpleBusinessService<RewardType> businessService = _kernel.Get<ISimpleBusinessService<RewardType>>();
            //RewardTypeServiceController rewardTypeServiceController = new RewardTypeServiceController(businessService);

        }
    }
}
