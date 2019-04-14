using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
   
    public enum CondemnationTypeEnum
    {
        [Description("جریمه")]
        Penalty = 1
        
    }

    public class CondemnationType : EnumEntity<CondemnationTypeEnum>
    {
        public CondemnationType(CondemnationTypeEnum enumType) : base(enumType)
        {
        }

        public CondemnationType() : base() { } // should excplicitly define for EF!
    }
}
