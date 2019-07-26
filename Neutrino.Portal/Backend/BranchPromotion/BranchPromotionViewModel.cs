using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class BranchPromotionViewModel : ViewModelBase
    {
        public int PromotionId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalPromotion { get; set; }
        public decimal PrivateFulfilledPercent { get; set; }
        public decimal TotalReceiptFulfilledPercent { get; set; }
        /// <summary>
        ///  درصد دستیابی هدف تجمیعی
        /// </summary>
        public decimal TotalAndAggregationReached { get; set; }
        /// <summary>
        /// پورسانت فروش
        /// </summary>
        public decimal TotalSalesPromotion { get; set; }
        
        public decimal PrivateReceiptPromotion { get; set; }
        public decimal TotalReceiptPromotion { get; set; }
        /// <summary>
        /// پورسانت ترمیمی
        /// </summary>
        public decimal CompensatoryPromotion { get; set; }
        /// <summary>
        /// پورسانت تامین کننده
        /// </summary>
        public decimal SupplierPromotion { get; set; }
        
        public int PromotionReviewStatusId { get; set; }

        #region [ Constructor(s) ]
        public BranchPromotionViewModel():base()
        {

        }
        #endregion

    }

    
}