using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neutrino.Core;
using Ninject.Extensions.Logging;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    public interface IServiceJob
    {
        ExternalServices ServiceName { get; }
        ILogger Logger { get; set; }
    }
}
