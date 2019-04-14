using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IBranchReceiptBS : IBusinessService
    {
        Task<IBusinessResultValue<BranchReceipt>> LoadBranchReceiptAsync(int branchId, int year, int month);
        Task<IBusinessResultValue<List<BranchReceipt>>> LoadBranchReceiptListAsync(int year, int month);
        Task<IBusinessResultValue<BranchReceipt>> LoadLatestYearMonthAsync();
        Task<IBusinessResultValue<int>> AddBatchBranchReceiptAsync(List<BranchReceipt> lstEntities);

        Task<IBusinessResultValue<List<BranchReceipt>>> LoadMonthYearListAsync();
    }
}
