using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core.Ninject;
using Espresso.Core.Ninject.Http;
using Espresso.InjectModules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.InjectModules;
using Ninject.MockingKernel.Moq;
using Ninject.Modules;

namespace Neutrino.Portal.Tests
{
    public class UnitTestBase
    {
        protected readonly MoqMockingKernel _kernel;

        public UnitTestBase()
        {
            _kernel = new MoqMockingKernel();
            NinjectHttpContainer.RegisterModules(_kernel, injectModules);
           
        }

        [TestInitialize]
        public void Initialize()
        {
            _kernel.Reset();
        }

        //Return Lists of Modules in the Application
        List<NinjectModule> injectModules
        {
            get
            {
                return new List<NinjectModule>() { new EliteServicesModule()
                    , new SharedModule()
                    , new MainModule()
                    , new EspressoModule() };
            }
        }
    }
}
