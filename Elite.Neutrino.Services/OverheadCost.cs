using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات هزینه سربار هر مرکز
    /// </summary>
    [DataContract]
    public class OverheadCost
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }
        /// <summary>
        ///  مبلغ سربار
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }
        /// <summary>
        ///  ماه
        /// </summary>
        [DataMember]
        public int Month { get; set; }
        /// <summary>
        ///  سال
        /// </summary>
        [DataMember]
        public int Year { get; set; }
    }
}