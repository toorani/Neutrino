using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class BranchBenefitGoodsCategory : EntityBase
    {
        public int BranchBenefitId { get; set; }
        public int GoodsCategoryId { get; set; }
        public virtual BranchBenefit BranchBenefit { get; set; }
        public virtual GoodsCategory GoodsCategory { get; set; }
    }
}
