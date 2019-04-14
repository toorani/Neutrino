using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using Espresso.Communication;
using Espresso.Communication.Model;
using Espresso.Core.Ninject;
using Neutrino.Data.EntityFramework;
using Neutrino.Data.Synchronization.Configuration;
using Neutrino.Entities;
using Neutrino.External.Sevices;
using NLog;
using Quartz;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    abstract class ServiceJob : IJob
    {
        #region [ Public properites ]
        public abstract ExternalServices serviceName { get; }
        protected readonly DataSyncServiceSection dataSyncConfiguration;
        protected readonly ServiceConfigElement serviceSetting;
        protected JobCallTypes jobCallTypes;
        protected YearMonth yearMonth;
        #endregion

        #region [ Inner classes ]
        protected class YearMonth
        {
            public int Year;
            public int Month;
        }

        protected class NotCompleteRecord
        {
            public DataSyncStatus DataSyncStatus { get; set; }
            public bool ExistAnyRecord { get; set; }
        }
        #endregion

        #region [ Varibale(s) ]
        private int currentYear;
        private int currentMonth;
        private IJobExecutionContext context;

        #endregion

        #region [ Protected Property(ies) ]
        protected readonly NeutrinoUnitOfWork unitOfWork;
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region [ Constructor(s) ]
        public ServiceJob()
        {
            dataSyncConfiguration = ConfigurationManager.GetSection("serviceSection") as DataSyncServiceSection;
            if (dataSyncConfiguration == null)
                throw new Exception("It wasn't found any configuration section");

            serviceSetting = dataSyncConfiguration.Services[serviceName];

            yearMonth = new YearMonth();

            PersianCalendar pcal = new PersianCalendar();
            currentYear = pcal.GetYear(DateTime.Now);
            currentMonth = pcal.GetMonth(DateTime.Now);
            
            unitOfWork = NinjectContainer.Resolve<NeutrinoUnitOfWork>();
            

        }
        #endregion

        #region [ Public Method(s) ]
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (context.MergedJobDataMap.ContainsKey("jobCallTypes"))
                {
                    jobCallTypes = (JobCallTypes)context.MergedJobDataMap["jobCallTypes"];
                }
                else
                {
                    jobCallTypes = JobCallTypes.AcquireData;
                }
                if (serviceSetting.DateSensitive)
                {

                    if (serviceSetting.ForceDate == false)
                    {
                        yearMonth = await GetLatestYearMonthAsync();
                        if (yearMonth.Month == 0)
                        {
                            yearMonth.Month = dataSyncConfiguration.GettingMonth;
                            yearMonth.Year = dataSyncConfiguration.GettingYear;
                        }
                        else
                        {
                            if (SetNextRangeTime() == false)
                                return;
                        }

                    }
                    else
                    {
                        yearMonth.Month = serviceSetting.GettingMonth;
                        yearMonth.Year = serviceSetting.GettingYear;
                    }

                }
                else
                {
                    yearMonth.Year = currentYear;
                    yearMonth.Month = currentMonth;
                }
                logger.Trace(serviceName, $"starting {serviceName} service.the mode is {jobCallTypes}.the date range is {yearMonth.Year}--{yearMonth.Month}");
                await Execute();
                logger.Trace(serviceName, $"finishing {serviceName} service.the mode is {jobCallTypes}.the date range is {yearMonth.Year}--{yearMonth.Month}");
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }
        #endregion

        #region [ Abstract Method(s) ]
        protected abstract Task Execute();
        protected virtual Task<YearMonth> GetLatestYearMonthAsync()
        {
            return Task.FromResult(new YearMonth() { Month = dataSyncConfiguration.GettingMonth, Year = dataSyncConfiguration.GettingYear });
        }
        #endregion

        #region [ Protected Method(s) ]
        protected async Task RecordDataSyncStatusAsync(int insertedCount, int readedCount, bool increaseTryCount = true)
        {
            var dataSyncStatus = await unitOfWork.DataSyncStatusDataService.FirstOrDefaultAsync(where: x => x.Year == yearMonth.Year
                    && x.Month == yearMonth.Month
                    && x.ServiceName == serviceName.ToString());
            await RecordDataSyncStatusAsync(insertedCount, readedCount, dataSyncStatus, increaseTryCount);
        }
        protected async Task RecordDataSyncStatusAsync(int insertedCount, int readedCount, DataSyncStatus dataSyncStatus, bool increaseTryCount = true)
        {
            if (readedCount != 0)
            {
                if (dataSyncStatus == null)
                {
                    dataSyncStatus = new DataSyncStatus();
                    dataSyncStatus.Month = yearMonth.Month;
                    dataSyncStatus.Year = yearMonth.Year;
                    dataSyncStatus.InsertedCount = insertedCount;
                    dataSyncStatus.ReadedCount = readedCount;
                    dataSyncStatus.ServiceName = serviceName.ToString();
                    unitOfWork.DataSyncStatusDataService.Insert(dataSyncStatus);
                }
                else
                {
                    //dataSyncStatus.ReadedCount = readedCount;
                    dataSyncStatus.InsertedCount += insertedCount;
                    dataSyncStatus.TryCount += 1;
                    unitOfWork.DataSyncStatusDataService.Update(dataSyncStatus);
                }
                await unitOfWork.CommitAsync();
            }
        }
        protected bool SetNextRangeTime()
        {
            // go the next month
            yearMonth.Month += 1;
            if (yearMonth.Month > 12)
            {
                yearMonth.Month = 1;
                yearMonth.Year++;
            }

            if (currentMonth < yearMonth.Month && yearMonth.Year > currentYear)
            {
                logger.Info(serviceName, "service {0} hasn't completed successfully,there isn't any data to load", serviceName);
                return false;
            }
            return true;
        }
        protected async Task<NotCompleteRecord> SetNotCompleteRangeTime(List<YearMonth> lstMonthYear)
        {
            NotCompleteRecord notCompleteRecord = new NotCompleteRecord();

            notCompleteRecord.ExistAnyRecord = false;
            if (lstMonthYear.Count != 0)
            {
                foreach (var item in lstMonthYear)
                {
                    var dataSyncStatus = await unitOfWork.DataSyncStatusDataService.FirstOrDefaultAsync(x => x.ServiceName == serviceName.ToString()
                    && x.Year == item.Year
                    && x.Month == item.Month);

                    if (dataSyncStatus == null)
                    {
                        //failing at saving the DataSyncStatus row in the database on AcquireData mode.
                        yearMonth.Month = item.Month;
                        yearMonth.Year = item.Year;
                        notCompleteRecord.ExistAnyRecord = true;

                        break;
                    }
                    else if (dataSyncStatus.IsInsertedAllData == false && dataSyncStatus.DateCreated < DateTime.Now.Date)
                    {
                        if (dataSyncStatus.TryCount >= dataSyncConfiguration.MaxCheckFailCount)
                        {
                            var mailData = new Mail();
                            mailData.Subject = "Failing in running a service";
                            mailData.Body = string.Format("service {0} has failed {1} times.", serviceName, dataSyncStatus.TryCount);
                            mailData.To = dataSyncConfiguration.SupporterEmailAddress;

                            await Communicator.Instance.SendGmailAsync(mailData);
                        }
                        yearMonth.Month = dataSyncStatus.Month;
                        yearMonth.Year = dataSyncStatus.Year;
                        notCompleteRecord.ExistAnyRecord = true;
                        notCompleteRecord.DataSyncStatus = dataSyncStatus;
                        break;
                    }

                }
            }
            else
            {
                var dataSyncStatus = await unitOfWork.DataSyncStatusDataService.FirstOrDefaultAsync(x => x.ServiceName == serviceName.ToString());

                if (dataSyncStatus == null)
                {
                    notCompleteRecord.ExistAnyRecord = true;
                }
                else if (dataSyncStatus.IsInsertedAllData == false && dataSyncStatus.DateCreated < DateTime.Now.Date)
                {
                    if (dataSyncStatus.TryCount >= dataSyncConfiguration.MaxCheckFailCount)
                    {
                        var mailData = new Mail();
                        mailData.Subject = "Failing in running a service";
                        mailData.Body = string.Format("service {0} has failed {1} times.", serviceName, dataSyncStatus.TryCount);
                        mailData.To = dataSyncConfiguration.SupporterEmailAddress;

                        await Communicator.Instance.SendGmailAsync(mailData);
                    }
                    yearMonth.Month = dataSyncStatus.Month;
                    yearMonth.Year = dataSyncStatus.Year;
                    notCompleteRecord.ExistAnyRecord = true;
                    notCompleteRecord.DataSyncStatus = dataSyncStatus;
                }
            }

            return notCompleteRecord;

        }
        #endregion

        #region [ Log Methods ]
        //protected void DoLogServiceStarted()
        //{
        //    if (externalData != null)
        //    {
        //        if (externalData.JobCallTypes == JobCallTypes.AcquireData)
        //        {
        //            logger.exInfo(serviceName, "{0} service by mode {1} started,load data range : Year is {2} and Month {3}"
        //               , serviceName
        //               , externalData.JobCallTypes
        //               , externalData.Year
        //               , externalData.Month
        //               );
        //        }
        //        else if (externalData.JobCallTypes == JobCallTypes.CheckNotCompleted)
        //        {

        //            logger.exInfo(serviceName, "{0} service by mode {1} started,reload data range : Year is {2} and Month {3}"
        //                , serviceName
        //                , externalData.JobCallTypes
        //                , externalData.Year
        //                , externalData.Month);
        //        }

        //    }
        //    else
        //    {
        //        logger.exInfo(serviceName, "{0} service started", serviceName);
        //    }
        //}
        protected void LogInsertedData(int loadedCount, int instertedCount, int duplicatedCount = -1)
        {
            logger.Info(serviceName, $"Calling of {serviceName} service with {jobCallTypes} mode " +
                        $"in the year {yearMonth.Year} and month {yearMonth.Month} has finished." +
                        $"the number of records loaded: {loadedCount}, the number of records inserted: {instertedCount}" +
                        (duplicatedCount == -1 ? "" : $", the number of records duplicate: {duplicatedCount}."));

        }
        protected void LogNotDataForReloading()
        {
            logger.Info(serviceName, "{0} service by mode {1} finished.There wasn't any record for reloading."
                           , serviceName
                           , jobCallTypes);
        }
        protected void LogException(Exception ex)
        {
            if (serviceName != ExternalServices.ReportSummery)
                logger.Error(serviceName, "service {0} has encountred the error.the service's mode is {1}", serviceName, jobCallTypes, ex);
            else
                logger.Error(serviceName, "service {0} has encountred the error", serviceName, ex);
        }
        #endregion

    }
}
