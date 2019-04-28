using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Data.Entity;
using System;

namespace Neutrino.Business
{
    class BranchGoalBusinessRules : NeutrinoValidator<BranchGoal>
    {
        #region [ Constructor(s) ]

        public BranchGoalBusinessRules()
            : base()
        {
            RuleFor(entity => entity.BranchId)
               .NotNull()
               .WithMessage(".مرکز مشخص نشده است");

            RuleFor(entity => entity.GoalId)
                .NotEmpty()
                .WithMessage(".هدف مشخص نشده است");
        }

        #endregion
    }

    public class BranchGoalBatchRules : AbstractValidator<BranchGoalDTO>
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Constructor(s) ]
        public BranchGoalBatchRules()
        {
            RuleForEach(entity => entity.Items)
               .SetValidator(x => new BranchGoalBatchItemRules());
            RuleFor(entity => entity.Items)
                .Must(x => CheckTotalPercent(x))
                .WithMessage("جمع درصد سهام وارد شده برای مراکز نباید بیشتر از 100 باشد");
            When(x => x.Goal.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal
            || x.Goal.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptGovernGoal
            || x.Goal.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal, () =>
            {
                RuleFor(entity => entity)
                .Must(x => CheckTotalAmount(x))
                .WithMessage("جمع مبالغ وصول مراکز نباید بیشتر از مبلغ وصول باشد");
            });
        }

        private bool CheckTotalAmount(BranchGoalDTO dto)
        {
            decimal totalAmount = 0;
            dto.Items.ForEach((item) =>
            {
                if (item.Amount.HasValue)
                    totalAmount += item.Amount.Value;
            });

            return totalAmount <= dto.Goal.Amount;
        }

        private bool CheckTotalPercent(List<BranchGoalItem> items)
        {
            decimal totalPercent = 0;
            items.ForEach((item) =>
            {
                if (item.Percent.HasValue)
                    totalPercent += item.Percent.Value;
            });
            return Math.Round(totalPercent, 3) <= 100;
        }
        #endregion
    }

    class BranchGoalBatchItemRules : AbstractValidator<BranchGoalItem>
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Constructor(s) ]
        public BranchGoalBatchItemRules()
        {
            When(x => x.Percent != null, () =>
            {
                RuleFor(entity => entity.BranchId)
               .NotNull()
               .WithMessage(".مرکز را مشخص نشده است");

                RuleFor(entity => entity.Percent)
                .GreaterThanOrEqualTo(0)
                .LessThan(100)
                .WithMessage(".لطفا مقداری بین صفر تا صد برای درصد سهم مشخص کنید");

            });
        }
        #endregion
    }

}
