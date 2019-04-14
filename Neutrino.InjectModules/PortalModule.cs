using System;
using System.Data.Entity;
using System.Threading;
using System.Web;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Business;
using Neutrino.Core.SecurityManagement;
using Neutrino.Data.EntityFramework;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Web.Common;



namespace Neutrino.InjectModules
{
    //Shared Module For Application
    public class PortalModule : NinjectModule
    {
        public override void Load()
        {
            Bind<NeutrinoContext>().ToSelf().InRequestScope();
            Bind<NeutrinoUnitOfWork>().ToSelf().InRequestScope();
            //Bind<IPermissionManager>().To<PermissionManager>().InSessionScope();
            Bind<IPermissionManager>().ToProvider<PermissionManagerProvider>();

        }
    }

    class PermissionManagerProvider : Provider<PermissionManager>
    {
        private const string permissionMgr = "SES_permissionMgr";

        protected override PermissionManager CreateInstance(IContext context)
        {
            // Retrieve services from kernel
            HttpContextBase httpContext = context.Kernel.Get<HttpContextBase>();

            if (httpContext == null || httpContext.Session == null)
            {
                throw new Exception("No bind service found in Kernel for HttpContextBase");
            }

            return (httpContext.Session[permissionMgr] ?? (
                    httpContext.Session[permissionMgr] = new PermissionManager(context.Kernel.Get<NeutrinoUnitOfWork>()))
                ) as PermissionManager;
        }
    }
}
