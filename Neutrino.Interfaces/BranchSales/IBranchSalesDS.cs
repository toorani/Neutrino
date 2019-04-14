using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IBranchSalesDS : IEntityRepository<BranchSales>
    {
        Task<BranchSales> GetLatestYearMonthAsync();
        Task<List<BranchSales>> GetMonthYearListAsync();
        Task<List<BranchSales>> GetTotalSalesPerBranchAsync(int year, int month);
    }
}
