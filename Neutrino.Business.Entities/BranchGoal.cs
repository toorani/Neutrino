using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// سهم هر مرکز از هدف
    /// </summary>
    public class BranchGoal : EntityBase
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// مرکز
        /// </summary>
        public virtual Branch Branch { get; set; }
        /// <summary>
        /// درصد سهم مرکز
        /// </summary>
        public decimal? Percent { get; set; }
        /// <summary>
        /// مبلغ سهم مرکز
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// شناسه هدف
        /// </summary>
        public int GoalId { get; set; }
        /// <summary>
        /// هدف
        /// </summary>
        public virtual Goal Goal { get; set; }


        public BranchGoal() { }
    }
}
