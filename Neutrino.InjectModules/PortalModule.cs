using Neutrino.Data.EntityFramework;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Web.Common;
using System;
using System.Web;



namespace Neutrino.InjectModules
{
    //Shared Module For Application
    public class PortalModule : NinjectModule
    {
        public override void Load()
        {
            Bind<NeutrinoContext>().ToSelf().InRequestScope();
            Bind<NeutrinoUnitOfWork>().ToSelf().InRequestScope();
        }
    }
}
