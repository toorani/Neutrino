using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Owin;
using System;
using System.Threading.Tasks;

namespace Neutrino.Portal
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext(NinjectWebCommon.CreateKernel);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //LoginPath = new PathString(String.Empty),
                LoginPath = new PathString("/Account/Login"),

                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  

                    OnValidateIdentity = (ctx) => Task.FromResult(0)
                    //SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, int>(
                    //validateInterval: TimeSpan.FromMinutes(30),
                    //regenerateIdentityCallback: (manager, user) => manager.GenerateUserIdentityAsync(user),
                    //getUserIdCallback: (id) => (id.GetUserId<int>())),
                    //OnApplyRedirect = ctx =>
                    //{
                    //    if (!IsAjaxRequest(ctx.Request))
                    //    {
                    //        ctx.Response.Redirect(ctx.RedirectUri);
                    //    }
                    //}
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }

        private static bool IsAjaxRequest(IOwinRequest request)
        {
            IReadableStringCollection query = request.Query;
            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }
            IHeaderDictionary headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}