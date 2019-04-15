using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Utilities.AppSettingManagement;
using Espresso.Utilities.Interfaces;
using Ninject.MockingKernel.Moq;
using Ninject.Modules;

namespace Neutrino.Portal.Tests
{
    public static class EspressoModule 
    {
        public static void Bind(MoqMockingKernel _kernel)
        {
            _kernel.Bind(typeof(ISimpleBusinessService<>)).To(typeof(SimpleBusinessService<>));
            //Bind(typeof(IEntityRepository<>)).To(typeof(RepositoryBase<>));

            _kernel.Bind<IBusinessResult>().To<BusinessResult>();
            _kernel.Bind<IAppSettingManager>().To<AppSettingManager>().InThreadScope();
            
           
        }
    }
}
