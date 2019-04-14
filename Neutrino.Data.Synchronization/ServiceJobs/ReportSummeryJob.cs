using System;
using System.IO;
using System.Threading.Tasks;
using Espresso.Communication;
using Espresso.Communication.Model;
using Espresso.Core;
using Neutrino.External.Sevices;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class ReportSummeryJob : ServiceJob
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.ReportSummery; }
        }
        #endregion

        #region [ Constructor(s) ]
        public ReportSummeryJob() : base()
        {
            
        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task Execute()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            DateTime dateTime = new DateTime(year, month, day, 0, 0, 0);
            string emailBody = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\EmailTemplate\\Summery.html");
            var lstData = await unitOfWork.DataSyncStatusDataService.GetAsync(x => x.LastUpdated > dateTime);
            Mail mail = new Mail();
            var report = string.Empty;
            lstData.ForEach(x =>
            {
                report += $"<tr><td>{x.ServiceName}</td><td>{x.Year} / {x.Month} </td><td>{x.ReadedCount}</td><td>{x.InsertedCount}</td><td>{x.TryCount}</td></tr>";
            });
            mail.Body = emailBody.Replace("$TableRow$", report)
                .Replace("$DateTime$", Utilities.ToPersianDate(DateTime.Now));
            mail.To = dataSyncConfiguration.SupporterEmailAddress;
            mail.Subject = $"Neutrino Daily Service Call Report  - {Utilities.ToPersianDate(DateTime.Now)} ";
            await Communicator.Instance.SendGmailAsync(mail);

            string full_logFileName = AppDomain.CurrentDomain.BaseDirectory
                + "logs"
                + Path.DirectorySeparatorChar
                + year + "-"
                + (month <= 9 ? "0" + month : month.ToString()) + "-"
                + (day <= 9 ? "0" + day : day.ToString());

            string file_errorAndWarning = full_logFileName + "(er-war).csv"; //2019-03-30(er-war).scv

            logger.Trace($"file_errorAndWarning is {file_errorAndWarning}");
            logger.Trace($"Exists file_errorAndWarning is {File.Exists(file_errorAndWarning)}");
            
            if (File.Exists(file_errorAndWarning))
            {
                mail = new Mail();
                emailBody = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\EmailTemplate\\ErrorAndWarning.html");
                mail.Body = emailBody.Replace("$DateTime$", Utilities.ToPersianDate(DateTime.Now));
                mail.To = dataSyncConfiguration.SupporterEmailAddress;
                mail.Subject = $"Neutrino Service error and warning reports - {Utilities.ToPersianDate(DateTime.Now)} ";

                mail.Attachments.Add(file_errorAndWarning);

                await Communicator.Instance.SendGmailAsync(mail);
            }
        }
        #endregion

        #region [ Private Method(s) ]

        #endregion
    }
}
