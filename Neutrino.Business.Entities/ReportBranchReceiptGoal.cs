using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Entities
{
    public class ReportBranchReceiptGoal
    {
        public string BranchName { get; set; }
        public string GoalGoodsCategoryName { get; set; }
        public decimal TotalSales { get; set; }
        public decimal FinalPromotion { get; set; }
        public decimal AmountSpecified { get; set; }
        public decimal FulfilledPercent { get; set; }
        public int TotalQuantity { get; set; }
        public decimal PositionPromotion { get; set; }
        public string PositionTitle { get; set; }
        public decimal ReceiptAmount { get; set; }
        
    }
}
