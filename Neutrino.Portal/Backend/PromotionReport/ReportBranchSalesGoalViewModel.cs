using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class ReportBranchSalesGoalViewModel : ViewModelBase
    {
        public string BranchName { get; set; }
        public decimal TotalSales { get; set; }
        public decimal FinalPromotion { get; set; }
        public decimal PromotionWithOutFulfillmentPercent { get; set; }
        public List<PromotionGoalStep> PromotionGoalSteps { get; set; }
        public ReportBranchSalesGoalViewModel()
        {
            PromotionGoalSteps = new List<PromotionGoalStep>();
        }

    }

    public class PromotionGoalStep
    {
        public decimal GoalAmount { get; set; }
        public decimal AmountSpecified { get; set; }
        public decimal FulfilledPercent { get; set; }

    }

}