using System;
using System.Runtime.Serialization;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات فروش مراکز
    /// </summary>
    public class BranchSales : EntityBase
    {
        public int BranchId { get; set; }
        public int BranchRefId { get; set; }
        public virtual Branch Branch { get; set; }
        public int GoodsId { get; set; }
        public int GoodsRefId { get; set; }
        public virtual Goods Goods { get; set; }
        /// <summary>
        /// مبلغ فروش
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// تعداد فروش
        /// </summary>
        public double TotalNumber { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}