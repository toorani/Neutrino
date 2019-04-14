using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات وصولی اعضا
    /// </summary>
    public class MemberReceipt : EntityBase
    {
        /// <summary>
        /// شناسه عضو
        /// </summary>
        public int MemberId { get; set; }
        public int MemberRefId { get; set; }
        public virtual Member Member { get; set; }
        /// <summary>
        /// مبلغ وصولی
        /// </summary>
        public decimal ReceiptAmount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}