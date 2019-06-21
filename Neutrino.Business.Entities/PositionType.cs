using System.ComponentModel;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// انواع پست سازمانی 
    /// </summary>
    public enum PositionTypeEnum
    {
        /// <summary>
        /// قائم مقام مدیرعامل
        /// </summary>
        [Description("قائم مقام مدیرعامل")]
        ViceCEO = 1000,
        /// <summary>
        /// مدیر منابع انسانی
        /// </summary>
        [Description("مدیر منابع انسانی")]
        HRManager,
        /// <summary>
        /// پرسنل منابع انسانی
        /// </summary>
        [Description("پرسنل منابع انسانی")]
        HREmployee,
        /// <summary>
        /// مدیر بازرگانی
        /// </summary>
        [Description("مدیر بازرگانی")]
        CommercialManager,
        /// <summary>
        /// پرسنل بازرگانی
        /// </summary>
        [Description("پرسنل بازرگانی")]
        CommercialEmployee,
        /// <summary>
        /// مدیر انفورماتیک
        /// </summary>
        [Description("مدیر انفورماتیک")]
        ITManager,
        /// <summary>
        /// پرسنل انفورماتیک
        /// </summary>
        [Description("پرسنل انفورماتیک")]
        ITEmployee,
        /// <summary>
        /// مدیر مالی
        /// </summary>
        [Description("مدیر مالی")]
        FinancialManager,
        /// <summary>
        /// پرسنل مالی
        /// </summary>
        [Description("پرسنل مالی")]
        FinancialEmployee,
        /// <summary>
        /// مدیر فروش منطقه یک
        /// </summary>
        [Description("مدیر فروش منطقه یک")]
        SalesManager_ZoneOne,
        /// <summary>
        /// پرسنل فروش منطقه یک
        /// </summary>
        [Description("پرسنل فروش منطقه یک")]
        SalesEmployee_ZoneOne,
        /// <summary>
        /// مدیر فروش منطقه دو
        /// </summary>
        [Description("مدیر فروش منطقه دو")]
        SalesManager_ZoneTwo,
        /// <summary>
        /// پرسنل فروش منطقه دو
        /// </summary>
        [Description("پرسنل فروش منطقه دو")]
        SalesEmployee_ZoneTwo,
        /// <summary>
        /// فروشنده
        /// </summary>
        [Description("فروشنده")]
        Seller ,
        /// <summary>
        /// سرپرست فروش
        /// </summary>
        [Description("سرپرست فروش")]
        SalesSupervisor,
        /// <summary>
        /// اپراتور فروش 
        /// </summary>
        [Description("اپراتور فروش")]
        SalesOperator,
        /// <summary>
        /// حسابدار
        /// </summary>
        [Description("حسابدار")]
        Accountant,
        /// <summary>
        /// سرپرست حسابداری
        /// </summary>
        [Description("سرپرست حسابداری")]
        AccountantSupervisor,
        /// <summary>
        /// رئیس مرکز
        /// </summary>
        [Description("رئیس مرکز")]
        BranchManager,
        /// <summary>
        /// کارمند حسابداری
        /// </summary>
        [Description("کارمند حسابداری")]
        AccountantEmployee,
        /// <summary>
        /// مسئول عملیات
        /// </summary>
        [Description("مسئول عملیات")]
        OperationResponsible,
        /// <summary>
        /// کارمند اداری
        /// </summary>
        [Description("کارمند اداری")]
        Officer,
        /// <summary>
        /// سرپرست انبار 
        /// </summary>
        [Description("سرپرست انبار")]
        StoreSupervisor,
        /// <summary>
        /// انباردار
        /// </summary>
        [Description("انباردار")]
        Storekeeper,
        /// <summary>
        /// کمک انباردار 
        /// </summary>
        [Description("کمک انباردار")]
        StorekeeperAssistance,
        /// <summary>
        /// کارگر انبار 
        /// </summary>
        [Description("کارگر انبار")]
        Storeworker,
        /// <summary>
        /// اپراتور انبار 
        /// </summary>
        [Description("اپراتور انبار")]
        StoreOperator,
        /// <summary>
        /// راننده 
        /// </summary>
        [Description("راننده")]
        Driver,
        /// <summary>
        /// موزع 
        /// </summary>
        [Description("موزع")]
        Carrier,
        /// <summary>
        /// کارگر خدمات
        /// </summary>
        [Description("کارگر خدمات")]
        Servant,
        /// <summary>
        /// نگهبان
        /// </summary>
        [Description("نگهبان")]
        Guard,
        /// <summary>
        /// سایر
        /// </summary>
        [Description("سایر")]
        Other

    }

    public class PositionType : EnumEntity<PositionTypeEnum>
    {
        public PositionType(PositionTypeEnum enumType) : base(enumType)
        {
        }

        public PositionType() : base() { } // should excplicitly define for EF!
    }
}
