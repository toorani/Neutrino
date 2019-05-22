using Espresso.Entites;
using System;
using System.Collections.Generic;

namespace Neutrino.Entities
{
    public class BranchGoalPromotion : EntityBase
    {
        //شناسه پورسانت مرکز
        public int BranchPromotionId { get; set; }
        public virtual BranchPromotion BranchPromotion { get; set; }
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int GoalId { get; set; }
        public virtual Goal Goal { get; set; }
        /// <summary>
        /// مقدار پورسانت بدون در نظر گرفتن درصد پورسانت مرکز
        /// </summary>
        public decimal PromotionWithOutFulfillmentPercent { get; set; }
        /// <summary>
        /// مقدار پورسانت با در نظر گرفتن درصد پورسانت مرکز
        /// </summary>
        public decimal FinalPromotion { get; set; }
        /// <summary>
        /// شناسه هدف مرکز
        /// </summary>
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        
        public virtual ICollection<PositionReceiptPromotion> PositionReceiptPromotions { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalQuantity { get; set; }
        public decimal FulfilledPercent { get; set; }

        public BranchGoalPromotion() => PositionReceiptPromotions = new HashSet<PositionReceiptPromotion>();

    }
}
