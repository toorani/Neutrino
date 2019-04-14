using System.Collections.Generic;
using Espresso.Portal;
using Neutrino.Entities;
using Espresso.Core;

namespace Neutrino.Portal.Models
{
    public class BranchReceiptGoalPercentItemViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        /// <summary>
        /// درصد زده 
        /// </summary>
        public decimal ReachedPercent { get; set; }
        /// <summary>
        /// درصد نزده 
        /// </summary>
        public decimal NotReachedPercent { get; set; }
        public int GoalId { get; set; }
        public List<int> Branches { get; set; }
        public List<int> DeselectedBranches { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public BranchReceiptGoalPercentItemViewModel()
        {
            Branches = new List<int>();
            DeselectedBranches = new List<int>(); 


        }
        #endregion
    }

    public class BranchReceiptGoalPercentViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        public List<BranchReceiptGoalPercentItemViewModel> Items { get; set; }
        public string GoalGoodsCategoryName { get; set; }
        public int GoalGoodsCategoryTypeId { get; set; }
        public string GoalDate { get; set; }
        public bool IsGoalUsed { get; set; }
        public int GoalId { get; set; }
        public decimal GoalAmount { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public BranchReceiptGoalPercentViewModel()
        {
            Items= new List<BranchReceiptGoalPercentItemViewModel>();
        }
        #endregion
    }
}