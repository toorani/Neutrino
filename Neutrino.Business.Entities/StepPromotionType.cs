using Espresso.Entites;
using System.ComponentModel;

namespace Neutrino.Entities
{
    public enum StepPromotionTypeEnum :int
    {
        
        [Description("مدیر مرکز")]
        Manager = 1,
        [Description("مدیر عامل")]
        CEO = 2,
        [Description("نهایی")]
        Final = 3,
        

    }

    public class StepPromotionType : EnumEntity<StepPromotionTypeEnum>
    {
        public StepPromotionType(StepPromotionTypeEnum enumType) : base(enumType)
        {
        }

        public StepPromotionType() : base() { } // should excplicitly define for EF!
    }
}