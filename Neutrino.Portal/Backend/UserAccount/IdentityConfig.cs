using Espresso.Communication;
using Espresso.Communication.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Linq;
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