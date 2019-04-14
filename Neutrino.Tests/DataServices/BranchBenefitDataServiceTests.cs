using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Data.EntityFramework.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.MockingKernel.Moq;
using Espresso.DataAccess.Interfaces;
using Ninject;

namespace Neutrino.Data.EntityFramework.DataServices.Tests
{
    [TestClass()]
    public class BranchBenefitDataServiceTests
    {
        private readonly MoqMockingKernel _kernel;

        public BranchBenefitDataServiceTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUnitOfWork>().To<NeutrinoUnitOfWork>();
            _kernel.Bind<BranchGoalDataService>().ToSelf();
        }

        [TestMethod()]
        public async Task GetBranchBenefitListAsyncTest()
        {
            //Arrange
            var dataService = _kernel.Get<BranchGoalDataService>();

            //Action
            var result = await dataService.GetBranchGoalListAsync(4072);

        }
    }
}