using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات فاکتور فروشنده
    /// </summary>
    [DataContract]
    public class InvoiceInfo
    {
        /// <summary>
        /// شناسه فروشنده
        /// </summary>
        [DataMember]
        public int SellerId { get; set; }
        /// <summary>
        /// شناسه محصول
        /// </summary>
        [DataMember]
        public int GoodsId { get; set; }
        /// <summary>
        ///  جمع تعداد فاکتور
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }
        /// <summary>
        /// جمع مبلغ فاکتور 
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }
    }

}
