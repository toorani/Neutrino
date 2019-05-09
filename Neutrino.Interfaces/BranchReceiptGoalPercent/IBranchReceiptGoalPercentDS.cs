using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IBranchReceiptGoalPercentDS : IEntityBaseRepository<BranchReceiptGoalPercent>
    {
        Task<List<Branch>> GetNotSpecifiedBranchesAsync(int goalId);
        Task<Goal> GetReceiptGoalAsync(int goalId);
    }
}
