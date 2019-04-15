using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Espresso.Core.Ninject;
using Ninject;
using Ninject.Extensions.Logging.NLog4;
using Quartz;
using Quartz.Impl;
using ServiceDebuggerHelper;

namespace Neutrino.Data.Synchronization
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var settings = new NinjectSettings();
            settings.LoadExtensions = false;
            settings.AllowNullInjection = true;
            IKernel kernel = new StandardKernel(settings);

            NinjectContainer.RegisterModules(kernel,NinjectModules.Modules);

#if DEBUG
            Application.Run(new ServiceRunner(new DataSyncService()));
#elif !DEBUG
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                    new DataSyncService()

            };
            ServiceBase.Run(ServicesToRun);
#endif
            

        }
    }

    
}
