using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// مراکز عدم تحقق هدف برای 
    /// </summary>
    public class GoalNonFulfillmentBranch : EntityBase
    {
        /// <summary>
        /// شناسه هدف
        /// </summary>
        public int GoalNonFulfillmentPercentId { get; set; }
        public virtual GoalNonFulfillmentPercent GoalNonFulfillmentPercent { get; set; }
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// مرکز
        /// </summary>
        public virtual Branch Branch { get; set; }
        
        public GoalNonFulfillmentBranch()
        {

        }
    }
}