using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;

namespace Neutrino.Data.Synchronization.Schedulers
{
    class GeneralScheduler
    {
        #region [ Protected Property(ies) ]
        protected IScheduler scheduler;
        #endregion

        #region [ Constructor(s) ]
        public GeneralScheduler(IScheduler scheduler)
        {
            this.scheduler = scheduler;
        }
        #endregion

        #region [ Public Method(s) ]
        public virtual async Task AddServiceAsync<TServiceName>()
            where TServiceName : IJob
        {
            try
            {
                IJobDetail job = JobBuilder.Create<TServiceName>()
                  .WithIdentity(typeof(TServiceName).Name, "serviceGroups")
                  .Build();

                ITrigger trigger = CreateTrigger("tri" + typeof(TServiceName).Name);

                await scheduler.ScheduleJob(job, trigger);
                //await scheduler.Start();
            }
            catch (Exception ex)
            {

            }

        }

        public virtual void AddService<TServiceName>()
            where TServiceName : IJob
        {
            try
            {
                IJobDetail job = JobBuilder.Create<TServiceName>()
                  .WithIdentity(typeof(TServiceName).Name, "serviceGroups")
                  .Build();

                ITrigger trigger = CreateTrigger("tri" + typeof(TServiceName).Name);

                Task.Run(() => scheduler.ScheduleJob(job, trigger));
                //await scheduler.Start();
            }
            catch (Exception ex)
            {
                //TODO log
            }

        }
        public virtual async Task StartAsync()
        {
            await scheduler.Start();
        }
        public virtual async Task Stop()
        {
            await scheduler.Shutdown(true);
        }
        #endregion

        #region [ Protected Method(s) ]
        protected virtual ITrigger CreateTrigger(string name)
        {
            DateTimeOffset startTime = DateBuilder.NextGivenMinuteDate(null, 2);

            return TriggerBuilder.Create()
                       .WithIdentity(name, "serviceGroups")
                       //.WithCronSchedule("0 0 1 * * ?")
                       //.WithCronSchedule("0 14 0 * * ?")
                       .StartAt(startTime)
                       .Build();
        }
        #endregion
    }
}
