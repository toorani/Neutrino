using System;
using System.Threading.Tasks;
using Espresso.Communication;
using Espresso.Communication.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neutrino.ServiceTests
{
    [TestClass]
    public class GmailTest
    {
        [TestMethod]
        public async Task SendEmailWithAttachment()
        {
            Mail mailInfo = new Mail();
            mailInfo.Attachments.Add(@"D:\Projects\Elite\Source\Neutrino\Neutrino.External.ServiceTests\bin\Debug\logs\2019-03-30.json");
            mailInfo.Subject = "Failing in running a service";
            mailInfo.Body = string.Format("service 0 has failed 1 times.");
            mailInfo.To = "porzn20@gmail.com";

            var result = await Communicator.Instance.SendGmailAsync(mailInfo);
            Assert.IsTrue(result);
        }
    }
}
