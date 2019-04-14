using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class QuantityConditionLoader : GeneralEntityListLoader<QuantityCondition>, IEntityListLoader<QuantityCondition>
    {
        public QuantityConditionLoader(IDataRepository<QuantityCondition> dataRepository, IBusinessService businessService) 
            : base(dataRepository, businessService)
        {
        }


       
    }
}
