using System.Collections.Generic;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Business;
using Neutrino.Core.SecurityManagement;
using Neutrino.Data.EntityFramework;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject.Modules;

namespace Neutrino.InjectModules
{
    //Main Module For Application
    public class EliteServicesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<NeutrinoContext>().ToSelf().InThreadScope();
            Bind<NeutrinoUnitOfWork>().ToSelf().InThreadScope();
        }
    }
}
