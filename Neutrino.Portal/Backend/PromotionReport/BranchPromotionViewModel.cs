using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class BranchPromotionViewModel : ViewModelBase
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Branch { get; set; }
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

        #region [ Constructor(s) ]
        public BranchPromotionViewModel():base()
        {

        }
        #endregion

    }
}