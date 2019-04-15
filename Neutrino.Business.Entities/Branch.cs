using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class Branch : EntityBase
    {
        public int RefId { get; set; }
        [Required,StringLength(50)]
        public string Name { get; set; }
        [StringLength(300)]
        public string Address { get; set; }
        public int Zone { get; set; }
        [StringLength(50)]
        public string CityName { get; set; }
        [StringLength(50)]
        public string Level { get; set; }
        public string Order { get; set; }
        public virtual ICollection<BranchGoal> BranchGoals { get; set; }
        public virtual ICollection<FulfillmentPercent> GoalFulfillments { get; set; }
        public virtual ICollection<BranchReceiptGoalPercent> BranchReceiptGoalPercent { get; set; }
        public Branch()
        {
            BranchGoals = new HashSet<BranchGoal>();
            GoalFulfillments = new HashSet<FulfillmentPercent>();
            BranchReceiptGoalPercent = new HashSet<BranchReceiptGoalPercent>();
        }
    }
}