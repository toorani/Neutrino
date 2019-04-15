using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class Goal : EntityBase
    {
        /// <summary>
        /// شناسه شرکت هدف گذار
        /// در صورتی که مقدار خالی داشته باشد ،هدف گذاری برای شرکت الیت انجام میشود
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// شرکت هدف گذار
        /// </summary>
        public virtual Company Company { get; set; }
        /// <summary>
        /// شناسه دسته دارویی
        /// </summary>
        public int GoalGoodsCategoryId { get; set; }
        /// <summary>
        /// دسته دارویی
        /// </summary>
        public virtual GoalGoodsCategory GoalGoodsCategory { get; set; }
        /// <summary>
        /// دسته دارویی
        /// </summary>
        public virtual GoalGoodsCategoryTypeEnum GoalGoodsCategoryTypeId { get; set; }
        [ForeignKey("GoalGoodsCategoryTypeId")]
        public virtual GoalGoodsCategoryType GoalGoodsCategoryType { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        [Required]
        public virtual DateTime StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        [Required]
        public virtual DateTime EndDate { get; set; }
        /// <summary>
        /// سال هدف 
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// ماه هدف
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// پله های هدف
        /// </summary>
        public virtual ICollection<GoalStep> GoalSteps { get; set; }
        /// <summary>
        /// نوع هدف گذاری 
        /// توزیع کننده - تامین کننده
        /// </summary>
        [Required]
        public GoalTypeEnum GoalTypeId { get; set; }
        [ForeignKey("GoalTypeId")]
        public GoalType GoalType { get; set; }
        [NotMapped]
        public List<Goods> GoodsSelectionList { get; set; }
        /// <summary>
        /// برای محاسبه پورسانت استفاده شده است؟
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// لیست سهم مراکز از هدف
        /// </summary>
        public virtual ICollection<BranchGoal> BranchGoals { get; set; }
        /// <summary>
        /// مبلغ هدف کل /هدف وصول
        /// </summary>
        [NotMapped]
        public decimal? Amount { get; set; }
        [Required]
        public virtual ComputingTypeEnum ComputingTypeId { get; set; }
        [ForeignKey("ComputingTypeId")]
        public virtual ComputingType ComputingType { get; set; }
        public int? PromotionId { get; set; }
        public virtual Promotion Promotion { get; set; }
        
        public virtual ApprovePromotionTypeEnum? ApprovePromotionTypeId { get; set; }
        [ForeignKey("ApprovePromotionTypeId")]
        public virtual ApprovePromotionType ApprovePromotionType { get; set; }
        
        public virtual ICollection<BranchReceiptGoalPercent> BranchReceiptGoalPercent { get; set; }
        public virtual ICollection<GoalNonFulfillmentPercent> GoalNonFulfillmentPercents { get; private set; }
        public Goal()
        {
            GoalSteps = new HashSet<GoalStep>();
            GoodsSelectionList = new List<Goods>();
            IsUsed = false;
            BranchGoals = new HashSet<BranchGoal>();
            BranchReceiptGoalPercent = new HashSet<BranchReceiptGoalPercent>();
            GoalNonFulfillmentPercents = new HashSet<GoalNonFulfillmentPercent>();
        }

    }
}
