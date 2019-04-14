using System.ComponentModel;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum QuantityConditionTypeEnum 
    {
        [Description("وابسته به هدف")]
        DependedOnGoal = 1,
        [Description("مستقل از هدف")]
        Independent = 2
    }

    public class QuantityConditionType : EnumEntity<QuantityConditionTypeEnum>
    {
        public QuantityConditionType(QuantityConditionTypeEnum enumType) : base(enumType)
        {
        }

        public QuantityConditionType() : base() { } // should excplicitly define for EF!
    }

    
}