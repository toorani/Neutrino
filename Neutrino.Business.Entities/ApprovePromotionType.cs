using System.ComponentModel;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum ApprovePromotionTypeEnum
    {
        /// <summary>
        /// مرکز
        /// </summary>
        [Description("مرکز")]
        Branch = 1,
        /// <summary>
        /// عوامل فروش
        /// </summary>
        [Description("عوامل فروش")]
        Seller= 2,
        

    }

    public class ApprovePromotionType : EnumEntity<ApprovePromotionTypeEnum>
    {
        public ApprovePromotionType(ApprovePromotionTypeEnum enumType) : base(enumType)
        {
        }
        public ApprovePromotionType() : base() { } // should excplicitly define for EF!
    }

}