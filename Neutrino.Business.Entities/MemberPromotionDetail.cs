using Espresso.Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neutrino.Entities
{
    public class MemberPromotionDetail : EntityBase
    {
        public int MemberPromotionId { get; set; }
        public virtual MemberPromotion MemberPromotion { get; set; }
        public ReviewPromotionStepEnum ReviewPromotionStepId { get; set; }
        public int MemberId { get; set; }
        /// <summary>
        /// سهم از پورسانت تامین کننده
        /// </summary>
        public decimal SupplierPromotion { get; set; }
        /// <summary>
        /// سهم از پورسانت ترمیمی
        /// </summary>
        public decimal CompensatoryPromotion { get; set; }
        /// <summary>
        /// سهم از وصول
        /// </summary>
        public decimal ReceiptPromotion { get; set; }
        /// <summary>
        /// سهم پورسانت از فروش مرکز
        /// </summary>
        public decimal BranchSalesPromotion { get; set; }


        public MemberPromotionDetail()
        {
            
        }

    }
}