using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IMemberPayrollDS : IEntityRepository<MemberPayroll>
    {
        Task<MemberPayroll> GetLatestYearMonthAsync();
        Task<List<MemberPayroll>> GetMonthYearListAsync();
    }
}
