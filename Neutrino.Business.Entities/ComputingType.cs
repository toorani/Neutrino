using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum ComputingTypeEnum
    {
        [Description("تعداد")]
        Quantities = 1,
        [Description("مبلغ")]
        Amount = 2,
        [Description("درصد")]
        Percentage = 3,
    }

    public class ComputingType : EnumEntity<ComputingTypeEnum>
    {
        public ComputingType(ComputingTypeEnum enumType) : base(enumType)
        {
        }

        public ComputingType() : base() { } // should excplicitly define for EF!
    }
}
