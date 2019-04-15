using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum GoalStepActionTypeEnum
    {
        [Description("پاداش")]
        Reward = 1,
        [Description("جریمه")]
        Condemnation 

    }

    public class GoalStepActionType : EnumEntity<GoalStepActionTypeEnum> 
    {
        public GoalStepActionType(GoalStepActionTypeEnum enumType) : base(enumType)
        {
        }
        public GoalStepActionType() : base() { } // should excplicitly define for EF!
    }
}
