using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات وصول مرکز
    /// </summary>
    [DataContract]
    public class BranchReceiptInfo
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }
        /// <summary>
        ///  مبلغ وصول خصوصی
        /// </summary>
        [DataMember]
        public decimal PrivateAmount { get; set; }
        /// <summary>
        ///  مبلغ وصول دولتی
        /// </summary>
        [DataMember]
        public decimal GovernmentalAmount { get; set; }
        /// <summary>
        ///  مبلغ وصول کل
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }
    }
}