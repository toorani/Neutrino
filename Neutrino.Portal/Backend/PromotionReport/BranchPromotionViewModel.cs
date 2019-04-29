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
        /// درصد دست یافته شده هدف وصول کل
        /// </summary>
        public decimal TotalReceiptReachedPercent { get; set; }
        /// <summary>
        /// درصد دست یافته شده هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptReachedPercent { get; set; }
        /// <summary>
        ///  درصد دستیابی هدف تجمیعی
        /// </summary>
        public decimal TotalAndAggregationReached { get; set; }


        #region [ Constructor(s) ]
        public BranchPromotionViewModel() : base()
        {

        }
        #endregion

    }
}