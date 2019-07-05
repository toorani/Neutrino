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
        /// <summary>
        /// در انتظار بررسی مرحله اول رییس مرکز
        /// </summary>
        [Description("در انتظار بررسی مرحله اول رییس مرکز")]
        WaitingForStep1BranchManagerReview = 1,
        /// <summary>
        /// تایید مرحله اول رییس مرکز
        /// </summary>
        [Description("تایید مرحله اول رییس مرکز")]
        ReleasedStep1ByBranchManager = 2,
        /// <summary>
        /// تایید مدیر
        /// </summary>
        [Description("تایید مدیر")]
        ReleasedByCEO = 4,
        /// <summary>
        /// تایید نهایی 
        /// </summary>
        [Description("تایید نهایی ")]
        DeterminedPromotion = 6,
        /// <summary>
        /// در صف پورسانت ترمیمی
        /// </summary>
        [Description("در صف پورسانت ترمیمی")]
        WaitingForCompensatory = 7
    }

    public class PromotionReviewStatus : EnumEntity<PromotionReviewStatusEnum>
    {
        public PromotionReviewStatus(PromotionReviewStatusEnum enumType) : base(enumType)
        {
        }

        public PromotionReviewStatus() : base() { } // should excplicitly define for EF!
    }
}
