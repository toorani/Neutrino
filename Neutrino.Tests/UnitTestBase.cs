using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.MockingKernel.Moq;

namespace Neutrino.Portal.Tests
{
    public class UnitTestBase
    {
        protected readonly MoqMockingKernel _kernel;

        public UnitTestBase()
        {
            _kernel = new MoqMockingKernel();
            SharedModule.Bind(_kernel);
            MainModule.Bind(_kernel);
            EliteServicesModule.Bind(_kernel);
            EspressoModule.Bind(_kernel);
        }

        [TestInitialize]
        public void Initialize()
        {
            _kernel.Reset();
        }
    }
}
