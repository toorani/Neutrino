using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Neutrino.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Neutrino.Portal
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.


    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        private readonly ApplicationUserManager userManager;

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            this.userManager = userManager;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return userManager.GenerateUserIdentityAsync(user);
        }

    }
}
