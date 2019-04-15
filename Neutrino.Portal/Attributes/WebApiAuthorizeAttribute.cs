using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Espresso.Core.Ninject.Http;
using Espresso.Utilities.Interfaces;
using Neutrino.Core.SecurityManagement;

namespace Neutrino.Portal.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class WebApiAuthorizeAttribute : AuthorizeAttribute
    {
        #region [ Varibale(s) ]
        private readonly IAppSettingManager appSettingManager;
        private readonly IPermissionManager permissionManager;
        private int userId = 0;
        private bool? checkAccess = true;
        
        #endregion

        #region [ Constructor(s) ]
        public WebApiAuthorizeAttribute()
        {
            appSettingManager= NinjectHttpContainer.Resolve<IAppSettingManager>();
            permissionManager = NinjectHttpContainer.Resolve<IPermissionManager>();

            checkAccess = appSettingManager.GetValue<bool>("checkAccess");
            checkAccess = checkAccess == null ? true : checkAccess.Value;
            
        }
        #endregion

        #region [ Override Method(s) ]
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (checkAccess.Value)
            {
                setUserId(actionContext);
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
            if (checkAccess.Value)
                return Task.Run(() => permissionManager.HasAccess(actionContext.ControllerContext.Request.RequestUri.LocalPath, userId)).Result;
            return base.IsAuthorized(actionContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (checkAccess.Value)
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "شما دسترسی به این قسمت ندارید");
            else
                base.HandleUnauthorizedRequest(actionContext);
        }

        #endregion

        #region [ Private Method(s) ]
        private void setUserId(HttpActionContext actionContext)
        {
            if (userId == 0)
            {
                var claimsPrincipal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;
                userId = Convert.ToInt32(claimsPrincipal.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
            }
        }
        #endregion

    }
}