using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IGoalStepBS : IBusinessService,
        IEnabledEntityListLoader<GoalStep>
    {
        Task<IBusinessResultValue<GoalStep>> CreateGoalStepAsync(GoalStep goalStepEntity);
        Task<IBusinessResult> UpdateGoalStepAsync(GoalStep goalStepEntity);
        Task<IBusinessResult> DeleteGoalStepAsync(GoalStep goalStepEntity);
    }
}
