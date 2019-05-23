using Espresso.Entites;
using System.Collections.Generic;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات پورسانت فروشنده
    /// </summary>
    public class MemberPromotion : EntityBase
    {
        //شناسه پورسانت مرکز
        public int BranchPromotionId { get; set; }
        public virtual BranchPromotion BranchPromotion { get; set; }
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int GoalId { get; set; }
        /// <summary>
        /// شناسه پرسنل
        /// </summary>
        public int MemberId { get; set; }
        public decimal Promotion { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public virtual Goal Goal { get; set; }
    }
}
