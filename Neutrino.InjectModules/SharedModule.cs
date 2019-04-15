using System.Data.Entity;
using System.Threading;
using System.Web;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;
using Ninject.Modules;
using Ninject.Web.Common;


namespace Neutrino.InjectModules
{
    //Shared Module For Application
    public class SharedModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IEntityRepository<>)).To(typeof(NeutrinoRepositoryBase<>));
            Bind(typeof(IEntityEraser<>)).To(typeof(SimpleEntityEraser<>));
            Bind(typeof(IEntityCreator<>)).To(typeof(SimpleEntityCreator<>));
            Bind(typeof(IEntityModifer<>)).To(typeof(SimpleEntityModifer<>));
            Bind(typeof(IEntityCounter<>)).To(typeof(GeneralEntityCounter<>));
            Bind(typeof(IEntityLoader<>)).To(typeof(GeneralEntityLoader<>));
            Bind(typeof(IEntityListLoader<>)).To(typeof(GeneralEntityListLoader<>));
            Bind(typeof(IEntityListByPagingLoader<>)).To(typeof(GeneralEntityListByPagingLoader<>));
            Bind(typeof(ISimpleBusinessService<>)).To(typeof(SimpleBusinessService<>));
        }
    }
}
