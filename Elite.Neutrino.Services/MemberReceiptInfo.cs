using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات وصولی اعضا
    /// </summary>
    [DataContract]
    public class MemberReceiptInfo
    {
        /// <summary>
        /// شناسه عضو
        /// </summary>
        [DataMember]
        public int MemberId { get; set; }
        /// <summary>
        /// مبلغ وصولی
        /// </summary>
        [DataMember]
        public decimal ReceiptAmount { get; set; }
    }
}