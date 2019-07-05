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
        /// <summary>
        /// در انتظار تایید ضریب تحقق
        /// </summary>
        [Description("در انتظار تایید ضریب تحقق")]
        WaitingForGoalFulfillment = 1,
        /// <summary>
        /// در صف بررسی
        /// </summary>
        [Description("در صف بررسی")]
        InProcessQueue = 2,
        /// <summary>
        /// در حال محاسبه
        /// </summary>
        [Description("در حال محاسبه")]
        InProcessing = 3,
        /// <summary>
        /// اتمام محاسبات
        /// </summary>
        [Description("اتمام محاسبات")]
        Done = 4
    }

    public class PromotionStatus : EnumEntity<PromotionStatusEnum>
    {
        public PromotionStatus(PromotionStatusEnum enumType) : base(enumType)
        {
        }

        public PromotionStatus() : base() { } // should excplicitly define for EF!
    }
}
