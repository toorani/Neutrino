using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;
using System.Data.Entity;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class GoalGoodsCategoryDataService : NeutrinoRepositoryBase<GoalGoodsCategory>, IGoalGoodsCategoryDS
    {
        #region [ Constructor(s) ]
        public GoalGoodsCategoryDataService(NeutrinoContext context)
            : base(context)
        {
        }

        public async Task<GoalGoodsCategory> GetGoalGoodsCategoryAsync(int goalGoodsCategoryId)
        {
            return await dbContext.GoalGoodsCategories
                .Where(x => x.Id == goalGoodsCategoryId)
                .IncludeFilter(x => x.GoodsCollection.Where(y => y.Deleted == false))
                .IncludeFilter(x => x.GoodsCollection.Select(y => y.Goods))
                .IncludeFilter(x => x.GoodsCollection.Select(y => y.Goods.Company))
                .FirstOrDefaultAsync();
        }
        #endregion



    }
}
