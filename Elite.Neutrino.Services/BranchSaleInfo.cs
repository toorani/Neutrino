using System;
using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات فروش مراکز
    /// </summary>
    [DataContract]
    public class BranchSalesInfo
    {
        /// <summary>
        /// شناسه مرکز 
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }
        /// <summary>
        /// شناسه کالا
        /// </summary>
        [DataMember]
        public int GoodsId { get; set; }
        /// <summary>
        /// مبلغ فروش
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }
    }
}