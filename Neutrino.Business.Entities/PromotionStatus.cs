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
        [Description("ثبت اولیه")]
        Initialized = 1,
        [Description("در حال محاسبه")]
        InProcessing = 2,
        [Description("محاسبه شده")]
        Done = 3,
        [Description("در صف بررسی")]
        InProcessQueue = 4,
        [Description("در انتظار تایید شرط تحقق")]
        WaitingForGoalFulfillment = 5,
        [Description("اتمام محاسبه کلیه اهداف")]
        GoalCalculated = 6
    }

    public class PromotionStatus : EnumEntity<PromotionStatusEnum>
    {
        public PromotionStatus(PromotionStatusEnum enumType) : base(enumType)
        {
        }

        public PromotionStatus() : base() { } // should excplicitly define for EF!
    }
}
