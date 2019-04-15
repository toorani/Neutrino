using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IGoalGoodsCategoryBS : IBusinessService
        ,IEnabledEntityListLoader<GoalGoodsCategory>
        , IEnabledEntityLoader<GoalGoodsCategory>
        
    {
        Task<IBusinessResultValue<GoalGoodsCategory>> CreateGoalGoodsCategoryAsync(GoalGoodsCategory goalGoodsCategory,int goalCategorySimilarId);
        Task<IBusinessResultValue<GoalGoodsCategory>> LoadGoalGoodsCategoryAsync(int goalGoodsCategoryId);
        Task<IBusinessResultValue<List<GoalGoodsCategory>>> LoadVisibleGoalGoodsCategoryListAsync(Expression<Func<GoalGoodsCategory, bool>> where = null);
    }
}
