using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoalStepItemInfo :EntityBase
    {
        [Required]
        public virtual GoalStepActionTypeEnum ActionTypeId { get; set; }
        [ForeignKey("ActionTypeId")]
        public virtual GoalStepActionType ActionType { get; set; }
        [Required]
        public int ItemTypeId { get; set; }
        public virtual ComputingTypeEnum? ComputingTypeId { get; set; }
        [ForeignKey("ComputingTypeId")]
        public virtual ComputingType ComputingType { get; set; }
        public int? EachValue { get; set; }
        public decimal? Amount { get; set; }
        /// <summary>
        /// showing the id of item in a selectable object for goalStepItem ,
        /// for example an otherReward is in this category.
        /// </summary>
        public int? ChoiceValueId { get; set; }
        public int GoalStepId { get; set; }
        public virtual GoalStep GoalStep { get; set; }

        public GoalStepItemInfo()
        {
            
        }

    }
}
