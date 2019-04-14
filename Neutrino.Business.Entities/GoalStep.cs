using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoalStep : EntityBase
    {
        [Required]
        public int GoalId { get; set; }
        public virtual Goal Goal { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public virtual ComputingTypeEnum ComputingTypeId { get; set; }
        public decimal ComputingValue { get; set; }
        [ForeignKey("ComputingTypeId")]
        public virtual ComputingType ComputingType { get; set; }
        public decimal? IncrementPercent { get; set; }
        public decimal RawComputingValue { get; set; }
        public GoalTypeEnum GoalTypeId { get; set; }
        [ForeignKey("GoalTypeId")]
        public GoalType GoalType { get; set; }
        public virtual ICollection<GoalStepItemInfo> Items { get; set; }

        #region [ Constructor(s) ]
        public GoalStep()
        {
            Items = new HashSet<GoalStepItemInfo>();
            IsActive = true;
        }
        #endregion

    }
}
