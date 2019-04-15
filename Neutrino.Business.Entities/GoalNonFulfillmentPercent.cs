using System.Collections.Generic;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// مقدار پاداش در صورت عدم تحقق هدف برای هر مرکز 
    /// </summary>
    public class GoalNonFulfillmentPercent : EntityBase
    {
        /// <summary>
        /// شناسه هدف
        /// </summary>
        public int GoalId { get; set; }
        public virtual Goal Goal { get; set; }
        /// <summary>
        /// درصد 
        /// </summary>
        public decimal Percent { get; set; }
        public virtual ICollection<GoalNonFulfillmentBranch> GoalNonFulfillmentBranches { get; private set; }
        public GoalNonFulfillmentPercent()
        {
            GoalNonFulfillmentBranches = new HashSet<GoalNonFulfillmentBranch>();
        }
    }
}