using System.ComponentModel;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum GoalGoodsCategoryTypeEnum
    {
        /// <summary>
        /// گروهی
        /// </summary>
        [Description("گروهی")]
        Group = 1,
        /// <summary>
        /// تکی
        /// </summary>
        [Description("تکی")]
        Single = 2,
        /// <summary>
        /// هدف کل
        /// </summary>
        [Description("هدف کل")]
        TotalSalesGoal = 3,
        /// <summary>
        /// هدف وصول کل
        /// </summary>
        [Description("هدف وصول کل")]
        ReceiptTotalGoal = 4,
        /// <summary>
        /// هدف وصول خصوصی
        /// </summary>
        [Description("هدف وصول خصوصی")]
        ReceiptPrivateGoal = 5,
        /// <summary>
        /// هدف وصول دولتی
        /// </summary>
        [Description("هدف وصول دولتی")]
        ReceiptGovernGoal = 6,
        /// <summary>
        /// هدف تجمیعی
        /// </summary>
        [Description("هدف تجمیعی")]
        AggregationGoal = 7
    }

    public class GoalGoodsCategoryType : EnumEntity<GoalGoodsCategoryTypeEnum>
    {
        public GoalGoodsCategoryType(GoalGoodsCategoryTypeEnum enumType) : base(enumType)
        {
        }

        public GoalGoodsCategoryType() : base() { } // should excplicitly define for EF!
    }
}
