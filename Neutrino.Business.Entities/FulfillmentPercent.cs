using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    ///  درصد مشمول پورسانت درصورت تحقق
    /// </summary>
    public class FulfillmentPercent : EntityBase
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
        public virtual Branch Branch { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPercent()
        {
            IsUsed = false;
        }
        #endregion
    }
    
}