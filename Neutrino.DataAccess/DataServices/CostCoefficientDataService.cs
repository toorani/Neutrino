using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
namespace Neutrino.Data.EntityFramework.DataServices
{
    public class CostCoefficientDataService : NeutrinoRepositoryBase<CostCoefficient>, ICostCoefficientDS
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Constructor(s) ]
        public CostCoefficientDataService(NeutrinoContext context)
            : base(context)
        {
        }


        #endregion

        #region [ Public Method(s) ]
        public async Task<List<CostCoefficient>> GetCoefficientList()
        {
            List<CostCoefficient> result = new List<CostCoefficient>();
            var left_join = await (from gds in dbContext.GoodsCategoryTypes
                                   join cof in dbContext.CostCoefficients on
                                   gds equals cof.GoodsCategoryType
                                   into cof_gds
                                   from final in cof_gds.Where(x => x.Deleted == false).DefaultIfEmpty()
                                   select new
                                   {
                                       GoodsCategory = gds,
                                       CostCoefficient = final
                                   })
                                   .ToListAsync();
            result = left_join.Select(x => new CostCoefficient
            {
                Coefficient = x.CostCoefficient?.Coefficient,
                GoodsCategoryTypeId = x.GoodsCategory.Id,
                GoodsCategoryType = x.GoodsCategory,
                Id = x.CostCoefficient == null ? 0 : x.CostCoefficient.Id
            }).ToList();


            return result;
        }
        #endregion

        #region [ Override Method(s) ]
        public override void InsertOrUpdate(CostCoefficient entity)
        {
            base.InsertOrUpdate(entity);
        }
        #endregion

    }
}
