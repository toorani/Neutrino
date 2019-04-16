using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// قیمت دارو
    /// </summary>
    [DataContract]
    public class GoodsPrice
    {
        /// <summary>
        /// شناسه محصول
        /// </summary>
        [DataMember]
        public int GoodsId { get; set; }
        /// <summary>
        /// قیمت فروش با احتساب جوایز
        /// </summary>
        [DataMember]
        public decimal SalesPrice { get; set; }
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