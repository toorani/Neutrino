using System;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class QuantityConditionBR : NeutrinoValidator<QuantityCondition>
    {
        public QuantityConditionBR(NeutrinoUnitOfWork unitOfWork):base(unitOfWork)
        {
            RuleFor(x => x.GoalId)
                .NotEqual(0)
                .WithMessage("اطلاعات هدف موجود نمیباشد");

            RuleFor(x => x)
                .Must(x => x.Quantity != 0)
                .WithMessage("تعداد ایتم را مشخص نماید")
                .Must(entity => checkedLessThanGoodsCount(entity))
                .WithMessage("تعداد ایتم باید از تعداد داروهای موجود در هدف کمتر یا مساوی باشد");

            When(x => x.QuantityConditionTypeId == QuantityConditionTypeEnum.DependedOnGoal, () =>
             {
                 RuleFor(x => x.ExtraEncouragePercent)
                 .NotEmpty()
                 .WithMessage("درصد پاداش اضافی را مشخص نماید");
             });
        }

        private bool checkedLessThanGoodsCount(QuantityCondition entity)
        {
            var goalEntity = unitOfWork.GoalDataService.FirstOrDefault(x => x.Id == entity.GoalId);
            var count = unitOfWork.GoalGoodsCategoryGoodsDataService.GetCount(x => x.GoalGoodsCategoryId == goalEntity.GoalGoodsCategoryId);
            return count >= entity.Quantity;
        }
    }

    class GoodsQuantityConditionRule : AbstractValidator<GoodsQuantityCondition>
    {
        public GoodsQuantityConditionRule()
        {
            //RuleFor(x => x.Quantity)
            //    .LessThan(0)
            //    .WithMessage(x =>
            //    {
            //        return string.Format("تعداد فراورده مرتبط با {0} را مشخص نماید", x.Goods.EnName);
            //    });

            RuleFor(x => x.GoodsId)
                .NotEqual(0)
                .WithMessage("اطلاعات فراورده موجود نمیباشد");
            RuleForEach(x => x.BranchQuantityConditions)
                .SetValidator(x => new BranchQuantityConditionRules(x.Goods));
        }
    }

    class BranchQuantityConditionRules : AbstractValidator<BranchQuantityCondition>
    {
        public BranchQuantityConditionRules(Goods goods)
        {
            RuleFor(x => x.Quantity)
                .NotEqual(0)
                .WithMessage(x =>
                {
                    return string.Format("تعداد شعبه {0} مرتبط با فراورده {1} را مشخص نماید", goods.EnName, x.Branch.Name);
                });

            RuleFor(x => x.BranchId)
                .NotEqual(0)
                .WithMessage(string.Format("اطلاعات شعبه برای فراورده {0} موجود نمیباشد", goods.EnName));

        }
    }
}
