using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoodsQuantityCondition : EntityBase
    {
        #region [ Public Property(ies) ]
        public int QuantityConditionId { get; set; }
        public virtual QuantityCondition QuantityCondition { get; set; }
        public int Quantity { get; set; }
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }
        public virtual ICollection<BranchQuantityCondition> BranchQuantityConditions { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public GoodsQuantityCondition()
        {
            BranchQuantityConditions = new HashSet<BranchQuantityCondition>();
        }
        #endregion
    }
    
}