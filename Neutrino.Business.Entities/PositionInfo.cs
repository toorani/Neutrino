using Espresso.Entites;
using System.Runtime.Serialization;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات پست های سازمانی
    /// </summary>
    public class ElitePosition : EntityBase
    {
        /// <summary>
        /// شناسه پست سازمانی
        /// </summary>
        public int RefId { get; set; }
        /// <summary>
        /// عنوان پست سازمانی
        /// </summary>
        public string Title { get; set; }
    }
}