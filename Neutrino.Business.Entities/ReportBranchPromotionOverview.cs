using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Entities
{
    public class ReportBranchPromotionOverview
    {
        public int BranchId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string BranchTitle { get; set; }

        public decimal PrivateAmount { get; set; }
        public decimal PrivateBranchGoalAmount { get; set; }
        public decimal PrivateFulfilledPercent { get; set; }

        public decimal TotalRecieptAmount { get; set; }
        public decimal TotalReceiptBranchGoalAmount { get; set; }
        public decimal TotalReceiptFulfilledPercent { get; set; }

        public int TotalQuantity { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalSalesFulfilledPercent { get; set; }

        public decimal TotalSalesBranchGoalAmount { get; set; }
        public int AggregationQuantity { get; set; }
        public decimal AggregationAmount { get; set; }
        public decimal AggregationFulfilledPercent { get; set; }
        public decimal AggregationBranchGoalAmount { get; set; }



    }
}
