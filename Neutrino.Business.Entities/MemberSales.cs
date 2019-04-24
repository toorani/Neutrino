using System;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات فروش اعضا
    /// </summary>
    public class MemberSales : EntityBase
    {
        /// <summary>
        /// شناسه عضو
        /// </summary>
        public int MemberId { get; set; }
        public int MemberRefId { get; set; }
        public virtual Member Member { get; set; }
        /// <summary>
        /// مبلغ فروش
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// تعداد فروش
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// شناسه محصول در الیت
        /// </summary>
        public int GoodsRefId { get; set; }
        /// <summary>
        /// شناسه محصول
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Goods Goods { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}