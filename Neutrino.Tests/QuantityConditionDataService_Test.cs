using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Interfaces;
using Ninject.MockingKernel.Moq;
using Ninject;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Portal.Tests
{
    [TestClass]
    public class QuantityConditionDataService_Test
    {
        private readonly MoqMockingKernel _kernel;

        public QuantityConditionDataService_Test()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IQuantityConditionDS>().To<QuantityConditionDataService>();
            _kernel.Bind<IUnitOfWork>().To<NeutrinoUnitOfWork>();
        }

        [TestInitialize]
        public void Initialize_Test()
        {
            _kernel.Reset();
        }
        [TestMethod]
        public async Task GetQuantityConditionAsync()
        {
            //Arrange
            var quantityConditionDS = _kernel.Get<IQuantityConditionDS>();

            //Action
            var result = await quantityConditionDS.GetQuantityConditionAsync(3045);

            //Assert
            Assert.AreEqual(result.GoalId, 3045);

        }
    }
}
