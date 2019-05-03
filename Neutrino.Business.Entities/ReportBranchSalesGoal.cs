using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Entities
{
    public class ReportBranchSalesGoal
    {
        public string BranchName { get; set; }
        public string GoalGoodsCategoryName { get; set; }
        public decimal TotalSales { get; set; }
        public decimal FinalPromotion { get; set; }
        public decimal PromotionWithOutFulfillmentPercent { get; set; }
        public decimal GoalAmount { get; set; }
        public decimal AmountSpecified { get; set; }
        public decimal FulfilledPercent { get; set; }

    }
}
