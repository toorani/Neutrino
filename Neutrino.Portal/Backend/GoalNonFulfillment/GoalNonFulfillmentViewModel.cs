using System.Collections.Generic;
using Espresso.Portal;
using Neutrino.Entities;
using Espresso.Core;

namespace Neutrino.Portal.Models
{
    public class GoalNonFulfillmentViewModel : ViewModelBase
    {
        /// <summary>
        /// شناسه هدف
        /// </summary>
        public int GoalId { get; set; }
        /// <summary>
        /// شناسه مراکز
        /// </summary>
        public List<GoalNonFulfillmentBranchViewModel> Branches { get; set; }
        /// <summary>
        /// درصد 
        /// </summary>
        public decimal Percent { get; set; }

        #region [ Constructor(s) ]

        #endregion
    }

    public class GoalNonFulfillmentBranchViewModel : ViewModelBase
    {
        public int GoalNonFulfillmentBranchId { get; set; }
        public string Name { get; set; }
        
    }

    public class GoalNonFulfillmentCollectionViewModel : ViewModelBase
    {
        public List<GoalNonFulfillmentViewModel> Items { get; set; }
        public string GoalGoodsCategoryName { get; set; }
        public int GoalGoodsCategoryId { get; set; }
        public string StartGoalDate { get; set; }
        public string EndGoalDate { get; set; }
        public bool IsGoalUsed { get; set; }
        public int GoalId { get; set; }
        
        public GoalNonFulfillmentCollectionViewModel()
        {
            Items = new List<GoalNonFulfillmentViewModel>();
        }
    }


}