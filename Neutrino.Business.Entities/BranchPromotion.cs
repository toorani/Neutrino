using Espresso.Entites;

namespace Neutrino.Entities
{
    public class BranchPromotion : EntityBase
    {
        //شناسه پورسانت
        public int PromotionId { get; set; }
        public virtual Promotion Promotion { get; set; }
        /// <summary>
        /// شناسه شعبه
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// ماه محاسبه شده
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// سال محاسبه شده
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        ///  مقدار مشخص شده مرکز برای دستیابی به هدف وصول کل
        /// </summary>
        public decimal TotalReceiptSpecifiedAmount { get; set; }
        /// <summary>
        /// مبلغ دست یافته شده برای وصول کل 
        /// </summary>
        public decimal TotalReceiptAmount { get; set; }
        /// <summary>
        /// درصد دست یافته شده هدف وصول کل
        /// </summary>
        public decimal TotalReceiptReachedPercent { get; set; }
        /// <summary>
        /// درصد پورسانت دستیابی شده هدف وصول کل
        /// </summary>
        public decimal TotalReceiptPromotionPercent { get; set; }
        /// <summary>
        /// مبلغ پورسانت دستیابی شده هدف وصول کل
        /// </summary>
        public decimal TotalReceiptPromotion { get; set; }

        /// <summary>
        ///  مقدار مشخص شده مرکز برای دستیابی به هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptAmountSpecified { get; set; }
        /// <summary>
        /// مبلغ دست یافته شده برای وصول خصوصی 
        /// </summary>
        public decimal PrivateReceiptAmount { get; set; }
        /// <summary>
        /// درصد دست یافته شده هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptReachedPercent { get; set; }
        /// <summary>
        /// درصد پورسانت دریافت شده هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptPromotionPercent { get; set; }
        /// <summary>
        /// مبلغ پورسانت دریافت شده هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptPromotion { get; set; }

        /// <summary>
        /// مبلغ کل فروش
        /// </summary>
        public decimal TotalSalesAmount { get; set; }
        /// <summary>
        ///  درصد دستیابی هدف کل فروش
        /// </summary>
        public decimal TotalSalesReachedPercent { get; set; }
        /// <summary>
        /// درصد مشخص شده مرکز برای هدف کل فروش
        /// </summary>
        public decimal TotalSalesSpecifiedPercent { get; set; }
        /// <summary>
        /// درصد مشخص شده مرکز برای هدف کل فروش
        /// </summary>
        public decimal TotalSalesSpecifiedAmount { get; set; }
        /// <summary>
        /// درصد پورسانت دریافت شده هدف کل فروش
        /// </summary>
        public decimal TotalSalesPromotionPercent { get; set; }
        /// <summary>
        /// مبلغ پورسانت دریافت شده هدف کل فروش
        /// </summary>
        public decimal TotalSalesPromotion { get; set; }
        /// <summary>
        /// مبلغ پورسانت دریافت شده از اهداف گروهی / تکی
        /// </summary>
        public decimal SalesPromotion { get; set; }

        #region [ Constructor(s) ]

        #endregion
    }
}
