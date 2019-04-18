using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class FulfillmentPercentViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        /// <summary>
        /// درصد معیار رییس و سرپرست
        /// </summary>
        public decimal ManagerReachedPercent { get; set; }
        /// <summary>
        /// درصد معیار فروشنده
        /// </summary>
        public decimal SellerReachedPercent { get; set; }
        /// <summary>
        /// درصد مشمول رییس و سرپرست
        /// </summary>
        public decimal? ManagerFulfillmentPercent { get; set; }
        /// <summary>
        /// درصد مشمول فروشنده
        /// </summary>
        public decimal? SellerFulfillmentPercent { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsUsed { get; set; }
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        public BranchViewModel Branch { get; set; }
        /// <summary>
        ///  درصد دستیابی هدف کل فروش یا تجمیعی 
        /// </summary>
        public decimal TotalSalesFulfilledPercent { get; set; }
        /// <summary>
        /// درصد تحقق هدف وصول کل
        /// </summary>
        public decimal TotalReceiptFulfilledPercent { get; set; }
        /// <summary>
        /// درصد تحقق هدف وصول خصوصی
        /// </summary>
        public decimal PrivateReceiptFulfilledPercent { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPercentViewModel()
        {
            IsUsed = true;
        }
        #endregion
    }


}