using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IGoalGoodsCategoryDS : IEntityBaseRepository<GoalGoodsCategory>
    {
        Task<GoalGoodsCategory> GetGoalGoodsCategoryAsync(int goalGoodsCategoryId);
    }
}
