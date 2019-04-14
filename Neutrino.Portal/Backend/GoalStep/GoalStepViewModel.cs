using Espresso.Portal;
using Espresso.Core;

namespace Neutrino.Portal.Models
{
    public class GoalStepViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        public int GoalId { get; set; }
        public int ComputingTypeId { get; set; }
        public decimal ComputingValue { get; set; }
        public decimal RawComputingValue { get; set; }
        public decimal IncrementPercent { get; set; }
        public int GoalTypeId { get; set; }
        public int? RewardTypeId
        {
            get
            {
                if (RewardInfo != null)
                {
                    return RewardInfo.ItemTypeId;
                }
                return null;
            }
            set
            {
                if (RewardInfo != null)
                {
                    RewardInfo.ItemTypeId = value.Value;
                }
            }
        }
        public int? CondemnationTypeId
        {
            get
            {
                if (CondemnationInfo != null)
                {
                    return CondemnationInfo.ItemTypeId;
                }
                return null;
            }
            set
            {
                if (CondemnationInfo != null)
                {
                    CondemnationInfo.ItemTypeId = value.Value;
                }
            }
        }
       

        public GoalStepItemInfoViewModel RewardInfo { get; set; }
        public GoalStepItemInfoViewModel CondemnationInfo { get; set; }

        #endregion

        #region [ Constructor(s) ]
        public GoalStepViewModel()
        {

        }
        #endregion


    }
}