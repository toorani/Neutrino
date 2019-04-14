using System;
using System.Runtime.Serialization;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات فاکتور فروشنده
    /// </summary>
    public class Invoice : EntityBase
    {
        /// <summary>
        /// شناسه فروشنده
        /// </summary>
        public int SellerId { get; set; }
        public int SellerRefId { get; set; }
        public int GoodsId { get; set; }
        public int GoodsRefId { get; set; }
        public virtual Goods Goods { get; set; }
        /// <summary>
        ///  جمع تعداد فاکتور
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// جمع مبلغ فاکتور 
        /// </summary>
        public decimal TotalAmount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}
