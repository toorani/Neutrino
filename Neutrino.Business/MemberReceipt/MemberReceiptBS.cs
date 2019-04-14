
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
    public class MemberReceiptBS : NeutrinoBusinessService, IMemberReceiptBS
    {

        #region [ Constructor(s) ]
        public MemberReceiptBS(NeutrinoUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<List<MemberReceipt>>> LoadListAsync(int year, int month)
        {
            var result = new BusinessResultValue<List<MemberReceipt>>();
            try
            {
                result.ResultValue = await unitOfWork.MemberReceiptDataService.GetAsync(where: x => x.Year == year && x.Month == month);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<MemberReceipt>> LoadLatestYearMonthAsync()
        {
            var result = new BusinessResultValue<MemberReceipt>();
            try
            {
                result.ResultValue = await unitOfWork.MemberReceiptDataService.GetLatestYearMonthAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<int>> AddBatchAsync(List<MemberReceipt> lstEntities)
        {
            var result = new BusinessResultValue<int>();
            try
            {
                result.ResultValue = await unitOfWork.MemberReceiptDataService.InsertBulkAsync(lstEntities);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<MemberReceipt>>> LoadMonthYearListAsync()
        {
            var result = new BusinessResultValue<List<MemberReceipt>>();
            try
            {
                result.ResultValue = await unitOfWork.MemberReceiptDataService.GetMonthYearListAsync();
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
