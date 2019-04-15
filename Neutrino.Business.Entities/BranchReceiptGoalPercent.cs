using System.Collections.Generic;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// سهم مراکز از اهداف وصول 
    /// </summary>
    public class BranchReceiptGoalPercent : EntityBase
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
        /// درصد زده 
        /// </summary>
        public decimal ReachedPercent { get; set; }
        /// <summary>
        /// درصد نزده 
        /// </summary>
        public decimal NotReachedPercent { get; set; }
        /// <summary>
        /// شناسه هدف
        /// </summary>
        public int GoalId { get; set; }
        /// <summary>
        /// هدف
        /// </summary>
        public virtual Goal Goal { get; set; }

        public BranchReceiptGoalPercent() { }
    }

    public class BranchReceiptGoalPercentDTO
    {
        /// <summary>
        /// درصد زده 
        /// </summary>
        public decimal ReachedPercent { get; set; }
        /// <summary>
        /// درصد نزده 
        /// </summary>
        public decimal NotReachedPercent { get; set; }
        /// <summary>
        /// شناسه هدف
        /// </summary>
        public int GoalId { get; set; }
        public List<int> Branches { get; set; }
        public List<int> DeselectedBranches { get; set; }

        #region [ Constructor(s) ]
        public BranchReceiptGoalPercentDTO()
        {
            Branches = new List<int>();
            DeselectedBranches = new List<int>();
        }
        #endregion
    }
}
