using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class ReportBranchPromotionDetailViewModel : ViewModelBase
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalFinalPromotion { get; set; }
        public decimal TotalPromotionWithOutFulfillmentPercent { get; set; }
        public decimal SellerFulfillmentPercent { get; set; }
        public decimal ManagerFulfillmentPercent { get; set; }
        public List<GoalPromotion> GoalPromotions { get; set; }
        public ReportBranchPromotionDetailViewModel()
        {
            GoalPromotions = new List<GoalPromotion>();
        }

    }

    public class GoalPromotion
    {

        public string GoalGoodsCategoryName { get; set; }
        public decimal FinalPromotion { get; set; }
        public decimal PromotionWithOutFulfillmentPercent { get; set; }
    }

}