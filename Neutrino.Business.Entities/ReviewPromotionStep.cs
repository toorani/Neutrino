using Espresso.Entites;
using System.ComponentModel;

namespace Neutrino.Entities
{
    public enum ReviewPromotionStepEnum : int
    {
        [Description("محاسبه سیستم")]
        Initial = 1,
        [Description("مدیر مرکز")]
        Manager = 2,
        [Description("مدیر عامل")]
        CEO = 3,
        [Description("نهایی")]
        Final = 4,
    }

    public class ReviewPromotionStep : EnumEntity<ReviewPromotionStepEnum>
    {
        public ReviewPromotionStep(ReviewPromotionStepEnum enumType) : base(enumType)
        {
        }

        public ReviewPromotionStep() : base() { } // should excplicitly define for EF!
    }
}