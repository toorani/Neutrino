using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Neutrino.Portal.Startup))]
namespace Neutrino.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
