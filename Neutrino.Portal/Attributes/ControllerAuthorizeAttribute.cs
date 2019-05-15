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
        #region [ Varibale(s) ]
        private readonly bool checkAccessEnabled = true;
        #endregion

        #region [ Constructor(s) ]
        public ControllerAuthorizeAttribute()
        {
            checkAccessEnabled = IdentityConfig.CheckAccessEnabled;
        }
        #endregion

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (checkAccessEnabled)
            {
                return base.AuthorizeCore(httpContext);
            }
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (checkAccessEnabled)
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}