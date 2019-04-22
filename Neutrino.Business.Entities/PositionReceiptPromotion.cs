using Espresso.Entites;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neutrino.Entities
{
    /// <summary>
    /// پورسانت پست های مختلف از اهداف وصول
    /// </summary>
    public class PositionReceiptPromotion : EntityBase
    {
        //شناسه پورسانت مرکز
        public int BranchGoalPromotionId { get; set; }
        public virtual BranchGoalPromotion BranchGoalPromotion { get; set; }
        /// <summary>
        /// شناسه سهم پست سازمانی از هدف وصول
        /// </summary>
        public int OrgStructureShareId { get; set; }
        /// <summary>
        /// سهم پست سازمانی از هدف وصول
        /// </summary>
        public virtual OrgStructureShare OrgStructureShare { get; set; }

        public virtual PositionTypeEnum PositionTypeId { get; set; }
        [ForeignKey("PositionTypeId")]
        public virtual PositionType PositionType { get; set; }

        public decimal Promotion { get; set; }

    }
}
