
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Data.EntityFramework.DataServices;
using System.Linq.Expressions;
using Espresso.BusinessService.Interfaces;
using Ninject;

namespace Neutrino.Business
{
    public class BranchBS : BusinessServiceBase,IBranchBS
    {
        #region [ Varibale(s) ]
        private readonly IEntityRepository<Branch> dataService;
        #endregion

        #region [ Public Property(ies) ]
        [Inject]
        public IEntityListLoader<Branch> EntityListLoader { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public BranchBS(IEntityRepository<Branch> dataService)
        {
            this.dataService = dataService;
            
        }
        #endregion


    }
}
