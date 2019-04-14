using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Data.Synchronization.Configuration
{
    class SchedulePattern : ConfigurationElement
    {
        [ConfigurationProperty("acquireMode", DefaultValue = "0 0 1 * * ?")]
        public string AcquireMode => (string)this["acquireMode"];

        [ConfigurationProperty("checkFailureMode", DefaultValue = "0 0 3 * * ?")]
        public string CheckFailureMode => (string)this["checkFailureMode"];

        [ConfigurationProperty("reportSummery", DefaultValue = "0 0 7 * * ?")]
        public string ReportSummery => (string)this["reportSummery"];

    }
}
