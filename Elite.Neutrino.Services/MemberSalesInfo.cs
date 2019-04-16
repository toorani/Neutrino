using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات فروش هر فروشنده
    /// </summary>
    [DataContract]
    public class MemberSalesInfo
    {
        /// <summary>
        /// شناسه فروشنده
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// شناسه کالا
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// جمع مبلغ فروش هر کالا
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        ///  جمع تعداد فروش هر کالا
        /// </summary>
        public int TotalCount { get; set; }

    }
}