using Espresso.Entites;
using System.Collections.Generic;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات فروش به همراه پورسانت فروشنده
    /// </summary>
    public class MemberSales_PromotionInfo
    {
        public int MemberId { get; set; }
        public decimal Promotion { get; set; }
        /// <summary>
        /// تعداد کل فروش
        /// </summary>
        public int TotalQauntity { get; set; }
        /// <summary>
        /// مبلغ کل فروش
        /// </summary>
        public decimal TotalAmount { get; set; }
        
    }
}
