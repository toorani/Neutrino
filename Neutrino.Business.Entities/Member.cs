using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// اطلاعات اعضا شرکت
    /// </summary>
    public class Member:EntityBase
    {
        /// <summary>
        /// کد ملی کارمند
        /// </summary>
        [StringLength(11)]
        public string NationalCode { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// نام خانوادگی 
        /// </summary>
        [StringLength(250)]
        public string LastName { get; set; }
        /// <summary>
        /// کد کارمند
        /// </summary>
        public int Code { get; set; }
        public int RefId { get; set; }
        /// <summary>
        /// شناسه مرکز فروش
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// شناسه مرکز فروش
        /// </summary>
        public int BranchRefId { get; set; }
        public virtual Branch Branch { get; set; }
        /// <summary>
        /// پست سازمانی
        /// </summary>
        public virtual PositionTypeEnum PositionTypeId { get; set; }
        [ForeignKey("PositionTypeId")]
        public virtual PositionType PositionType { get; set; }

    }
    
}

