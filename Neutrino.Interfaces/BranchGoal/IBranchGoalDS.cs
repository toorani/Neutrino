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
    }
}
