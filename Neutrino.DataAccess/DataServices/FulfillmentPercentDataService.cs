using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Espresso.Identity.Models;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using Z.EntityFramework.Plus;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class FulfillmentPercentDataService : NeutrinoRepositoryBase<FulfillmentPercent>, IFulfillmentPercentDS
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPercentDataService(NeutrinoContext context)
            : base(context)
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<List<FulfillmentPercent>> GetFulfillmentListAsync(int year, int month)
        {
            var result = await (from branch in dbContext.Branches
                                join goalFil in dbContext.FulfillmentPercents
                                on branch.Id equals goalFil.BranchId into jn_br_fullfil
                                from lf_br_fullfil in jn_br_fullfil
                                .Where(x => x.Year == year && x.Month == month)
                                .DefaultIfEmpty()
                                where !branch.Deleted
                                orderby branch.Name
                                select new
                                {
                                    Branch = branch,
                                    FulfillmentPercent = lf_br_fullfil,
                                })
                                .ToListAsync();
            return result.Select(x => new FulfillmentPercent()
            {
                Branch = x.Branch,
                BranchId = x.Branch.Id,
                ManagerFulfillmentPercent = x.FulfillmentPercent != null ? x.FulfillmentPercent.ManagerFulfillmentPercent : 0,
                ManagerReachedPercent = x.FulfillmentPercent != null ? x.FulfillmentPercent.ManagerReachedPercent : 0,
                SellerFulfillmentPercent = x.FulfillmentPercent != null ? x.FulfillmentPercent.SellerFulfillmentPercent : 0,
                SellerReachedPercent = x.FulfillmentPercent != null ? x.FulfillmentPercent.SellerReachedPercent : 0,
                Id = x.FulfillmentPercent != null ? x.FulfillmentPercent.Id : 0,
                IsUsed = x.FulfillmentPercent != null ? x.FulfillmentPercent.IsUsed : false,
                Month = month,
                Year = year,
            }).ToList();

        }

        public void InsertFulfillment(List<FulfillmentPercent> lstGoalFulfillment)
        {
            lstGoalFulfillment.ForEach(goalFulfillment =>
            {
                if (goalFulfillment.Branch != null)
                    dbContext.Entry(goalFulfillment.Branch).State = EntityState.Unchanged;

                if (goalFulfillment.Id == 0)
                {
                    Insert(goalFulfillment);
                }
                else
                {
                    Update(goalFulfillment);
                }
            });

        }
        #endregion
    }
}
