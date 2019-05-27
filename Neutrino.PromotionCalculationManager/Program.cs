using Espresso.Core.Ninject;
using Espresso.InjectModules;
using Neutrino.InjectModules;
using Ninject;
using Ninject.Extensions.Logging.NLog4;
using Ninject.Modules;
using ServiceDebuggerHelper;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Windows.Forms;

namespace Neutrino.PromotionCalculationManager
{
    static class Program
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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var settings = new NinjectSettings
            {
                LoadExtensions = false,
                AllowNullInjection = true
            };
            IKernel kernel = new StandardKernel(settings);
            NinjectContainer.RegisterModules(kernel, Modules);
            if (ConfigurationManager.AppSettings["AppMode"].ToLower() == "debug")
                Application.Run(new ServiceRunner(new CalculationManagerService()));
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new CalculationManagerService()

                };
                ServiceBase.Run(ServicesToRun);

            }
            
        }
    }
}
