using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum GoalTypeEnum
    {
        [Description("توزیع کننده")]
        Distributor = 1,
        [Description("تامین کننده")]
        Supplier 
    }

    public class GoalType : EnumEntity<GoalTypeEnum> 
    {
        public GoalType(GoalTypeEnum enumType) : base(enumType)
        {
        }
        public GoalType() : base() { } // should excplicitly define for EF!
    }
}
