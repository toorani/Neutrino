using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class FulfillmentPercentViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        public GoalViewModel Goal { get; set; }
        public decimal TouchedGoalPercent { get; set; }
        public decimal EncouragePercent { get; set; }
        public string YearMonth { get; set; }
        public bool IsUsed { get; set; }
        public int BranchId { get; set; }
        public BranchViewModel Branch { get; set; }
        public int GoalId { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPercentViewModel()
        {
            IsUsed = true;
        }
        #endregion
    }
   

}