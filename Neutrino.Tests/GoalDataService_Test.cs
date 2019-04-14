using System;
using Espresso.DataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Data.EntityFramework;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Interfaces;
using Ninject;
using Ninject.MockingKernel.Moq;

namespace Neutrino.Portal.Tests
{
    [TestClass]
    public class GoalDataService_Test
    {
        private readonly MoqMockingKernel _kernel;

        public GoalDataService_Test()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IGoalDS>().To<GoalDataService>();
            _kernel.Bind<IUnitOfWork>().To<NeutrinoUnitOfWork>();
        }

        [TestMethod]
        public void GetGeneralGoal_NotLoadGoalGeneral_Deleted()
        {
            //Arrange
            var dataService = _kernel.Get<IGoalDS>();

            //Action
            var goal = dataService.GetGoalAync(3044);
            //Assert
            
        }
    }
}
