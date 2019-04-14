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
    public interface IMemberReceiptBS : IBusinessService
    {
        Task<IBusinessResultValue<MemberReceipt>> LoadLatestYearMonthAsync();
        Task<IBusinessResultValue<List<MemberReceipt>>> LoadListAsync(int year, int month);
        Task<IBusinessResultValue<int>> AddBatchAsync(List<MemberReceipt> lstEntities);
        Task<IBusinessResultValue<List<MemberReceipt>>> LoadMonthYearListAsync();

    }
}
