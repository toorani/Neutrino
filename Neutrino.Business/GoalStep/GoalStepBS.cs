using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;

namespace Neutrino.Business
{
    public class GoalStepBS : NeutrinoBusinessService, IGoalStepBS
    {
        #region [ Varibale(s) ]
        private readonly IEntityCreator<GoalStep> goalStepCreator;
        private readonly IEntityModifer<GoalStep> goalStepUpdater;
        private readonly IEntityEraser<GoalStep> goalStepEraser;
        #endregion

        #region [ Public Property(ies) ]
        [Inject]
        public IEntityListLoader<GoalStep> EntityListLoader { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public GoalStepBS(IEntityCreator<GoalStep> goalStepCreator
            , IEntityModifer<GoalStep> goalStepUpdater
            , IEntityEraser<GoalStep> goalStepEraser
            , NeutrinoUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.goalStepCreator = goalStepCreator;
            this.goalStepUpdater = goalStepUpdater;
            this.goalStepEraser = goalStepEraser;
        }
        #endregion

        public Task<IBusinessResultValue<GoalStep>> CreateGoalStepAsync(GoalStep goalStepEntity)
        {
            if (goalStepEntity.GoalTypeId == GoalTypeEnum.Supplier)
            {
                goalStepEntity.ComputingValue = goalStepEntity.RawComputingValue + (goalStepEntity.RawComputingValue * goalStepEntity.IncrementPercent.Value / 100);
            }
            return goalStepCreator.CreateAsync(goalStepEntity);
        }
        public Task<IBusinessResult> UpdateGoalStepAsync(GoalStep goalStepEntity)
        {
            if (goalStepEntity.GoalTypeId == GoalTypeEnum.Supplier)
            {
                goalStepEntity.ComputingValue = goalStepEntity.RawComputingValue + (goalStepEntity.RawComputingValue * goalStepEntity.IncrementPercent.Value / 100);
            }
            return goalStepUpdater.UpdateAsync(goalStepEntity);
        }
        public Task<IBusinessResult> DeleteGoalStepAsync(GoalStep goalStepEntity)
        {
            return goalStepEraser.DeleteAsync(goalStepEntity);
        }
    }
}
