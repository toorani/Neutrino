using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// نگاشت اطلاعات پست سازمانی الیت و پست سازمانی نوترینو
    /// </summary>
    public class PostionMapping : EntityBase
    {
        /// <summary>
        /// عنوان پست سازمانی
        /// اطلاعات الیت NamePost معادل فیلد اطلاعاتی 
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// اطلاعات الیت میباشد ccpost معادل کد 
        /// کد پست سازمانی در اطلاعات الیت
        /// </summary>
        public int PostionRefId { get; set; }
        /// <summary>
        /// شناسه مرکز فروش
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// شناسه مرکز فروش
        /// </summary>
        public int BranchRefId { get; set; }
        /// <summary>
        /// پست سازمانی
        /// </summary>
        public virtual PositionTypeEnum? PositionTypeId { get; set; }

    }

}

