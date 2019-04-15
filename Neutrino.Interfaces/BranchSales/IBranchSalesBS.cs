using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IBranchSalesBS : IBusinessService
    {
        Task<IBusinessResultValue<BranchSales>> LoadAsync(int branchId, int year, int month);
        Task<IBusinessResultValue<List<BranchSales>>> LoadListAsync(int year, int month);
        Task<IBusinessResultValue<BranchSales>> LoadLatestYearMonthAsync();
        Task<IBusinessResultValue<int>> AddBatchAsync(List<BranchSales> lstEntities);
        Task<IBusinessResultValue<List<BranchSales>>> LoadMonthYearListAsync();
        Task<IBusinessResultValue<List<BranchSales>>> LoadTotalSalesPerBranchAsync(int year, int month);
        
    }
}
