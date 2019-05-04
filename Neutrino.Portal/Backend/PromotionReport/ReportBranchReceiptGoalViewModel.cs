using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class ReportBranchReceiptGoalViewModel : ViewModelBase
    {
        public string BranchName { get; set; }
        public string GoalGoodsCategoryName { get; set; }
        public decimal TotalSales { get; set; }
        public decimal FinalPromotion { get; set; }
        public decimal AmountSpecified { get; set; }
        public decimal FulfilledPercent { get; set; }
        public int TotalQuantity { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal PositionTotalAmount { get; set; }
        public List<PositionPromotion> PositionPromotions { get; set; }
        public ReportBranchReceiptGoalViewModel()
        {
            PositionPromotions = new List<PositionPromotion>();
        }

    }

    public class PositionPromotion
    {
        public decimal Promotion { get; set; }
        public string PositionTitle { get; set; }
    }

}