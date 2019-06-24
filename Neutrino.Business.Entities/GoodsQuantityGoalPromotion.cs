using Espresso.Entites;
using System;
using System.Collections.Generic;

namespace Neutrino.Entities
{
    public class QuantityGoalPromotion : EntityBase
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
        /// شناسه هدف مرکز
        /// </summary>
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalQuantity { get; set; }
        public decimal FulfilledPercent { get; set; }

        public int GoodsId { get; set; }
        public int GoalQuantity { get; set; }


    }
}
