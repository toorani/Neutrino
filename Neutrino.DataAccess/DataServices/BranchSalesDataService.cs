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
    public class BranchSalesDataService : NeutrinoRepositoryBase<BranchSales>, IBranchSalesDS
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Constructor(s) ]
        public BranchSalesDataService(NeutrinoContext context)
            : base(context)
        {

        }

        #endregion

        #region [ Public Method(s) ]
        public async Task<BranchSales> GetLatestYearMonthAsync()
        {
            int maxYear = await dbContext.BranchSales
                .Where(x => !x.Deleted)
                .Select(x => x.Year)
                .DefaultIfEmpty()
                .MaxAsync();
            if (maxYear != 0)
            {
                int maxMonth = await dbContext.BranchSales
                .Where(x => !x.Deleted && x.Year == maxYear)
                .Select(x => x.Month)
                .MaxAsync();

                return new BranchSales() { Year = maxYear, Month = maxMonth };
            }
            return new BranchSales();

        }

        public async Task<List<BranchSales>> GetMonthYearListAsync()
        {
            var lst = await (from branchRe in dbContext.BranchSales
                             where branchRe.Deleted == false
                             select new
                             {
                                 Month = branchRe.Month,
                                 Year = branchRe.Year
                             })
                    .Distinct()
                    .ToListAsync();

            return lst.Select(x => new BranchSales() { Month = x.Month, Year = x.Year })
                    .ToList();
        }

        public async Task<List<BranchSales>> GetTotalSalesPerBranchAsync(int year, int month)
        {
            var query = await (from br in dbContext.BranchSales
                               where br.Month == month && br.Year == year
                               group br by br.BranchId into grp
                               select new
                               {
                                   BranchId = grp.Key,
                                   TotalAmount = grp.Sum(x => x.TotalAmount)
                               }).ToListAsync();
            var resultValue = query.Select(x => new BranchSales
            {
                BranchId = x.BranchId,
                TotalAmount = x.TotalAmount,
                Year = year,
                Month = month
            }).ToList();

            return resultValue;
        }
        #endregion


    }
}
