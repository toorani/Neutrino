using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات واحد کاری
    /// </summary>
    [DataContract]
    public class DepartmentInfo
    {
        /// <summary>
        /// شناسه واحد کاری
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// عنوان 
        /// </summary>
        [DataMember]
        public string Title { get; set; }
    }
}