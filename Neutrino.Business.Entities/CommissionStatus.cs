using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum PromotionStatusEnum
    {
        [Description("در حال محاسبه")]
        InProcessing = 1,
        [Description("محاسبه شده")]
        Done = 2,
        [Description("در صف بررسی")]
        InProcessQueue = 3,
        [Description("در انتظار تایید شرط تحقق")]
        WaitingForGoalFulfillment = 4

    }

    public class PromotionStatus : EnumEntity<PromotionStatusEnum>
    {
        public PromotionStatus(PromotionStatusEnum enumType) : base(enumType)
        {
        }

        public PromotionStatus() : base() { } // should excplicitly define for EF!
    }
}
