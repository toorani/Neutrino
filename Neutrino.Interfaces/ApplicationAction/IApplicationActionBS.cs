using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IApplicationActionBS : IBusinessService,IEnabledEntityListLoader<ApplicationAction>
    {
        Task<IBusinessResult> CreateOrModify(List<ApplicationAction> applicationActions);
        
    }
}
