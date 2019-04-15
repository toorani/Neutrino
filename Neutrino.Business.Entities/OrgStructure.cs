using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// ساختار سازمانی
    /// </summary>
    public class OrgStructure : EntityBase
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// مرکز
        /// </summary>
        public virtual Branch Branch { get; set; }
        /// <summary>
        /// شناسه پست سازمانی
        /// </summary>
        public PositionTypeEnum PositionTypeId { get; set; }
        /// <summary>
        /// پست سازمانی
        /// </summary>
        [ForeignKey("PositionTypeId")]
        public virtual PositionType PositionType { get; set; }
    }
}
