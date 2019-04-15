
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Data.EntityFramework.DataServices;
using System.Linq.Expressions;

namespace Neutrino.Business
{
    public class MemberBusinessService : NeutrinoBSBase<Member,IMember>
    {
        #region [ Varibale(s) ]
        private MemberPayrollDataService memberPayrollDataService = null;
        private MemberReceiptDataService memberReceiptDataService = null;
        #endregion

        #region [ Constructor(s) ]
        public MemberBusinessService(ITransactionalData transactionalData)
            :base(transactionalData)
        {
            memberPayrollDataService = (MemberPayrollDataService)Neutrino.DependencyResolver.Context.Instance.GetService<IMemberPayroll>();
            memberReceiptDataService = (MemberReceiptDataService)Neutrino.DependencyResolver.Context.Instance.GetService<IMemberReceipt>();
        }
        public MemberBusinessService()
            : base()
        {
            memberPayrollDataService = (MemberPayrollDataService)Neutrino.DependencyResolver.Context.Instance.GetService<IMemberPayroll>();
            memberReceiptDataService = (MemberReceiptDataService)Neutrino.DependencyResolver.Context.Instance.GetService<IMemberReceipt>();
        }

       
        #endregion

        #region [ MemberPayroll Method(s) ]
        public List<MemberPayroll> LoadMemberPayrollList(int year, int month)
        {
            return memberPayrollDataService.Get(where: x => x.Year == year && x.Month == month).ToList();
        }
        public Task<List<MemberPayroll>> LoadMemberPayrollListAsync(int year, int month)
        {
            return memberPayrollDataService.GetAsync(where: x => x.Year == year && x.Month == month);
        }

        public Task<MemberPayroll> LoadLatestPayrollYearMonthAsync()
        {
            return memberPayrollDataService.GetLatestYearMonthAsync();
        }
        public List<MemberPayroll> LoadPayrollMonthYearList()
        {
            return memberPayrollDataService.GetMonthYearList();
        }
        public async Task<int> AddBatchMemberPayrollAsync(List<MemberPayroll> lstEntities)
        {
            TransactionalData = TransactionalData.CreateInstance();
            int result = -1;
            try
            {
                result = await memberPayrollDataService.InsertBulkAsync(lstEntities);
                TransactionalData.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                //TODO log
                TransactionalData.ReturnStatus = false;
                TransactionalData.ReturnMessage.Add(ex.ToString());
                result = -1;
            }
            return result;
        }
        public int AddBatchMemberPayroll(List<MemberPayroll> lstEntities)
        {
            TransactionalData = TransactionalData.CreateInstance();
            int result = -1;
            try
            {
                result = memberPayrollDataService.InsertBulk(lstEntities);
                TransactionalData.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                //TODO log
                TransactionalData.ReturnStatus = false;
                TransactionalData.ReturnMessage.Add(ex.ToString());
                result = -1;
            }
            return result;
        }

        public Task<int> GetMemberPayrollCountAsync(Expression<Func<MemberPayroll, bool>> where)
        {
            return memberPayrollDataService.GetCountAsync(where);
        }

        #endregion


        #region [ MemberReceipt Method(s) ]
        public Task<List<MemberReceipt>> LoadMemberReceiptListAsync(int year, int month)
        {
            return memberReceiptDataService.GetAsync(where: x => x.Year == year && x.Month == month);
        }

        public Task<MemberReceipt> LoadLatestYearMonthAsync()
        {
            return memberReceiptDataService.GetLatestYearMonthAsync();
        }
        public List<MemberReceipt> LoadMonthYearList()
        {
            return memberReceiptDataService.GetMonthYearList();
        }

        public async Task<int> AddBatchMemberReceiptAsync(List<MemberReceipt> lstEntities)
        {
            TransactionalData = TransactionalData.CreateInstance();
            int result = -1;
            try
            {
                result = await memberReceiptDataService.InsertBulkAsync(lstEntities);
                TransactionalData.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                //TODO log
                TransactionalData.ReturnStatus = false;
                TransactionalData.ReturnMessage.Add(ex.ToString());
                result = -1;
            }
            return result;
        }
        
        
        #endregion
    }
}
