using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Z.EntityFramework.Plus;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class BranchReceiptGoalPercentDataService : NeutrinoRepositoryBase<BranchReceiptGoalPercent>, IBranchReceiptGoalPercentDS
    {
        public BranchReceiptGoalPercentDataService(NeutrinoContext context) : base(context)
        {
            
        }
        public async Task<List<Branch>> GetNotSpecifiedBranchesAsync(int goalId)
        {
            return await dbContext.Branches
                .Where(br => !dbContext.BranchReceiptGoalPercents.Where(x => x.GoalId == goalId).Select(x => x.BranchId).Contains(br.Id))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<Goal> GetReceiptGoalAsync(int goalId)
        {
            var entity = await dbContext.Goals
                .IncludeFilter(x => x.GoalGoodsCategory)
                .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false))
                .IncludeFilter(x => x.BranchReceiptGoalPercent.Where(y => y.Deleted == false))
                .IncludeFilter(x => x.BranchReceiptGoalPercent.Select(y => y.Branch))
                .FirstOrDefaultAsync(x => x.Id == goalId);

            return entity;
        }
    }
}
