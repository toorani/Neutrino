using System.Collections.Generic;
using Espresso.InjectModules;
using Neutrino.InjectModules;
using Ninject.Extensions.Logging.NLog4;
using Ninject.Modules;

namespace Neutrino.Data.Synchronization
{
    public class NinjectModules
    {
        public static List<NinjectModule> Modules
        {
            get
            {
                return new List<NinjectModule>() { new EliteServicesModule()
                    , new MainModule()
                    , new SharedModule()
                    , new EspressoModule()
                    , new NLogModule() };
            }
        }

    }



}
