using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IBranchGoalDS : IEntityRepository<BranchGoal>
    {
        Task<List<BranchGoalItem>> GetBranchGoalListAsync(int goalId);
        //Task<Tuple<List<BranchBenefitIndex>, int>> GetBranchBenefitIndexByPaging(Expression<Func<BranchBenefitIndex, bool>> where = null
        //    , Func<IQueryable<BranchBenefitIndex>, IOrderedQueryable<BranchBenefitIndex>> orderBy = null
        //    , int pageNumber = 0
        //    , int pageSize = 15);
    }
}
