using System.Data.Entity;
using System.Linq;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class BranchGoalDataService : NeutrinoRepositoryBase<BranchGoal>, IBranchGoalDS
    {
        #region [ Constructor(s) ]
        public BranchGoalDataService(NeutrinoContext context)
            : base(context)
        { }
        #endregion

        #region [ Override Method(s) ]
        public override void InsertOrUpdate(BranchGoal entity)
        {
            //entity.GoalGoodsCategory
            //    .ToList<GoalGoodsCategory>()
            //    .ForEach(x =>
            //    {
            //        dbContext.Entry(x).State = EntityState.Modified;
            //    });

            if (entity.Id == 0) // insert
            {
                dbContext.Entry<BranchGoal>(entity).State = EntityState.Added;
            }
            else // update
            {
                dbContext.Entry<BranchGoal>(entity).State = EntityState.Modified;
            }
            base.InsertOrUpdate(entity);
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<List<BranchGoalItem>> GetBranchGoalListAsync(int goalId)
        {
            var query = await (from br in dbContext.Branches
                               join brBnf in dbContext.BranchGoals
                               on new { branchId = br.Id, _goalId = goalId }
                               equals new { branchId = brBnf.BranchId, _goalId = brBnf.GoalId } into bra
                               from bfit in bra.DefaultIfEmpty()
                               orderby br.Order
                               select new BranchGoalItem()
                               {
                                   BranchId = br.Id,
                                   BranchName = br.Name,
                                   Percent = bfit.Percent,
                                   Amount = bfit.Amount,
                                   BranchGoalId = bfit.Id,
                                   GoalId = goalId
                               })
                               //.OrderBy(x => x.)
                               .ToListAsync();
            return query;
        }
        #endregion

    }
}
