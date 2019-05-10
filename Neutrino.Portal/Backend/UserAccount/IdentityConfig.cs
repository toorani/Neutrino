using Espresso.Communication;
using Espresso.Communication.Model;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Neutrino.Portal
{
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