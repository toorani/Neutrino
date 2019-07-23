using Espresso.Portal;
using System.Collections.Generic;

namespace Neutrino.Portal.Models
{
    public class MemberPromotionViewModel : ViewModelBase
    {
        public int BranchPromotionId { get; set; }
        public int MemberId { get; set; }
        public decimal ManagerPromotion { get; set; }
        public decimal? CEOPromotion { get; set; }
        public decimal? FinalPromotion { get; set; }
        public MemberViewModel Member { get; set; }
        public List<MemberPromotionDetailViewModel> Details { get; set; }

    }

    public class MemberPromotionManagerViewModel : ViewModelBase
    {
        public int BranchPromotionId { get; set; }
        public int MemberId { get; set; }
        public decimal ManagerPromotion { get { return SupplierPromotion + CompensatoryPromotion + ReceiptPromotion; } }
        public decimal? CEOPromotion { get { return SupplierPromotion + CompensatoryPromotion + ReceiptPromotion; } }
        public decimal? FinalPromotion { get { return SupplierPromotion + CompensatoryPromotion + ReceiptPromotion; } }
        /// <summary>
        /// سهم از پورسانت فروش مرکز
        /// </summary>
        public decimal SupplierPromotion { get; set; }
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
        public decimal BranchSalesPromotion { get; internal set; }
    }


    public class MemberPromotionDetailViewModel : ViewModelBase
    {
        public int SharePromotionTypeId { get; set; }
        public int MemberId { get; set; }
        /// <summary>
        /// سهم از پورسانت فروش مرکز
        /// </summary>
        public decimal SupplierPromotion { get; set; }
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