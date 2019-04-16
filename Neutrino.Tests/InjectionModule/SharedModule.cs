using System.Data.Entity;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;
using Ninject.MockingKernel.Moq;
using Ninject.Web.Common;


namespace Neutrino.Portal.Tests
{
    //Shared Module For Application
    public class SharedModule 
    {
        public static void Bind(MoqMockingKernel _kernel)
        {
            _kernel.Bind<DbContext>().To<NeutrinoContext>().InRequestScope();
            _kernel.Bind<NeutrinoUnitOfWork>().ToSelf().InRequestScope();
            _kernel.Bind(typeof(IEntityRepository<>)).To(typeof(NeutrinoRepositoryBase<>));
            //_kernel.Bind(typeof(IEntityEraser<>)).To(typeof(SimpleEntityEraser<>));
            //_kernel.Bind(typeof(IEntityCreator<>)).To(typeof(SimpleEntityCreator<>));
            //_kernel.Bind(typeof(IEntityModifer<>)).To(typeof(SimpleEntityModifer<>));
            //_kernel.Bind(typeof(IEntityCounter<>)).To(typeof(GeneralEntityCounter<>));
            //_kernel.Bind(typeof(IEntityLoader<>)).To(typeof(GeneralEntityLoader<>));
            //_kernel.Bind(typeof(IEntityListLoader<>)).To(typeof(GeneralEntityListLoader<>));
            //_kernel.Bind(typeof(IEntityListByPagingLoader<>)).To(typeof(GeneralEntityListByPagingLoader<>));
        }
    }
}
