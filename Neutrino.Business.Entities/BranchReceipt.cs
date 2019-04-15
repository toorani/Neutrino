

using System;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات وصول
    /// </summary>
    public class BranchReceipt : EntityBase
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// شناسه مرکز در بانک اطلاعاتی الیت
        /// </summary>
        public int BranchRefId { get; set; }
        public virtual Branch Branch { get; set; }
        /// <summary>
        ///  مبلغ وصول خصوصی
        /// </summary>
        public decimal? PrivateAmount { get; set; }
        /// <summary>
        ///  مبلغ وصول دولتی
        /// </summary>
        public decimal? GovernmentalAmount { get; set; }
        /// <summary>
        ///  مبلغ وصول کل
        /// </summary>
        public decimal TotalAmount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}