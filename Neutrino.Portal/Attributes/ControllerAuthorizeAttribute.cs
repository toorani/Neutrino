using System;
using System.Web;
using System.Web.Mvc;
using Espresso.Core.Ninject.Http;
using Espresso.Utilities.Interfaces;

namespace Neutrino.Portal.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ControllerAuthorizeAttribute : AuthorizeAttribute
    {

        private IAppSettingManager appSettingManager;

        #region [ Varibale(s) ]

        #endregion

        #region [ Constructor(s) ]
        public ControllerAuthorizeAttribute()
        {
            //appSettingManager = NinjectHttpContainer.Resolve<IAppSettingManager>();
        }
        #endregion

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            appSettingManager = NinjectHttpContainer.Resolve<IAppSettingManager>();
            if (getCheckAccess().Value)
            {
                return base.AuthorizeCore(httpContext);
            }
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            appSettingManager = NinjectHttpContainer.Resolve<IAppSettingManager>();
            if (getCheckAccess().Value)
            {
                base.OnAuthorization(filterContext);
            }
        }

        private bool? getCheckAccess()
        {
            bool? checkAccess = appSettingManager.GetValue<bool>("checkAccess");
            return checkAccess == null ? true : checkAccess.Value;
        }
    }
}