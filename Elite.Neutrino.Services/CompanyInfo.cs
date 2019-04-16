using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات شرکت 
    /// </summary>
    [DataContract]
    public class CompanyInfo
    {
        [DataMember]
        public string FaName { get; set; }
        [DataMember]
        public string EnName { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Code { get; set; }
    }
    
}
