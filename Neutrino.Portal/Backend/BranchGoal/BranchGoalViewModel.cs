using System.Collections.Generic;
using Espresso.Portal;
using Neutrino.Entities;
using Espresso.Core;

namespace Neutrino.Portal.Models
{
    public class BranchGoalItemViewModel : ViewModelBase
    {
        public BranchGoalItemViewModel()  
        {
        }

        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal? Percent { get; set; }
        public decimal? Amount { get; set; }
        public int? BranchGoalId { get; set; }
        public int GoalId { get; set; }
       

        #region [ Constructor(s) ]
        
        #endregion

    }
    public class BranchGoalViewModel : ViewModelBase
    {
        public BranchGoalViewModel()  
        {
            Items = new List<BranchGoalItemViewModel>();
            //GoalSteps = new List<GoalStepViewModel>();
        }

        public List<BranchGoalItemViewModel> Items { get; set; }
        public GoalViewModel Goal { get; set; }
    }
    public class BranchBenefitGoalViewModel : ViewModelBase
    {
        public BranchBenefitGoalViewModel()  
        {
            
        }
        public string GoalGoodsCategoryName { get; set; }
        public int GoalGoodsCategoryId { get; set; }
        public decimal TotalPercent { get; set; }
        public decimal TotalAmount { get; set; }
    }
}