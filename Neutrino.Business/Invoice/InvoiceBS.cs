
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class InvoiceBS : NeutrinoBusinessService, IInvoiceBS
    {
        #region [ Constructor(s) ]
        public InvoiceBS(NeutrinoUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<Invoice>> LoadLatestYearMonthAsync()
        {
            var result = new BusinessResultValue<Invoice>();
            try
            {
                result.ResultValue = await unitOfWork.InvoiceDataService.GetLatestYearMonthAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<int>> AddBatchAsync(List<Invoice> lstData)
        {
            var result = new BusinessResultValue<int>();
            try
            {
                result.ResultValue = await unitOfWork.InvoiceDataService.InsertBulkAsync(lstData);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<Invoice>>> LoadMonthYearListAsync()
        {
            var result = new BusinessResultValue<List<Invoice>>();
            try
            {
                result.ResultValue = await unitOfWork.InvoiceDataService.GetMonthYearListAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<Invoice>>> LoadMonthYearListAsync(int year, int month)
        {
            var result = new BusinessResultValue<List<Invoice>>();
            try
            {
                result.ResultValue = await unitOfWork.InvoiceDataService.GetAsync(where: x => x.Year == year && x.Month == month);
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
