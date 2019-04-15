using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class QuantityCondition : EntityBase
    {
        #region [ Public Property(ies) ]
        /// <summary>
        /// شناسه هدف
        /// </summary>
        public int GoalId { get; set; }
        public virtual Goal Goal { get; set; }
        public virtual QuantityConditionTypeEnum? QuantityConditionTypeId { get; set; }
        [ForeignKey("QuantityConditionTypeId")]
        public virtual QuantityConditionType QuantityConditionType { get; set; }
        /// <summary>
        ///  تعداد آیتم
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        ///  درصد پاداش اضافی
        /// </summary>
        public decimal ExtraEncouragePercent { get; set; }
        /// <summary>
        ///  درصد دست نیافته
        /// </summary>
        public decimal NotReachedPercent { get; set; }
        /// <summary>
        ///  درصد حالت چهارم
        /// </summary>
        public decimal ForthCasePercent { get; set; }
        public virtual ICollection<GoodsQuantityCondition> GoodsQuantityConditions { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public QuantityCondition()
        {
            GoodsQuantityConditions = new HashSet<GoodsQuantityCondition>();
        }
        #endregion
    }
    
}