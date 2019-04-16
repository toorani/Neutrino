using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات اعضا شرکت
    /// </summary>
    [DataContract]
    public class MemberInfo
    {
        /// <summary>
        /// کد ملی کارمند
        /// </summary>
        [DataMember]
        public string NationalCode { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// نام خانوادگی 
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// کد کارمند
        /// </summary>
        [DataMember]
        public int Code { get; set; }
        /// <summary>
        /// شناسه مرکز فروش
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }
        /// <summary>
        /// پست سازمانی
        /// </summary>
        [DataMember]
        public Positions Position { get; set; }

    }
    /// <summary>
    /// انواع پست سازمانی 
    /// </summary>
    [DataContract]
    public enum Positions
    {
        /// <summary>
        /// قائم مقام مدیرعامل
        /// </summary>
        [EnumMember]
        ViceCEO = 1000,
        /// <summary>
        /// مدیر منابع انسانی
        /// </summary>
        [EnumMember]
        HRManager,
        /// <summary>
        /// پرسنل منابع انسانی
        /// </summary>
        [EnumMember]
        HREmployee,
        /// <summary>
        /// مدیر بازرگانی
        /// </summary>
        [EnumMember]
        CommercialManager,
        /// <summary>
        /// پرسنل بازرگانی
        /// </summary>
        [EnumMember]
        CommercialEmployee,
        /// <summary>
        /// مدیر انفورماتیک
        /// </summary>
        [EnumMember]
        ITManager,
        /// <summary>
        /// پرسنل انفورماتیک
        /// </summary>
        [EnumMember]
        ITEmployee,
        /// <summary>
        /// مدیر مالی
        /// </summary>
        [EnumMember]
        FinancialManager,
        /// <summary>
        /// پرسنل مالی
        /// </summary>
        [EnumMember]
        FinancialEmployee,
        /// <summary>
        /// مدیر فروش منطقه یک
        /// </summary>
        [EnumMember]
        SalesManager_ZoneOne,
        /// <summary>
        /// پرسنل فروش منطقه یک
        /// </summary>
        [EnumMember]
        SalesEmployee_ZoneOne,
        /// <summary>
        /// مدیر فروش منطقه دو
        /// </summary>
        [EnumMember]
        SalesManager_ZoneTwo,
        /// <summary>
        /// پرسنل فروش منطقه دو
        /// </summary>
        [EnumMember]
        SalesEmployee_ZoneTwo,
        /// <summary>
        /// فروشنده
        /// </summary>
        [EnumMember]
        Seller,
        /// <summary>
        /// سرپرست فروش
        /// </summary>
        [EnumMember]
        SalesSupervisor,
        /// <summary>
        /// اپراتور فروش 
        /// </summary>
        [EnumMember]
        SalesOperator,
        /// <summary>
        /// حسابدار
        /// </summary>
        [EnumMember]
        Accountant,
        /// <summary>
        /// سرپرست حسابداری
        /// </summary>
        [EnumMember]
        AccountantSupervisor,
        /// <summary>
        /// رئیس مرکز
        /// </summary>
        [EnumMember]
        BranchManager,
        /// <summary>
        /// کارمند حسابداری
        /// </summary>
        [EnumMember]
        AccountantEmployee,
        /// <summary>
        /// مسئول عملیات
        /// </summary>
        [EnumMember]
        OperationResponsible,
        /// <summary>
        /// کارمند اداری
        /// </summary>
        [EnumMember]
        Officer,
        /// <summary>
        /// سرپرست انبار 
        /// </summary>
        [EnumMember]
        StoreSupervisor,
        /// <summary>
        /// انباردار
        /// </summary>
        [EnumMember]
        Storekeeper,
        /// <summary>
        /// کمک انباردار 
        /// </summary>
        [EnumMember]
        StorekeeperAssistance,
        /// <summary>
        /// کارگر انبار 
        /// </summary>
        [EnumMember]
        Storeworker,
        /// <summary>
        /// اپراتور انبار 
        /// </summary>
        [EnumMember]
        StoreOperator,
        /// <summary>
        /// راننده 
        /// </summary>
        [EnumMember]
        Driver,
        /// <summary>
        /// موزع 
        /// </summary>
        [EnumMember]
        Carrier,
        /// <summary>
        /// کارگر خدمات
        /// </summary>
        [EnumMember]
        Servant,
        /// <summary>
        /// نگهبان
        /// </summary>
        [EnumMember]
        Gguard


    }
}

