using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Elite.Neutrino.Services
{
    [ServiceContract]
    public interface IEliteService
    {
        /// <summary>
        ///  اطلاعات شرکت های طرف حساب شرکت الیت
        /// </summary>
        /// <param name="userName">the valid username</param>
        /// <param name="password">the valid password</param>
        /// <param name="startDate">
        ///  در صورتی که این فیلد مقداری نداشته باشد یعنی شروع دریافت رکورد از اولین رکورد موجود در بانک اطلاعاتی میباشد
        /// </param>
        /// <param name="endDate">
        /// تاریخ انتها جهت دریافت اطلاعات  
        /// </param>
        /// <returns>
        ///    لیست شرکت هایی ثبت شده در محدوده تاریخی مشخص شده در تاریخ شروع و تاریخ پایان
        /// </returns>
        [OperationContract]
        List<CompanyInfo> GetCompanies(string userName, string password, DateTime? startDate, DateTime endDate);
        /// <summary>
        ///  محصولات ثبت شده شرکت های تامین کننده 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="companyId">شناسه شرکت تامین کننده</param>
        /// <returns>
        /// لیست محصولات شرکت مشخص شده برگشت داده میشود
        /// </returns>
        [OperationContract]
        List<GoodsInfo> GetGoods(string userName, string password, int companyId);
        /// <summary>
        /// اطلاعات شعبات و مراکز پخش الیت را مشخص مینماید
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>
        /// لیستی از مراکز پخش موجود 
        /// </returns>
        [OperationContract]
        List<BranchInfo> GetBranches(string userName, string password);
        /// <summary>
        /// اطلاعات کارمندان مراکز 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchId"></param>
        /// <returns>
        ///  لیستی از اطلاعات کارمندان به تفکیک مراکز 
        ///  که از تاریخ شروع تا تاریخ پایان در مرکز اعلام شده شروع به کار کرده اند
        /// </returns>
        [OperationContract]
        List<MemberInfo> GetMembers(string userName, string password, DateTime? startDate, DateTime endDate,int branchId);
        /// <summary>
        /// اطلاعات فروش هر مرکز 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>
        /// لیست اطلاعات فروش تمام مراکز در محدود تاریخی مشخص شده
        /// </returns>
        [OperationContract]
        List<BranchSalesInfo> GetBranchSales(string userName, string password, int year, int month);
        /// <summary>
        /// اطلاعات فروش هر ویزیتور 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>
        /// لیست فروش هر ویزیتور در ماه مشخص شده
        /// </returns>
        [OperationContract]
        List<InvoiceInfo> GetInvoices(string userName, string password, int year, int month);
        /// <summary>
        /// اطلاعات دستمزد کارمندان 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>
        /// لیست اطلاعات دستمزد کارمندان در هرماه 
        /// </returns>
        [OperationContract]
        List<Payroll> GetPayroll(string userName, string password,int year,int month);
        /// <summary>
        /// اطلاعات وصول هر شخص
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>
        /// لیست وصول شخص 
        /// </returns>
        [OperationContract]
        List<MemberReceiptInfo> GetMemberReceipts(string userName, string password, int year, int month);
        /// <summary>
        /// اطلاعات وصول هر مرکز
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>
        /// لیست اطلاعات وصول هر مرکز 
        /// </returns>
        [OperationContract]
        List<BranchReceiptInfo> GetBranchReceipts(string userName, string password, int year,int month,int branchId);
        /// <summary>
        /// قیمت کالا با احتساب جوایز
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// لیست قیمت کالا با احتساب جوایز
        /// </returns>
        [OperationContract]
        List<GoodsPrice> GetGoodsPrice(string userName, string password, int year, int month);
        /// <summary>
        /// اطلاعات هزینه سربار مراکز
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// لیست اطلاعات هزینه سربار مراکز
        /// </returns>
        [OperationContract]
        List<OverheadCost> GetOverheadCost(string userName, string password, int year, int month);
        /// <summary>
        /// اطلاعات فروش به تفکیک هر فروشنده
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// لیست اطلاعات فروش هر فروشنده
        /// </returns>
        [OperationContract]
        List<MemberSalesInfo> GetMemberSalesInfo(string userName, string password, int year, int month);
        /// <summary>
        /// اطلاعات اهداف تعداد مشتری
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// لیست اطلاعات اهداف تعداد مشتری
        /// </returns>
        [OperationContract]
        List<CustomerGoalsInfo> GetCustomerGoalsInfo(string userName, string password, int year, int month);

    }

    
}

