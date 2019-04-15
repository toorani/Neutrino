using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Ninject.MockingKernel.Moq;
using Ninject.MockingKernel;
using Ninject;
using Neutrino.Portal.App_Start;

namespace Neutrino.Portal.Tests
{
    [TestClass]
    public class SimpleBusinessServiceTest
    {

        private readonly IKernel _kernel;
        


        public SimpleBusinessServiceTest()
        {
            _kernel = NinjectWebCommon.CreateKernel();


            //_kernel = new MoqMockingKernel();
            //_kernel.Bind<IUnitOfWork>().To<NeutrinoUnitOfWork>();
            //_kernel.Bind<IDataRepository>().To<RepositoryBase>();
            //_kernel.Bind<IBusinessActionResult>().To<BusinessActionResult>();
            //_kernel.Bind(typeof(ISimpleBusinessService<>)).ToMock(typeof(SimpleBusinessService<>));
    
        }

        [TestInitialize]
        public void ApiTestInitialize()
        {
            //_kernel.Reset();
        }

        [TestMethod]
        public void SimpleBusinessServiceNotNullInInjectionTest()
        {
            //Arrange
            //var qcBizService = _kernel.GetMock<ISimpleBusinessService<QuantityCondition>>();
            var qcBizService = _kernel.Get<ISimpleBusinessService<QuantityCondition>>();
            _kernel.Release(qcBizService);
            //Assert
            Assert.IsNotNull(qcBizService);
        }

        [TestMethod]
        public void RulesServiceNotNullInSimpleBusinessWithInjectionTest()
        {
            //Arrange
            _kernel.Bind(typeof(FluentValidation.AbstractValidator<Entities.QuantityCondition>)).To(typeof(Business.QuantityConditionBusinessRules));
            var qcBizService = _kernel.Get<ISimpleBusinessService<QuantityCondition>>();
            _kernel.Release(qcBizService);
            //Action

            //Assert
            Assert.IsNotNull(qcBizService.RulesService);
        }

        [TestMethod]
        public void RulesServiceIsNullInSimpleBusinessWithOutInjectionTest()
        {
            //Arrange

            var qcBizService = _kernel.Get<ISimpleBusinessService<QuantityCondition>>();
           
            //Action

            //Assert
            
            Assert.IsNull(qcBizService.RulesService);
        }

        [TestMethod]
        public void ILoggerNotNullInSimpleBusinessWithInjectionExtensionTest()
        {
            //Arrange

            var qcBizService = _kernel.Get<ISimpleBusinessService<QuantityCondition>>();

            //Action

            //Assert

            Assert.IsNotNull(qcBizService.Logger);
        }
    }
}
