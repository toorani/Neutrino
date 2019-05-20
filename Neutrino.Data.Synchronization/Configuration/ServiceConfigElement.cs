using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neutrino.External.Sevices;

namespace Neutrino.Data.Synchronization.Configuration
{
    class ServiceConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public ExternalServices ServiceName => (ExternalServices)base["name"];

        [ConfigurationProperty("gettingYear", IsRequired = false, DefaultValue = 1396)]
        public int GettingYear => (int)base["gettingYear"];

        [ConfigurationProperty("gettingMonth", IsRequired = false, DefaultValue = 1)]
        public int GettingMonth => (int)base["gettingMonth"];

        [ConfigurationProperty("forceDate", IsRequired = false, DefaultValue = false)]
        public bool ForceDate => (bool)base["forceDate"];

        [ConfigurationProperty("dateSensitive", IsRequired = true, DefaultValue = false)]
        public bool DateSensitive => (bool)base["dateSensitive"];

        [ConfigurationProperty("isAcquireExecute", IsRequired = false, DefaultValue = true)]
        public bool IsAcquireExecute => (bool)base["isAcquireExecute"];

        [ConfigurationProperty("isCheckNotCompletedExecute", IsRequired = false, DefaultValue = true)]
        public bool IsCheckNotCompletedExecute => (bool)base["isCheckNotCompletedExecute"];

        [ConfigurationProperty("acquireSchedulePattern", IsRequired = false, DefaultValue = "0 0 1 * * ?")]
        public string AcquireSchedulePattern => (string)base["acquireSchedulePattern"];

        [ConfigurationProperty("checkFailureSchedulePattern", IsRequired = false, DefaultValue = "0 0 3 * * ?")]
        public string CheckFailureSchedulePattern => (string)base["checkFailureSchedulePattern"];

        public ServiceConfigElement()
        {

        }
    }
}
