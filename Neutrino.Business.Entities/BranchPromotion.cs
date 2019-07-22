using Espresso.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public decimal? TotalSalesPromotion { get; set; }
        public decimal? PrivateReceiptPromotion { get; set; }
        public decimal? TotalReceiptPromotion { get; set; }
        /// <summary>
        /// پورسانت ترمیمی
        /// </summary>
        public decimal CompensatoryPromotion { get; set; }

        public PromotionReviewStatusEnum PromotionReviewStatusId { get; set; }
        [ForeignKey("PromotionReviewStatusId")]
        public virtual PromotionReviewStatus PromotionReviewStatus { get; set; }
        public virtual ICollection<BranchGoalPromotion> BranchGoalPromotions { get; private set; }
        public virtual ICollection<SellerPromotion> SellerPromotions { get; private set; }
        public virtual ICollection<MemberPromotion> MemberPromotions { get; private set; }
        public virtual ICollection<MemberPenalty> MemberPenalties { get; private set; }
        public virtual ICollection<QuantityGoalPromotion> QuantityGoalPromotions { get; private set; }
        public virtual ICollection<OperationPromotion> OperationPromotions { get; private set; }

        public decimal Budget { get; set; }
        
        #region [ Constructor(s) ]
        public BranchPromotion()
        {
            BranchGoalPromotions = new HashSet<BranchGoalPromotion>();
            SellerPromotions = new HashSet<SellerPromotion>();
            MemberPromotions = new HashSet<MemberPromotion>();
            QuantityGoalPromotions = new HashSet<QuantityGoalPromotion>();
            MemberPenalties = new HashSet<MemberPenalty>();
            OperationPromotions = new HashSet<OperationPromotion>();
        }
        #endregion
    }
}
