using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum RewardTypeEnum
    {
        [Description("درصد پاداش"),StepItemSingleValue]
        Percent = 1,
        [Description("جایزه عوامل فروش کلی محصول"), StepItemSingleValue]
        TotalGoods,
        [Description("جایزه عوامل فروش هر محصول"), StepItemSingleValue]
        SingleGoods,
        [Description("جایزه عوامل فروش"),StepItemComplexValue]
        Seller,
        [Description("مبلغ کارت هدیه"), StepItemSingleValue]
        GiftCard,
        [Description("جایزه بر فاکتور"), StepItemComplexValue]
        Invoice,
        [Description("سایر جوایز"),StepItemSelectableValue]
        Others
    }

    public class RewardType : EnumEntity<RewardTypeEnum> 
    {
        public RewardType(RewardTypeEnum enumType) : base(enumType)
        {
        }

        public RewardType() : base() { } // should excplicitly define for EF!
    }
}
