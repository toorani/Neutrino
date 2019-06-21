using Espresso.Entites;
using System.ComponentModel;

namespace Neutrino.Entities
{
    public enum SharePromotionTypeEnum :int
    {
        [Description("مدیر مرکز")]
        Manager = 1,
        [Description("مدیر عامل")]
        CEO = 2,
        [Description("نهایی")]
        Final = 3,

    }

    public class SharePromotionType : EnumEntity<SharePromotionTypeEnum>
    {
        public SharePromotionType(SharePromotionTypeEnum enumType) : base(enumType)
        {
        }

        public SharePromotionType() : base() { } // should excplicitly define for EF!
    }
}