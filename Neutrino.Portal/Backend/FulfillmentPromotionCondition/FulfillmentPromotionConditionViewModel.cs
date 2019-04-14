using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class FulfillmentPromotionConditionViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        /// <summary>
        /// درصد تحقق هدف کل
        /// </summary>
        public decimal? TotalSalesFulfilledPercent { get; set; }
        /// <summary>
        ///  درصد تحقق هدف وصول کل
        /// </summary>
        public decimal? TotalReceiptFulfilledPercent { get; set; }
        /// <summary>
        /// درصد تحقق هدف وصول خصوصی
        /// </summary>
        public decimal? PrivateReceiptFulfilledPercent { get; set; }
        /// <summary>
        /// درصد تحقق هدف تجمیع
        /// </summary>
        public decimal? AggregateFulfilledPercent { get; set; }
        /// <summary>
        /// درصد پورسانت رییس و سرپرست
        /// </summary>
        public decimal? ManagerPromotion { get; set; }
        /// <summary>
        ///  درصد پورسانت ویزیتور
        /// </summary>
        public decimal? SellerPromotion { get; set; }

        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPromotionConditionViewModel()
        {
        }
        #endregion

    }
   
}