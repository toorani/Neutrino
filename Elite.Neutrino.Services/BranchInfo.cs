using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات مراکز توزیع
    /// </summary>
    [DataContract]
    public class BranchInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Name { get; set; }
        /// <summary>
        /// شماره منطقه
        /// </summary>
        [DataMember]
        public int Zone { get; set; }
        [DataMember]
        public string CityName { get; set; }
        /// <summary>
        /// درجه مرکز
        /// </summary>
        [DataMember]
        public string Level { get; set; }
    }
}