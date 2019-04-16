namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات اهداف تعداد مشتری
    /// </summary>
    public class CustomerGoalsInfo
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// شناسه فروشنده
        /// </summary>
        public int SellerId { get; set; }
        /// <summary>
        /// شناسه کالا
        /// </summary>
        public int GoodsId { get; set; }
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
    }
}