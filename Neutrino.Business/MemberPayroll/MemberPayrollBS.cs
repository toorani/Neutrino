
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
    public class MemberPayrollBS : NeutrinoBusinessService, IMemberPayrollBS
    {
        #region [ Constructor(s) ]
        public MemberPayrollBS(NeutrinoUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<List<MemberPayroll>>> LoadListAsync(int year, int month)
        {
            var result = new BusinessResultValue<List<MemberPayroll>>();
            try
            {
                result.ResultValue = await unitOfWork.MemberPayrollDataService.GetAsync(where: x => x.Year == year && x.Month == month);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<MemberPayroll>> LoadLatestYearMonthAsync()
        {
            var result = new BusinessResultValue<MemberPayroll>();
            try
            {
                result.ResultValue = await unitOfWork.MemberPayrollDataService.GetLatestYearMonthAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<int>> AddBatchAsync(List<MemberPayroll> lstEntities)
        {
            var result = new BusinessResultValue<int>();
            try
            {
                result.ResultValue = await unitOfWork.MemberPayrollDataService.InsertBulkAsync(lstEntities);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<MemberPayroll>>> LoadMonthYearListAsync()
        {
            var result = new BusinessResultValue<List<MemberPayroll>>();
            try
            {
                result.ResultValue = await unitOfWork.MemberPayrollDataService.GetMonthYearListAsync();
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
