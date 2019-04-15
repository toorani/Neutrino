using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace Neutrino.Portal.Attributes
{
    public class WebApiValidateAntiForgeryTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (actionContext.Request.Method.Method != "GET")
            {
                var headers = actionContext.Request.Headers;
                var tokenCookie = headers
                    .GetCookies()
                    .Select(c => c[AntiForgeryConfig.CookieName])
                    .FirstOrDefault();

                var tokenHeader = string.Empty;
                if (headers.Contains("X-XSRF-Token"))
                {
                    tokenHeader = headers.GetValues("X-XSRF-Token").FirstOrDefault();
                }

                try
                {
                    AntiForgery.Validate(tokenCookie != null ? tokenCookie.Value : null, tokenHeader);
                }
                catch (System.Web.Mvc.HttpAntiForgeryException ex)
                {
                    actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex);
                }
                
                
            }

            base.OnActionExecuting(actionContext);
        }
        
    }
}