using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات اعضا شرکت
    /// </summary>
    [DataContract]
    public class MemberInfo
    {
        /// <summary>
        /// کد ملی کارمند
        /// </summary>
        [DataMember]
        public string NationalCode { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// نام خانوادگی 
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// کد کارمند
        /// </summary>
        [DataMember]
        public int Code { get; set; }
        /// <summary>
        /// شناسه مرکز فروش
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }
        /// <summary>
        /// شناسه پست سازمانی
        /// </summary>
        [DataMember]
        public int PositionId { get; set; }
        [DataMember]
        public int? ccgoroh { get; set; }
        /// <summary>
        /// شناسه واحد کاری 
        /// </summary>
        [DataMember]
        public int DepartmentId { get; set; }
    }
    
}

