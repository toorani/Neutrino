using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum PromotionReviewStatusEnum
    {
        [Description("در انتظار بررسی مرحله اول رییس مرکز")]
        WaitingForStep1BranchManagerReview = 1,
        [Description("تایید مرحله اول رییس مرکز")]
        ReleadedStep1ByBranchManager = 2,
        [Description("تایید مدیر")]
        ReleasedByCEO = 4,
        [Description("تایید نهایی ")]
        DeterminedPromotion = 6,
    }

    public class PromotionReviewStatus : EnumEntity<PromotionReviewStatusEnum>
    {
        public PromotionReviewStatus(PromotionReviewStatusEnum enumType) : base(enumType)
        {
        }

        public PromotionReviewStatus() : base() { } // should excplicitly define for EF!
    }
}
