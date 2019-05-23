using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class ReportBranchSalesViewModel : ViewModelBase
    {
        public int ApprovePromotionTypeId { get; set; }
        public int ComputingTypeId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalQuantity { get; set; }
        public decimal FinalPromotion { get; set; }
        public decimal PromotionWithOutFulfillmentPercent { get; set; }
    }

    public class ReportSales_Amount_Qualntity_ViewModel : ReportBranchSalesViewModel
    {
        public List<PromotionGoalStep> PromotionGoalSteps { get; set; }
        public ReportSales_Amount_Qualntity_ViewModel()
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

    public class ReportSales_Percent_ViewModel : ReportBranchSalesViewModel
    {
        public decimal FulfilledPercent { get; set; }
        public ReportSales_Percent_ViewModel()
        {
            
        }
    }
   

}