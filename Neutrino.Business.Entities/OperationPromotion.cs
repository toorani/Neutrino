using Espresso.Entites;
using System.Collections.Generic;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات پورسانت عملیات
    /// </summary>
    public class OperationPromotion : EntityBase
    {
        //شناسه پورسانت مرکز
        public int BranchPromotionId { get; set; }
        public virtual BranchPromotion BranchPromotion { get; set; }
        /// <summary>
        /// شناسه پرسنل
        /// </summary>
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }
        public decimal Promotion { get; set; }
        
    }
}
