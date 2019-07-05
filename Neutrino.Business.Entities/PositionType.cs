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
        [Description("مدیر فروش مناطق")]
        SalesManager = 1009,
        /// <summary>
        /// پرسنل فروش منطقه یک
        /// </summary>
        [Description("پرسنل فروش مناطق")]
        SalesEmployee = 1010,
        /// <summary>
        /// فروشنده
        /// </summary>
        [Description("فروشنده")]
        Seller = 1013,
        /// <summary>
        /// سرپرست فروش
        /// </summary>
        [Description("سرپرست فروش")]
        SalesSupervisor = 1014,
        /// <summary>
        /// کارمند حسابداری
        /// </summary>
        [Description("کارمند حسابداری")]
        AccountantEmployee = 1016,
        /// <summary>
        /// سرپرست حسابداری
        /// </summary>
        [Description("سرپرست حسابداری")]
        AccountantSupervisor = 1017,
        /// <summary>
        /// رئیس مرکز
        /// </summary>
        [Description("رئیس مرکز")]
        BranchManager = 1018,
        /// <summary>
        /// مدیر  عملیات
        /// </summary>
        [Description("مدیر عملیات")]
        OperationManager = 1020,
        /// <summary>
        /// پرسنل عملیات
        /// </summary>
        [Description("پرسنل عملیات")]
        OperationEmployee = 1021,
        /// <summary>
        /// راننده 
        /// </summary>
        [Description("راننده/موزع")]
        DriverAndCarrier = 1027

    }

    public class PositionType : EnumEntity<PositionTypeEnum>
    {
        public PositionType(PositionTypeEnum enumType) : base(enumType)
        {
        }

        public PositionType() : base() { } // should excplicitly define for EF!
    }
}
