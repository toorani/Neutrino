using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Data.Synchronization.Configuration
{
    class DataSyncServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("services", IsDefaultCollection = true)]
        public ServiceConfigCollection Services => (ServiceConfigCollection)this["services"];

        [ConfigurationProperty("gettingYear", IsRequired = false, DefaultValue = 1396)]
        public int GettingYear => (int)base["gettingYear"];

        [ConfigurationProperty("gettingMonth", IsRequired = false, DefaultValue = 1)]
        public int GettingMonth => (int)base["gettingMonth"];

        [ConfigurationProperty("maxCheckFailCount", IsRequired = false, DefaultValue = 5)]
        public int MaxCheckFailCount => (int)base["maxCheckFailCount"];

        [ConfigurationProperty("supporterEmailAddress", IsRequired = true)]
        public string SupporterEmailAddress => (string)base["supporterEmailAddress"];

        [ConfigurationProperty("schedulePattern", IsRequired = true)]
        public SchedulePattern SchedulePattern => (SchedulePattern)this["schedulePattern"];
    }
}
