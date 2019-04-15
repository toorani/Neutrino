using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IQuantityConditionDS : IEntityRepository<QuantityCondition>
    {
        Task<QuantityCondition> GetQuantityConditionAsync(int goalId);
        Task<List<QuantityCondition>> GetQuantityConditionListAsync(List<int> goalIds);
    }
}
