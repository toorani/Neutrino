using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class BranchPromotionDetailViewModel : ViewModelBase
    {
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public string GoalTypeTitle { get; set; }
        public decimal TotalFinalPromotion { get; set; }
        public List<PositionPromotion> PositionPromotions { get; set; }

        public BranchPromotionDetailViewModel() { PositionPromotions = new List<PositionPromotion>(); }

    }


}