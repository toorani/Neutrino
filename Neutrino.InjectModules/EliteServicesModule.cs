using Neutrino.Data.EntityFramework;
using Ninject.Modules;

namespace Neutrino.InjectModules
{
    //Main Module For Application
    public class EliteServicesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<NeutrinoContext>().ToSelf().InThreadScope();
            Bind<NeutrinoUnitOfWork>().ToSelf().InThreadScope();
        }
    }
}
