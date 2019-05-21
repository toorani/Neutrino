using Espresso.Core.Ninject;
using Ninject;
using ServiceDebuggerHelper;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Windows.Forms;

namespace Neutrino.Data.Synchronization
{
    static class Program
    {
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
            NinjectContainer.RegisterModules(kernel, NinjectModules.Modules);

            if (ConfigurationManager.AppSettings["AppMode"].ToLower() == "debug")
                Application.Run(new ServiceRunner(new DataSyncService()));
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new DataSyncService()

                };
                ServiceBase.Run(ServicesToRun);

            }

        }
    }


}
