using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace Neutrino.Data.Synchronization
{
    internal class GlobalJobListener : IJobListener
    {
        public string Name => "GlobalJobListener";

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                Console.WriteLine("{0} -- {1} -- Job ({2}) was vetoed", Name, DateTime.Now, context.JobDetail.Key);
            });
        }
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                Console.WriteLine("{0} -- {1} -- Job ({2}) is about to be executed", Name, DateTime.Now, context.JobDetail.Key);
            });
        }
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                Console.WriteLine("{0} -- {1} -- Job ({2}) was executed", Name, DateTime.Now, context.JobDetail.Key);
            });
        }
    }
}
