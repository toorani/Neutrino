using System;
using System.Threading.Tasks;
using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections.Generic;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class MemberPayrollDataService : NeutrinoRepositoryBase<MemberPayroll>, IMemberPayrollDS
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Constructor(s) ]
        public MemberPayrollDataService(NeutrinoContext context)
            : base(context)
        {
            
        }

        #endregion

        #region [ Public Method(s) ]
        public async Task<MemberPayroll> GetLatestYearMonthAsync()
        {
            int maxYear = await dbContext.Payrolls
                .Where(x => !x.Deleted)
                .Select(x => x.Year)
                .DefaultIfEmpty()
                .MaxAsync();
            if (maxYear != 0)
            {
                int maxMonth = await dbContext.Payrolls
                .Where(x => !x.Deleted && x.Year == maxYear)
                .Select(x => x.Month)
                .MaxAsync();

                return new MemberPayroll() { Year = maxYear, Month = maxMonth };
            }

            return new MemberPayroll();
        }
        public async Task<List<MemberPayroll>> GetMonthYearListAsync()
        {
            var lst = await (from branchRe in dbContext.Payrolls
                             where branchRe.Deleted == false
                             select new
                             {
                                 Month = branchRe.Month,
                                 Year = branchRe.Year
                             })
                    .Distinct()
                    .ToListAsync();

            return lst.Select(x => new MemberPayroll() { Month = x.Month, Year = x.Year })
                    .ToList(); ;
        }
        #endregion

    }
}
