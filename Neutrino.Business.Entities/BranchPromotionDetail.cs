using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Entities
{
    public class BranchPromotionDetail
    {
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public decimal FinalPromotion { get; set; }
        public decimal? PositionPromotion { get; set; }
        public string PositionTitle { get; set; }
        public PositionTypeEnum? PositionTypeId { get; set; }
        public GoalGoodsCategoryTypeEnum GoalGoodsCategoryTypeId { get; set; }


    }
}
