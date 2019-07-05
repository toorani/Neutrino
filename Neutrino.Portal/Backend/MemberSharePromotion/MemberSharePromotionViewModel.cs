using Espresso.Portal;
using System.Collections.Generic;

namespace Neutrino.Portal.Models
{
    public class MemberSharePromotionViewModel : ViewModelBase
    {
        public int BranchPromotionId { get; set; }
        public int MemberId { get; set; }
        public decimal ManagerPromotion { get; set; }
        public decimal? CEOPromotion { get; set; }
        public decimal? FinalPromotion { get; set; }
        public MemberViewModel Member { get; set; }
        public List<MemberSharePromotionDetailViewModel> Details { get; set; }

    }

    public class MemberSharePromotionManagerViewModel : ViewModelBase
    {
        public int BranchPromotionId { get; set; }
        public int MemberId { get; set; }
        public decimal ManagerPromotion { get { return BranchSalesPromotion + CompensatoryPromotion + ReceiptPromotion; } }
        public decimal? CEOPromotion { get { return BranchSalesPromotion + CompensatoryPromotion + ReceiptPromotion; } }
        public decimal? FinalPromotion { get { return BranchSalesPromotion + CompensatoryPromotion + ReceiptPromotion; } }
        /// <summary>
        /// سهم از پورسانت فروش مرکز
        /// </summary>
        public decimal BranchSalesPromotion { get; set; }
        /// <summary>
        /// سهم از ترمیمی
        /// </summary>
        public decimal CompensatoryPromotion { get; set; }
        /// <summary>
        /// سهم از وصول
        /// </summary>
        public decimal ReceiptPromotion { get; set; }
        public string FullName { get; set; }
        public int MemberCode { get; set; }
        public string PositionTitle { get; set; }

    }


    public class MemberSharePromotionDetailViewModel : ViewModelBase
    {
        public int SharePromotionTypeId { get; set; }
        public int MemberId { get; set; }
        /// <summary>
        /// سهم از پورسانت فروش مرکز
        /// </summary>
        public decimal BranchSalesPromotion { get; set; }
        /// <summary>
        /// سهم از اهداف عوامل فروش
        /// </summary>
        public decimal CompensatoryPromotion { get; set; }
        /// <summary>
        /// سهم از وصول
        /// </summary>
        public decimal ReceiptPromotion { get; set; }
    }
}