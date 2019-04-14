using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class BranchSalesBS : NeutrinoBusinessService, IBranchSalesBS
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Constructor(s) ]
        public BranchSalesBS(NeutrinoUnitOfWork unitOfWork):base(unitOfWork)
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<BranchSales>> LoadAsync(int branchId, int year, int month)
        {
            var result = new BusinessResultValue<BranchSales>();
            try
            {
                result.ResultValue = await unitOfWork.BranchSalesDataService.FirstOrDefaultAsync(where: x => x.Year == year && x.Month == month && x.BranchId == branchId);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<BranchSales>>> LoadListAsync(int year, int month)
        {
            var result = new BusinessResultValue<List<BranchSales>>();
            try
            {
                result.ResultValue = await unitOfWork.BranchSalesDataService.GetAsync(where: x => x.Year == year && x.Month == month);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<BranchSales>> LoadLatestYearMonthAsync()
        {
            var result = new BusinessResultValue<BranchSales>();
            try
            {
                result.ResultValue = await unitOfWork.BranchSalesDataService.GetLatestYearMonthAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<int>> AddBatchAsync(List<BranchSales> lstEntities)
        {
            var result = new BusinessResultValue<int>();

            try
            {
                result.ResultValue = await unitOfWork.BranchSalesDataService.InsertBulkAsync(lstEntities);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<BranchSales>>> LoadMonthYearListAsync()
        {
            var result = new BusinessResultValue<List<BranchSales>>();
            try
            {
                result.ResultValue = await unitOfWork.BranchSalesDataService.GetMonthYearListAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<BranchSales>>> LoadTotalSalesPerBranchAsync(int year, int month)
        {
            var result = new BusinessResultValue<List<BranchSales>>();
            try
            {
                result.ResultValue = await unitOfWork.BranchSalesDataService.GetTotalSalesPerBranchAsync(year, month);
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
