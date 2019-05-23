using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Entities
{
    public class ReportSellerGoal
    {
        public string SellerName { get; set; }
        public string GoalGoodsCategoryName { get; set; }
        public decimal TotalSales { get; set; }
        public decimal FinalPromotion { get; set; }
        public ComputingTypeEnum ComputingTypeId { get; set; }
        public ApprovePromotionTypeEnum ApprovePromotionTypeId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal ComputingValue { get; set; }
        public string BranchName { get; set; }
        public decimal FulfilledPercent { get; set; }
    }
}
