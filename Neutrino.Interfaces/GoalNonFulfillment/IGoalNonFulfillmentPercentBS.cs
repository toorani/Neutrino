using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IGoalNonFulfillmentPercentBS : IBusinessService
    {
        Task<IBusinessResultValue<Goal>> LoadGoalAsync(int goalId);
        Task<IBusinessResultValue<GoalNonFulfillmentPercent>> CreateOrUpdateAsync(GoalNonFulfillmentPercent entityCreating);
    }
}
