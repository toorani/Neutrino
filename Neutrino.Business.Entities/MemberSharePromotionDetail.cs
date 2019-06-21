using Espresso.Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neutrino.Entities
{
    public class MemberSharePromotionDetail : EntityBase
    {
        public int MemberSharePromotionId { get; set; }
        public virtual MemberSharePromotion MemberSharePromotion { get; set; }
        public SharePromotionTypeEnum SharePromotionTypeId { get; set; }
        public int MemberId { get; set; }
        /// <summary>
        /// سهم از پورسانت فروش مرکز
        /// </summary>
        public decimal BranchSalesPromotion { get; set; }
        /// <summary>
        /// سهم از اهداف عوامل فروش
        /// </summary>
        public decimal SellerPromotion { get; set; }
        /// <summary>
        /// سهم از وصول
        /// </summary>
        public decimal ReceiptPromotion { get; set; }
        

        public MemberSharePromotionDetail()
        {
            
        }

    }
}