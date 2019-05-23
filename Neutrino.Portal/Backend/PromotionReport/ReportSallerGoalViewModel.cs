using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class ReportSellerGoalViewModel : ViewModelBase
    {
        public int ApprovePromotionTypeId { get; set; }
        public int ComputingTypeId { get; set; }
        public string SellerName { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalQuantity { get; set; }
        public decimal FinalPromotion { get; set; }
        public string BranchName { get; set; }
        public List<SellerPromotionGoalStep> PromotionGoalSteps { get; set; }
        public ReportSellerGoalViewModel() {
            PromotionGoalSteps = new List<SellerPromotionGoalStep>();
        }  
        
    }

    public class SellerPromotionGoalStep
    {
        public decimal ComputingValue { get; set; }
        public decimal FulfilledPercent { get; set; }

    }

}