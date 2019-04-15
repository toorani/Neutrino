using System;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// حقوق و دستمزد اعضا
    /// </summary>
    public class MemberPayroll : EntityBase
    {
        /// <summary>
        /// شناسه اعضا 
        /// </summary>
        public int MemberId { get; set; }
        public int MemberRefId { get; set; }
        public virtual Member Member { get; set; }
        /// <summary>
        /// خالص پرداختی
        /// </summary>
        public decimal Payment { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}