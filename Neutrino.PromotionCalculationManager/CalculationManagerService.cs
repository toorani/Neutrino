using NLog;
using Quartz;
using Quartz.Impl;
using ServiceDebuggerHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.PromotionCalculationManager
{
    public partial class CalculationManagerService : DebuggableService
    {
        private IScheduler appScheduler;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CalculationManagerService()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            var schehulerFactory = new StdSchedulerFactory();
            appScheduler = await schehulerFactory.GetScheduler();

        }

        protected override async void OnStop()
        {
            if (appScheduler != null)
            {
                await appScheduler.Shutdown(true);
            }

            await Task.Delay(1000);
        }
    }
}
