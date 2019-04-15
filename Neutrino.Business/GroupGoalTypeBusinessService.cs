
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core;
using Espresso.Core.Interfaces;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class GroupGoalTypeBusinessService : BusinessServiceBase<GroupGoalType>
    {

        #region [ Protected Property(ies) ]
        protected override AbstractValidator<GroupGoalType> businessRulesService
        {
            get { return null; }
        }
        #endregion

        #region [ Constructor(s) ]

        public GroupGoalTypeBusinessService(IGroupGoalType dataService, ITransactionalData transactionalData)
            :base(dataService, transactionalData)
        {

        }
        
        #endregion





    }
}
