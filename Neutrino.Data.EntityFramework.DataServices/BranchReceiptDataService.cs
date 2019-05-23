using System.Threading.Tasks;
using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class BranchReceiptDataService : NeutrinoRepositoryBase<BranchReceipt>, IBranchReceiptDS
    {
        #region [ Constructor(s) ]
        public BranchReceiptDataService(NeutrinoContext context)
            : base(context)
        { }

        #endregion

        #region [ Public Method(s) ]
        public async Task<BranchReceipt> GetLatestYearMonthAsync()
        {
            int? maxYear = await dbContext.BranchReceipts
                 .Where(x => !x.Deleted)
                 .Select(x => (int?)x.Year)
                 .MaxAsync();

            if (maxYear.HasValue)
            {
                int maxMonth = await dbContext.BranchReceipts
                .Where(x => !x.Deleted && x.Year == maxYear)
                .Select(x => x.Month)
                .MaxAsync();

                return new BranchReceipt() { Year = maxYear.Value, Month = maxMonth };
            }

            return new BranchReceipt();
        }

        public async Task<List<BranchReceipt>> GetMonthYearListAsync()
        {
            var lst = await (from branchRe in dbContext.BranchReceipts
                             where branchRe.Deleted == false
                             select new
                             {
                                 Month = branchRe.Month,
                                 Year = branchRe.Year
                             })
                    .Distinct()
                    .ToListAsync();
            return lst.Select(x => new BranchReceipt() { Month = x.Month, Year = x.Year }).ToList();
        }

        #endregion

    }
}
