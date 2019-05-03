using Espresso.Entites;
using System;
using System.Collections.Generic;

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
        public virtual Branch Branch { get; set; }
        /// <summary>
        /// ماه محاسبه شده
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// سال محاسبه شده
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        ///  مقدار مشخص شده مرکز برای دستیابی به هدف وصول کل
        /// </summary>
        public decimal TotalReceiptSpecifiedAmount { get; set; }
        /// <summary>
        /// درصد دست یافته شده هدف وصول کل
        /// </summary>
        public decimal TotalReceiptReachedPercent { get; set; }
        /// <summary>
        ///  مقدار مشخص شده مرکز برای دستیابی به هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptSpecifiedAmount { get; set; }
        /// <summary>
        /// مبلغ دست یافته شده برای وصول خصوصی 
        /// </summary>
        public decimal PrivateReceiptAmount { get; set; }
        /// <summary>
        /// درصد دست یافته شده هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptReachedPercent { get; set; }
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
        /// مبلغ کل تجمیعی
        /// </summary>
        public decimal AggregationSalesAmount { get; set; }
        /// <summary>
        ///  درصد دستیابی هدف تجمیعی
        /// </summary>
        public decimal AggregationReachedPercent { get; set; }
        
        /// <summary>
        /// مقدار مشخص شده مرکز برای هدف تجمیعی
        /// </summary>
        public decimal AggregationSpecifiedAmount { get; set; }

        public virtual ICollection<BranchGoalPromotion> BranchGoalPromotions { get; private set; }
        public virtual ICollection<MemberPromotion> MemberPromotions { get; private set; }

        #region [ Constructor(s) ]
        public BranchPromotion()
        {
            BranchGoalPromotions = new HashSet<BranchGoalPromotion>();
            MemberPromotions = new HashSet<MemberPromotion>();
        }
        #endregion
    }
}
