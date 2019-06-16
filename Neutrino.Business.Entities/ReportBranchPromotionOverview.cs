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
        public string BranchName { get; set; }

        public decimal PrivateFulfilledPercent { get; set; }
        public decimal TotalReceiptFulfilledPercent { get; set; }
        public decimal TotalSalesFulfilledPercent { get; set; }
        public decimal AggregationFulfilledPercent { get; set; }


    }
}
