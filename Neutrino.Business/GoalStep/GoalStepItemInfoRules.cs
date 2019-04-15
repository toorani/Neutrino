
using System;
using Espresso.Core;
using FluentValidation;
using Neutrino.Entities;

namespace Neutrino.Business
{
    class GoalStepItemInfoRules : AbstractValidator<GoalStepItemInfo>
    {

        #region [ Varibale(s) ]
        StepItemComplexValueAttribute complexItem = new StepItemComplexValueAttribute();
        StepItemSelectableValueAttribute selectableItem = new StepItemSelectableValueAttribute();
        StepItemSingleValueAttribute singleValue = new StepItemSingleValueAttribute();
        #endregion

        #region [ Constructor(s) ]
        public GoalStepItemInfoRules()
            :base()
        {
            When(x => getItemTypeCode(x) == complexItem.Value, () =>
            {
                RuleFor(entity => entity.ComputingTypeId)
                .NotNull()
                .WithMessage(x => GetMessage(x, " نحوه محاسبه"));

                RuleFor(entity => entity.EachValue)
                    .NotNull()
                    .WithMessage(x => GetMessage(x, " واحد"));

                RuleFor(entity => entity.Amount)
                     .NotNull()
                     .WithMessage(x => GetMessage(x, " مقدار"))
                     .NotEqual(0)
                    .WithMessage(x => String.Format("مقدار صفر برای  فیلد {0} معتبر نمیباشد", getItemTypeDescription(x)));

            });

            When(x => getItemTypeCode(x) == singleValue.Value, () =>
            {
                RuleFor(entity => entity.Amount)
                    .NotNull()
                    .WithMessage(x => GetMessage(x, " مقدار"))
                    .NotEmpty()
                    .WithMessage(x => String.Format("مقدار صفر برای  فیلد {0} معتبر نمیباشد", getItemTypeDescription(x)));

            });

            When(x => getItemTypeCode(x) == selectableItem.Value, () =>
            {
                RuleFor(entity => entity.ChoiceValueId)
                    .NotNull()
                    .WithMessage(x => GetMessage(x));
            });

            When(x => x.ActionTypeId == GoalStepActionTypeEnum.Reward && x.ItemTypeId == (int)RewardTypeEnum.Percent, () =>
            {
                RuleFor(entity => entity.Amount)
                .LessThanOrEqualTo(100)
                .WithMessage(".لطفا مقداری بین صفر تا صد برای درصد پاداش مشخص کنید");

                RuleFor(entity => entity.Amount)
                   .GreaterThan(0)
                   .WithMessage(".لطفا مقداری بین صفر تا صد برای درصد پاداش مشخص کنید");
            });
            
        }


        #endregion

        #region [ Private Method(s) ]

        private string GetMessage(GoalStepItemInfo entity, string fieldName = "")
        {
            string msg = "اطلاعات {0} {1} را مشخص نماید";
            return String.Format(msg, fieldName, getItemTypeDescription(entity));
        }

        private RewardType getRewardType(int itemTypeId)
        {
            RewardTypeEnum rewardTypeId = Utilities.ToEnum<RewardTypeEnum>(itemTypeId).Value;
            RewardType rewardType = new RewardType(rewardTypeId);
            return rewardType;
        }

        private CondemnationType getCondemnationType(int itemTypeId)
        {
            CondemnationTypeEnum condemnationTypeId = Utilities.ToEnum<CondemnationTypeEnum>(itemTypeId).Value;
            CondemnationType condemnationType = new CondemnationType(condemnationTypeId);
            return condemnationType;
        }

        private int getItemTypeCode(GoalStepItemInfo entity)
        {
            int? code = null;
            switch (entity.ActionTypeId)
            {
                case GoalStepActionTypeEnum.Reward:
                    code = getRewardType(entity.ItemTypeId).Code;
                    break;
                case GoalStepActionTypeEnum.Condemnation:
                    code = getCondemnationType(entity.ItemTypeId).Code;
                    break;
            }

            return code.HasValue ? code.Value : singleValue.Value;
        }

        private string getItemTypeDescription(GoalStepItemInfo entity)
        {
            string desc = "";
            switch (entity.ActionTypeId)
            {
                case GoalStepActionTypeEnum.Reward:
                    desc = getRewardType(entity.ItemTypeId).Description;
                    break;
                case GoalStepActionTypeEnum.Condemnation:
                    desc = getCondemnationType(entity.ItemTypeId).Description;
                    break;
            }

            return desc;
        }

        #endregion




    }
}
