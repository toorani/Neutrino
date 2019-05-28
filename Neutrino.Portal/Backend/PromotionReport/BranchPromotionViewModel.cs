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
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalPromotion { get; set; }
        /// <summary>
        /// درصد دست یافته شده هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptReachedPercent { get; set; }
        /// <summary>
        ///  درصد دستیابی هدف تجمیعی
        /// </summary>
        public decimal TotalAndAggregationReached { get; set; }
        public decimal TotalSalesPromotion { get; set; }
        public decimal PrivateReceiptPromotion { get; set; }
        public decimal TotalReceiptPromotion { get; set; }


        #region [ Constructor(s) ]
        public BranchPromotionViewModel() : base()
        {

        }
        #endregion

    }
}