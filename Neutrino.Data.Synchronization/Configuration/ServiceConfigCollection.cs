using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neutrino.External.Sevices;

namespace Neutrino.Data.Synchronization.Configuration
{
    [ConfigurationCollection(typeof(ServiceConfigElement),AddItemName = "service")]
    class ServiceConfigCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "service";
        public ServiceConfigElement this[int idx]
        {
            get { return (ServiceConfigElement)BaseGet(idx); }
        }
        public ServiceConfigElement this[ExternalServices serviceName]
        {
            get
            {
                var result = this.OfType<ServiceConfigElement>().FirstOrDefault(x => x.ServiceName == serviceName);
                if (result == null)
                {
                    result = new ServiceConfigElement();
                }
                return result;
            }
        }

        protected override string ElementName
        {
            get
            {
                return PropertyName;
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceConfigElement)(element)).ServiceName;
        }
    }
}
