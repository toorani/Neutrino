using Espresso.Communication;
using Espresso.Communication.Model;
using Microsoft.AspNet.Identity;
using Neutrino.Business;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Neutrino.Portal
{
    public class IdentityConfig
    {
        public static bool CheckAccessEnabled
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CheckAccessEnabled"))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["CheckAccessEnabled"]);
                }
                return true;
            }
        }

        public static int GetBranchId(IPrincipal user)
        {
            if (ConfigurationManager.AppSettings["Mode"] == "development")
                return 2397;
            var claimPrincipal = user as ClaimsPrincipal;
            if (claimPrincipal.HasClaim(x => x.Type == ApplicationClaimTypes.BranchId))
            {
                return Convert.ToInt32(claimPrincipal.FindFirst(x => x.Type == ApplicationClaimTypes.BranchId).Value);
            }
            return 0;
        }

    }

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            Mail mailInfo = new Mail
            {
                To = message.Destination,
                Subject = message.Subject,
                Body = message.Body
            };
            await Communicator.Instance.SendGmailAsync(mailInfo);
        }
    }
}