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
    public interface IMemberPayrollBS : IBusinessService
    {
        Task<IBusinessResultValue<MemberPayroll>> LoadLatestYearMonthAsync();
        Task<IBusinessResultValue<List<MemberPayroll>>> LoadListAsync(int year, int month);
        Task<IBusinessResultValue<int>> AddBatchAsync(List<MemberPayroll> lstEntities);
        Task<IBusinessResultValue<List<MemberPayroll>>> LoadMonthYearListAsync();

    }
}
