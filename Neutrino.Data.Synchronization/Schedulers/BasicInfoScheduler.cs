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
    class BasicInfoScheduler : GeneralScheduler
    {
        #region [ Constructor(s) ]
        public BasicInfoScheduler(IScheduler scheduler)
			:base(scheduler)
        {
            
        }

        #endregion

        #region [ Override Method(s) ]
        protected override ITrigger CreateTrigger(string name)
        {
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 5);
            /*
			 Seconds Minutes Hours Day-of-Month Month Day-of-Week Year (optional field)
			 the value “L” in the day-of-month field means “the last day of the month” - day 31 for January,
             day 28 for February on non-leap years
			 */
            return TriggerBuilder.Create()
                       .WithIdentity(name, "serviceGroups")
                       //.WithCronSchedule("0 30 0 * * ?")
                       .StartAt(startTime)
                       .Build();
        }
        #endregion

    }
}
