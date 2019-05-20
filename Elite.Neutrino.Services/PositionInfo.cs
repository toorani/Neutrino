using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات پست های سازمانی
    /// </summary>
    [DataContract]
    public class PositionInfo
    {
        /// <summary>
        /// شناسه پست سازمانی
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }
        /// <summary>
        /// عنوان پست سازمانی
        /// </summary>
        [DataMember]
        public string Title { get; set; }

    }
}