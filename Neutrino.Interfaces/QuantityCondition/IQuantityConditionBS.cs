using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IQuantityConditionBS : IBusinessService
    {
        Task<IBusinessResultValue<QuantityCondition>> LoadQuantityConditionAsync(int goalId);
        Task<IBusinessResultValue<QuantityCondition>> AddOrUpdateQuantityConditionAsync(QuantityCondition entity);
        Task<IBusinessResultValue<List<QuantityCondition>>> LoadQuantityConditionListAsync(List<int> goalIds);
        Task<IBusinessResultValue<QuantityConditionTypeEnum?>> LoadQuantityConditionTypeAsync(int goalId);
    }
}
