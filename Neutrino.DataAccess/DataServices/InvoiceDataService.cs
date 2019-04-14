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
    public class InvoiceDataService : NeutrinoRepositoryBase<Invoice>, IInvoiceDS
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Constructor(s) ]
        public InvoiceDataService(NeutrinoContext context)
            : base(context)
        {
            
        }

        #endregion

        #region [ Public Method(s) ]

        public async Task<Invoice> GetLatestYearMonthAsync()
        {
            int maxYear = await dbContext.Invoices
                .Where(x => !x.Deleted)
                .Select(x => x.Year)
                .DefaultIfEmpty()
                .MaxAsync();
            if (maxYear != 0)
            {
                int maxMonth = await dbContext.Invoices
                .Where(x => !x.Deleted && x.Year == maxYear)
                .Select(x => x.Month)
                .MaxAsync();

                return new Invoice() { Year = maxYear, Month = maxMonth };
            }
            return new Invoice();
        }
        public async Task<List<Invoice>> GetMonthYearListAsync()
        {
            var lst = await (from branchRe in dbContext.Invoices
                             where branchRe.Deleted == false
                             select new
                             {
                                 Month = branchRe.Month,
                                 Year = branchRe.Year
                             })
                    .Distinct()
                    .ToListAsync();

            return lst.Select(x => new Invoice() { Month = x.Month, Year = x.Year })
                    .ToList();
        }

        #endregion
    }
}
