using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IGoalBS : IBusinessService
        ,IEnabledEntityLoader<Goal>
        ,IEnabledEntityListByPagingLoader<Goal>
        , IEnabledEntityListLoader<Goal>
    {
        Task<IBusinessResultValue<Goal>> LoadGoalAync(int goalId);
        Task<IBusinessResult> DeleteGoalAsync(Goal goalEntity);
        Task<IBusinessResultValue<Goal>> CreateGoalAsync(Goal goalEntity);
        Task<IBusinessResult> UpdateGoalAsync(Goal goalEntity);
        Task<IBusinessResultValue<decimal>> LoadPreviousAggregationValueAync(int month, int year);

    }
}
