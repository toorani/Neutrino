using Espresso.Entites;

namespace Neutrino.Entities
{
    public class BranchQuantityCondition : EntityBase
    {
        #region [ Public Property(ies) ]
        public int Quantity { get; set; }
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public int? GoodsQuantityConditionId { get; set; }
        public virtual GoodsQuantityCondition GoodsQuantityCondition { get; set; }

        #endregion

        #region [ Constructor(s) ]
        public BranchQuantityCondition()
        {
            
        }
        #endregion
    }
    
}