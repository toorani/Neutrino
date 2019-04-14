using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class SupplierGoal : EntityBase
    {
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Required]
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }
        public virtual ICollection<GoalStep> GoalSteps { get; set; }
        public decimal? IncrementPercent { get; set; }

        public SupplierGoal()
        {
            GoalSteps = new HashSet<GoalStep>();
            IsActive = true;
        }

    }
}
