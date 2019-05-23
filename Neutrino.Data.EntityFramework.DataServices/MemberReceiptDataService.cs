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
    public class MemberReceiptDataService : NeutrinoRepositoryBase<MemberReceipt>, IMemberReceiptDS
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Constructor(s) ]
        public MemberReceiptDataService(NeutrinoContext context)
            : base(context)
        {
            
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<MemberReceipt> GetLatestYearMonthAsync()
        {
            int maxYear = await dbContext.MemberReceipts
                .Where(x => !x.Deleted)
                .Select(x => x.Year)
                .DefaultIfEmpty()
                .MaxAsync();
            if (maxYear != 0)
            {
                int maxMonth = await dbContext.MemberReceipts
                    .Where(x => !x.Deleted && x.Year == maxYear)
                    .Select(x => x.Month)
                    .MaxAsync();

                return new MemberReceipt() { Year = maxYear, Month = maxMonth };
            }
            return null;
        }

        public async Task<List<MemberReceipt>> GetMonthYearListAsync()
        {
            var lst = await (from branchRe in dbContext.MemberReceipts
                             where branchRe.Deleted == false
                             select new
                             {
                                 Month = branchRe.Month,
                                 Year = branchRe.Year
                             })
                    .Distinct()
                    .ToListAsync();

            return lst.Select(x => new MemberReceipt() { Month = x.Month, Year = x.Year })
                    .ToList(); ;
        }
        #endregion

    }
}
