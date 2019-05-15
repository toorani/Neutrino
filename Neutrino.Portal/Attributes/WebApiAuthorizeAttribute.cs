using Neutrino.Business;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Neutrino.Portal.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class WebApiAuthorizeAttribute : AuthorizeAttribute
    {
        #region [ Varibale(s) ]
        private readonly bool checkAccessEnabled = true;
        #endregion

        #region [ Constructor(s) ]
        public WebApiAuthorizeAttribute()
        {
            //permissionManager = NinjectHttpContainer.Resolve<IPermissionManager>();
            checkAccessEnabled = IdentityConfig.CheckAccessEnabled;
        }
        #endregion

        #region [ Override Method(s) ]
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (checkAccessEnabled)
            {
                if (IsAuthorized(actionContext))
                {
                    base.OnAuthorization(actionContext);
                }
                else
                {
                    //actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "شما دسترسی به این قسمت ندارید");
                }
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (checkAccessEnabled)
            {
                var claimPrincipal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;
                var actionUrl = actionContext.Request.RequestUri.AbsolutePath;
                if (actionUrl.StartsWith("/"))
                {
                    actionUrl = actionUrl.Substring(1, actionUrl.Length - 1);
                }
                return claimPrincipal.Claims.Any(x => x.Value.ToLower() == actionUrl.ToLower() && x.Type == ApplicationClaimTypes.ActionUrl);
            }
            return base.IsAuthorized(actionContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (checkAccessEnabled)
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "شما دسترسی به این قسمت ندارید");
            else
                base.HandleUnauthorizedRequest(actionContext);
        }

        #endregion

        #region [ Private Method(s) ]
        
        #endregion

    }
}