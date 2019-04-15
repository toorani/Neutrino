using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IBranchReceiptGoalPercentBS : IBusinessService
    {
        Task<IBusinessResultValue<BranchReceiptGoalPercentDTO>> CreateOrUpdateAsync(BranchReceiptGoalPercentDTO branchReceiptGoalPercentDTO);
        Task<IBusinessResultValue<List<Branch>>> LoadNotSpecifiedBranchesAsync(int goalId);
        Task<IBusinessResultValue<Goal>> LoadReceiptGoalAsync(int goalId);
        Task<IBusinessResult> DeleteAsync(BranchReceiptGoalPercentDTO branchReceiptGoalPercentDTO);
    }
}
