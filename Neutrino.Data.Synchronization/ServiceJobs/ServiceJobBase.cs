using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Quartz;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    abstract class ServiceJobBase : IJob
    {
        #region [ Protected Property(ies) ]
        protected StandardKernel kernel;
        #endregion

        #region [ Constructor(s) ]
        public ServiceJobBase()
        {
            kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
        }
        public abstract Task Execute(IJobExecutionContext context);
        #endregion

        #region [ Protected Method(s) ]
        #endregion

    }
}
