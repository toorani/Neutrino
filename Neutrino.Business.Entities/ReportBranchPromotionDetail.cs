using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Entities
{
    public class ReportBranchPromotionDetail
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string GoalGoodsCategoryName { get; set; }
        public decimal TotalFinalPromotion { get; set; }
        public decimal TotalPromotionWithOutFulfillmentPercent { get; set; }
        public decimal FinalPromotion { get; set; }
        public decimal PromotionWithOutFulfillmentPercent { get; set; }
        public decimal SellerFulfillmentPercent { get; set; }
        public decimal ManagerFulfillmentPercent { get; set; }

    }
}
