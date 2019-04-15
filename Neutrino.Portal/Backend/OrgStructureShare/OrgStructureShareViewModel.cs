using System.Collections.Generic;
using Espresso.Portal;
using Neutrino.Entities;
using Espresso.Core;

namespace Neutrino.Portal.Models
{
    public class OrgStructureShareDTOViewModel : ViewModelBase
    {
        public BranchViewModel Branch { get; set; }
        public List<OrgStructureShareViewModel> Items { get; set; }
        public OrgStructureShareDTOViewModel()  
        {
        }
    }
    public class OrgStructureShareViewModel : ViewModelBase
    {
        public OrgStructureIndexViewModel OrgStructure { get; set; }
        public int OrgStructureId { get; set; }
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

        public OrgStructureShareViewModel()
        {
        }
    }
}