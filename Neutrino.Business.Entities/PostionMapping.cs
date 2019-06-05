using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// نگاشت اطلاعات پست سازمانی الیت و پست سازمانی نوترینو
    /// </summary>
    public class PositionMapping : EntityBase
    {
        /// <summary>
        /// کد پست سازمانی در اطلاعات الیت
        /// </summary>
        public int PositionRefId { get; set; }
        public int ElitePositionId { get; set; }
        /// <summary>
        /// پست سازمانی
        /// </summary>
        public virtual PositionTypeEnum? PositionTypeId { get; set; }
        public virtual ElitePosition ElitePosition { get; set; }
    }

}

