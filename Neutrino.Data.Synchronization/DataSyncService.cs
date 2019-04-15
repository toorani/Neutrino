using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using Neutrino.Data.Synchronization.ServiceJobs;
using NLog;
using Quartz;
using Quartz.Impl;
using ServiceDebuggerHelper;
using Espresso.Core;
using Neutrino.Data.Synchronization.Configuration;
using Neutrino.External.Sevices;

namespace Neutrino.Data.Synchronization
{
    public enum JobCallTypes
    {
        AcquireData,
        CheckNotCompleted
    }
    public partial class DataSyncService : DebuggableService
    {
        #region [ Varibale(s) ]
        private IScheduler mainScheduler;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DataSyncServiceSection dataSyncConfiguration;
        #endregion

        #region [ Constructor(s) ]
        public DataSyncService()
        {
            InitializeComponent();
        }
        #endregion

        #region [ Protected Method(s) ]
        protected override async void OnStart(string[] args)
        {
            // First we must get a reference to a scheduler
            NameValueCollection properties = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "Neutrino.Data.Synchronization",
                // Set thread count to 1 to force Triggers scheduled for the same time to
                // to be ordered by priority.
                ["quartz.threadPool.threadCount"] = "1",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz"
            };
            dataSyncConfiguration = ConfigurationManager.GetSection("serviceSection") as DataSyncServiceSection;

            var mainSchehulerFactory = new StdSchedulerFactory(properties);

            mainScheduler = await mainSchehulerFactory.GetScheduler();

            //mainScheduler.ListenerManager.AddJobListener(new GlobalJobListener());
            //mainScheduler.ListenerManager.AddTriggerListener(new GlobalTriggerListener());

            if (dataSyncConfiguration.Services[ExternalServices.Company].IsAcquireExecute)
                // this service doesn't have any pre-required to get data
                await AddAcquireJobAsync<CompanyJob>(10);

            if (dataSyncConfiguration.Services[ExternalServices.Branch].IsAcquireExecute)
                //this service doesn't have any pre-required to get data
                await AddAcquireJobAsync<BranchJob>(10);

            if (dataSyncConfiguration.Services[ExternalServices.GoodsCatType].IsAcquireExecute)
                // this service doesn't have any pre-required to get data
                await AddAcquireJobAsync<GoodsCategoryTypesJob>(10);

            if (dataSyncConfiguration.Services[ExternalServices.Goods].IsAcquireExecute)
                //this service needs a companyId to get data
                await AddAcquireJobAsync<GoodsJob>(9);

            if (dataSyncConfiguration.Services[ExternalServices.Members].IsAcquireExecute)
                //this service needs a branchId to get data
                await AddAcquireJobAsync<MembersJob>(8);

            if (dataSyncConfiguration.Services[ExternalServices.GoodsCat].IsAcquireExecute)
                //this service needs a companyId, a goodsId and the goodsCategoryType's Ids to get data
                await AddAcquireJobAsync<GoodsCategoryJob>(8);

            if (dataSyncConfiguration.Services[ExternalServices.BranchSales].IsAcquireExecute)
                //this service needs the goods and branches data to save data
                await AddAcquireJobAsync<BranchSalesJob>(7);

            if (dataSyncConfiguration.Services[ExternalServices.Invoice].IsAcquireExecute)
                //this service needs the members and goods data to save data
                await AddAcquireJobAsync<InvoiceJob>(7);

            if (dataSyncConfiguration.Services[ExternalServices.Payroll].IsAcquireExecute)
                //this service needs the members and goods data to save data 
                await AddAcquireJobAsync<PayrollJob>(7);

            //this service needs the members data to save data 
            //await AddAcquireJobAsync<MemberReceiptJob>(7, jobExternalData);

            if (dataSyncConfiguration.Services[ExternalServices.BranchReceipts].IsAcquireExecute)
                //this service needs the branch data to save data 
                await AddAcquireJobAsync<BranchReceiptJob>(7);

            await mainScheduler.Start();


            //check not completed jobs

            if (dataSyncConfiguration.Services[ExternalServices.Payroll].IsCheckNotCompletedExecute)
                await AddCheckNotCompletedJobAsync<PayrollJob>();

            //await AddCheckFailureJobAsync<MemberReceiptJob>(jobExternalData);

            if (dataSyncConfiguration.Services[ExternalServices.BranchReceipts].IsCheckNotCompletedExecute)
                await AddCheckNotCompletedJobAsync<BranchReceiptJob>();

            if (dataSyncConfiguration.Services[ExternalServices.Invoice].IsCheckNotCompletedExecute)
                await AddCheckNotCompletedJobAsync<InvoiceJob>();

            if (dataSyncConfiguration.Services[ExternalServices.BranchSales].IsCheckNotCompletedExecute)
                await AddCheckNotCompletedJobAsync<BranchSalesJob>();

            if (dataSyncConfiguration.Services[ExternalServices.ReportSummery].IsAcquireExecute)
                //add report summery job
                await AddReportSummeryJobAsync<ReportSummeryJob>();
        }
        protected override async void OnStop()
        {
            if (mainScheduler != null)
            {
                await mainScheduler.Shutdown(true);
            }

            await Task.Delay(1000);
        }
        #endregion

        #region [ Private Method(s) ]
        private async Task AddAcquireJobAsync<TServiceJobName>(int priority)
            where TServiceJobName : ServiceJob, new()
        {
            Dictionary<string, object> jobDataMap = new Dictionary<string, object>();
            jobDataMap.Add("jobCallTypes", JobCallTypes.AcquireData);
            IJobDetail job = JobBuilder.Create<TServiceJobName>()
                .WithIdentity(typeof(TServiceJobName).Name)
                .Build();
            job.JobDataMap.PutAll(jobDataMap);


            /*
            Seconds Minutes Hours Day-of-Month Month Day-of-Week Year (optional field)
            the value “L” in the day-of-month field means “the last day of the month” - day 31 for January,
            day 28 for February on non-leap years
            */
#if (DEBUG == true)
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 5);
            int jobRepeatCount = ConfigurationManager.AppSettings["JobRepeatCount"].ToInt(defaultValue: 0);
            int repeatInterval = ConfigurationManager.AppSettings["RepeatInterval"].ToInt(defaultValue: 60);

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger_" + typeof(TServiceJobName).Name + "_" + priority)
                .StartAt(startTime)
                .WithPriority(priority)
                .WithSimpleSchedule(x => x.WithRepeatCount(jobRepeatCount).WithInterval(TimeSpan.FromMinutes(repeatInterval)))
                .ForJob(job)
                .Build();



#elif (DEBUG == false)

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger_" + typeof(TServiceJobName).Name + "_" + priority)
                .WithCronSchedule(dataSyncConfiguration.SchedulePattern.AcquireMode)
                .WithPriority(priority)
                .ForJob(job)
                .Build();
#endif
            await mainScheduler.ScheduleJob(job, trigger);

        }
        private async Task AddCheckNotCompletedJobAsync<TServiceJobName>()
            where TServiceJobName : ServiceJob, new()
        {
            Dictionary<string, object> jobDataMap = new Dictionary<string, object>();
            jobDataMap["jobCallTypes"] = JobCallTypes.CheckNotCompleted;
            IJobDetail job = JobBuilder.Create<TServiceJobName>()
                .WithIdentity(typeof(TServiceJobName).Name + "_chk")
                .Build();
            job.JobDataMap.PutAll(jobDataMap);

#if (DEBUG == true)
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 5);
            int jobRepeatCount = ConfigurationManager.AppSettings["JobRepeatCount"].ToInt(defaultValue: 0);
            int repeatInterval = ConfigurationManager.AppSettings["RepeatInterval"].ToInt(defaultValue: 60);

            ITrigger trigger = TriggerBuilder.Create()

                .WithIdentity("trigger_chk_" + typeof(TServiceJobName).Name + "_")
                .StartAt(startTime)
                //.WithPriority(priority)
                .WithSimpleSchedule(x => x.WithRepeatCount(jobRepeatCount)
                                           .WithInterval(TimeSpan.FromMinutes(repeatInterval)))
                .ForJob(job)

                .Build();

#elif (DEBUG == false)

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger_chk_" + typeof(TServiceJobName).Name + "_" )
                .WithCronSchedule(dataSyncConfiguration.SchedulePattern.CheckFailureMode)
                //.WithPriority(priority)
                .ForJob(job)
                .Build();
#endif
            await mainScheduler.ScheduleJob(job, trigger);

        }

        private async Task AddReportSummeryJobAsync<TServiceJobName>()
            where TServiceJobName : IJob
        {

            IJobDetail job = JobBuilder.Create<TServiceJobName>()
                .WithIdentity(typeof(TServiceJobName).Name + "_rpt")
                .Build();

#if (DEBUG == true)
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 5);
            int jobRepeatCount = ConfigurationManager.AppSettings["JobRepeatCount"].ToInt(defaultValue: 0);
            int repeatInterval = ConfigurationManager.AppSettings["RepeatInterval"].ToInt(defaultValue: 60);

            ITrigger trigger = TriggerBuilder.Create()

                .WithIdentity("trigger__rpt_" + typeof(TServiceJobName).Name + "_")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithRepeatCount(jobRepeatCount)
                                           .WithInterval(TimeSpan.FromMinutes(repeatInterval)))
                .ForJob(job)

                .Build();

#elif (DEBUG == false)
            

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger__rpt_" + typeof(TServiceJobName).Name + "_")
                .WithCronSchedule(dataSyncConfiguration.SchedulePattern.ReportSummery)
                .ForJob(job)
                .Build();
#endif
            await mainScheduler.ScheduleJob(job, trigger);

        }
        #endregion

    }
}
