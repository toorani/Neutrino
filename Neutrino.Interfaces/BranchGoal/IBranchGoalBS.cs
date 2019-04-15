using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IBranchGoalBS : IBusinessService
    {
        Task<IBusinessResultValue<BranchGoalDTO>> LoadBranchGoalDTOAsync(int goalId);
        Task<IBusinessResultValue<BranchGoalDTO>> BatchUpdateAsync(BranchGoalDTO batchData);
        Task<IBusinessResult> BatchDeleteAsync(BranchGoalDTO batchData);
        Task<IBusinessResultValue<BranchGoal>> LoadBranchGoalListAsync(params int[] goalIds);
    }
}
