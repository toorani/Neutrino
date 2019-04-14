using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class BranchReceiptBS : NeutrinoBusinessService, IBranchReceiptBS
    {

        #region [ Constructor(s) ]
        public BranchReceiptBS(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork) { }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<BranchReceipt>> LoadBranchReceiptAsync(int branchId, int year, int month)
        {
            var result = new BusinessResultValue<BranchReceipt>();
            try
            {
                result.ResultValue = await unitOfWork.BranchReceiptDataService.FirstOrDefaultAsync(where: x => x.Year == year && x.Month == month && x.BranchId == branchId);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<BranchReceipt>>> LoadBranchReceiptListAsync(int year, int month)
        {
            var result = new BusinessResultValue<List<BranchReceipt>>();
            try
            {
                result.ResultValue = await unitOfWork.BranchReceiptDataService.GetAsync(where: x => x.Year == year && x.Month == month);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<BranchReceipt>> LoadLatestYearMonthAsync()
        {
            var result = new BusinessResultValue<BranchReceipt>();
            try
            {
                result.ResultValue = await unitOfWork.BranchReceiptDataService.GetLatestYearMonthAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<int>> AddBatchBranchReceiptAsync(List<BranchReceipt> lstEntities)
        {
            var result = new BusinessResultValue<int>();

            try
            {
                result.ResultValue = await unitOfWork.BranchReceiptDataService.InsertBulkAsync(lstEntities);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<BranchReceipt>>> LoadMonthYearListAsync()
        {
            var result = new BusinessResultValue<List<BranchReceipt>>();
            try
            {
                result.ResultValue = await unitOfWork.BranchReceiptDataService.GetMonthYearListAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        #endregion
    }
}
