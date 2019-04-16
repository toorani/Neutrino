using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// حقوق و دستمزد اعضا
    /// </summary>
    [DataContract]
    public class Payroll
    {
        /// <summary>
        /// شناسه اعضا 
        /// </summary>
        [DataMember]
        public int MemberId { get; set; }
        /// <summary>
        /// خالص پرداختی
        /// </summary>
        [DataMember]
        public decimal Payment { get; set; }
    }
}