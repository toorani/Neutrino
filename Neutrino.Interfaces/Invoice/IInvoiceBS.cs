using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IInvoiceBS : IBusinessService
    {
        Task<IBusinessResultValue<Invoice>> LoadLatestYearMonthAsync();
        Task<IBusinessResultValue<int>> AddBatchAsync(List<Invoice> lstData);
        Task<IBusinessResultValue<List<Invoice>>> LoadMonthYearListAsync();
        Task<IBusinessResultValue<List<Invoice>>> LoadMonthYearListAsync(int year,int month);
    }
}
