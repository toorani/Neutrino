using Espresso.BusinessService.Interfaces;
using Espresso.Communication;
using Espresso.Communication.Model;
using Espresso.Core;
using Espresso.Core.Ninject;
using Neutrino.Entities;
using Quartz;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Neutrino.Data.Synchronization
{
    internal class GlobalTriggerListener : ITriggerListener
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<DataSyncStatus> dataSyncStatusDataService;
        #endregion

        #region [ Public Property(ies) ]
        public string Name => "GlobalTriggerListener";
        #endregion

        #region [ Constructor(s) ]
        public GlobalTriggerListener()
        {
            dataSyncStatusDataService = NinjectContainer.Resolve<IEntityListLoader<DataSyncStatus>>();
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {

            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month
                , DateTime.Now.Day, 0, 0, 0);
            string emailBody = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\EmailTemplate\\ErrorNotification.html");
            var lstData = await dataSyncStatusDataService.LoadListAsync(x => x.LastUpdated > dateTime);
            if (lstData.ReturnStatus)
            {
                Mail mail = new Mail();
                var report = string.Empty;
                lstData.ResultValue.ForEach(x =>
                {
                    report += $"<tr><td>{x.ServiceName}</td><td>{x.Year} / {x.Month} </td><td>{x.ReadedCount}</td><td>{x.InsertedCount}</td><td>{x.TryCount}</td></tr>";
                });
                mail.Body = emailBody.Replace("$TableRow$", report)
                    .Replace("$DateTime$", Utilities.ToPersianDate(DateTime.Now));
                mail.To = ConfigurationManager.AppSettings["SupporterEmailAddress"];
                mail.Subject = $"Neutrino Daily Service Call Report  - {Utilities.ToPersianDate(DateTime.Now)} ";
                await Communicator.Instance.SendGmailAsync(mail);
            }

        }
        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                Console.WriteLine("{0} -- {1} -- Trigger ({2}) was fired", Name, DateTime.Now, trigger.Key);
            });
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                Console.WriteLine("{0} -- {1} -- Trigger ({2}) was misfired", Name, DateTime.Now, trigger.Key);
            });
        }

        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine("{0} -- {1} -- Trigger ({2}) is going to veto the job ({3})", Name, DateTime.Now, trigger.Key, context.JobDetail.Key);
            return Task.Run(() =>
            {
                return false;
            });
        }
        #endregion


    }
}
