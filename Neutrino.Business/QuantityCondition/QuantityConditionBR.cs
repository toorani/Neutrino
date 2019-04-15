using FluentValidation;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class QuantityConditionBR : AbstractValidator<QuantityCondition>
    {
        public QuantityConditionBR()
        {
            RuleFor(x => x.GoalId)
                .NotEqual(0)
                .WithMessage("اطلاعات هدف موجود نمیباشد");

            RuleFor(x => x.Quantity)
                .NotEqual(0)
                .WithMessage("تعداد ایتم را مشخص نماید");

            RuleFor(x => x.ExtraEncouragePercent)
                .NotEmpty()
                .WithMessage("درصد پاداش اضافی را مشخص نماید");

            //RuleFor(x => x.ForthCasePercent)
            //    .NotEqual(0)
            //    .WithMessage("درصد حالت چهارم را مشخص نماید");

            //RuleFor(x => x.NotReachedPercent)
            //    .NotEqual(0)
            //    .WithMessage("درصد نزده را مشخص نماید");

            //RuleFor(x => x.GoodsQuantityConditions)
            //    .SetCollectionValidator(new GoodsQuantityConditionRule());
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
            RuleFor(x => x.BranchQuantityConditions)
                .SetCollectionValidator(x => new BranchQuantityConditionRules(x.Goods));
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
