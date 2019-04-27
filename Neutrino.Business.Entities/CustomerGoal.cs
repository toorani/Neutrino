using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات اهداف تعداد مشتری
    /// </summary>
    public class CustomerGoal : EntityBase
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public int BranchRefId { get; set; }
        /// <summary>
        /// شناسه فروشنده
        /// </summary>
        public int SellerId { get; set; }
        public int SellerRefId { get; set; }
        public virtual Member Seller { get; set; }
        /// <summary>
        /// شناسه کالا
        /// </summary>
        public int GoodsId { get; set; }
        public int GoodsRefId { get; set; }
        public virtual Goods Goods { get; set; }
        /// <summary>
        /// تعداد مشتری
        /// </summary>
        public int CustomerCount { get; set; }
        /// <summary>
        /// تعداد مشتری های به هدف رسیده
        /// </summary>
        public int ReachedCount { get; set; }
        /// <summary>
        /// درصد تحقق
        /// </summary>
        public decimal ReachedPercent { get; set; }
        /// <summary>
        /// تعداد فروش
        /// </summary>
        public int SalesCount { get; set; }
        /// <summary>
        /// مبلغ پاداش
        /// </summary>
        public decimal Promotion { get; set; }
        /// <summary>
        /// ماه مربوط به هدف
        /// </summary>
        public int Month { get; set; }
        public int Year { get; set; }

    }
}