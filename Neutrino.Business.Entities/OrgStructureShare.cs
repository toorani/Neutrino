using System.Collections.Generic;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// سهم پست سازمانی از پورسانت
    /// </summary>
    public class OrgStructureShare  : EntityBase
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// مرکز
        /// </summary>
        public Branch Branch { get; set; }
        /// <summary>
        /// شناسه ساختار سازمانی
        /// </summary>
        public int OrgStructureId { get; set; }
        /// <summary>
        /// ساختار سازمانی
        /// </summary>
        public OrgStructure OrgStructure { get; set; }
        /// <summary>
        /// سهم از پورسانت فروش 
        /// </summary>
        public decimal? SalesPercent { get; set; }
        /// <summary>
        /// سهم از پورسانت وصول خصوصی
        /// </summary>
        /// 
        public decimal? PrivateReceiptPercent { get; set; }
        /// <summary>
        /// سهم از پورسانت وصول کل
        /// </summary>
        public decimal? TotalReceiptPercent { get; set; }
        /// <summary>
        /// حداقل پورسانت
        /// </summary>
        public decimal? MinimumPromotion { get; set; }
        /// <summary>
        /// حداکثر پورسانت
        /// </summary>
        public decimal? MaximumPromotion { get; set; }

    }

    public class OrgStructureShareDTO : EntityBase
    {
        public Branch Branch { get; set; }
        public List<OrgStructureShare> Items { get; set; }

        #region [ Constructor(s) ]
        public OrgStructureShareDTO()
        {
            Items = new List<OrgStructureShare>();
        }
        #endregion
    }

}
