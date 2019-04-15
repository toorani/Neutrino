using System;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject.Extensions.Logging;

namespace Neutrino.Business
{
    public class AppMenuBS : BusinessServiceBase, IAppMenuBS
    {
        private IEntityListLoader<AppMenu> entityListLoader;
        private readonly IDataRepository<AppMenu> dataRepository;
        public IEntityListLoader<AppMenu> EntityListLoader
        {
            get
            {
                entityListLoader = new EntityLoad
            }
        }

        #region [ Constructor(s) ]
        public AppMenuBS(IDataRepository<AppMenu> dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        #endregion
    }
}
