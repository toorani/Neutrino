using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum TherapeuticTypeEnum
    {
        [Description("قلبی")]
        Heart = 1,
        [Description("اعصاب")]
        Nervous
    }

    public class TherapeuticType : EnumEntity<TherapeuticTypeEnum> 
    {
        public TherapeuticType(TherapeuticTypeEnum enumType) : base(enumType)
        {
        }
        public TherapeuticType() : base() { } // should excplicitly define for EF!
    }
}
