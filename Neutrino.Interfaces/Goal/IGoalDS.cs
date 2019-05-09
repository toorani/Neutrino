using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IGoalDS : IEntityBaseRepository<Goal>
    {
        Task<Goal> GetGoalAync(int goalId);
        Task<Goal> GetGoalInclude_GoalGoodsCategory_GoalSteps(int goalId);
        Task<decimal> GetPreviousAggregationValueAsync(int month, int year);
        decimal GetPreviousAggregationValue(int month, int year);
        decimal GetNextAggregationValue(int month, int year, decimal thisMonthAmount);
        

    }
}
