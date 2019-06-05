using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات واحد کاری
    /// </summary>
    public class Department : EntityBase
    {
        /// <summary>
        /// شناسه واحد کاری
        /// </summary>
        public int RefId { get; set; }
        /// <summary>
        /// عنوان 
        /// </summary>
        public string Title { get; set; }
    }
}