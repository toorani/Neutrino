using System.Collections.Generic;
using FluentValidation;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class FulfillmentPromotionConditionListBR : AbstractValidator<List<FulfillmentPromotionCondition>>
    {
        #region [ Constructor(s) ]
        public FulfillmentPromotionConditionListBR()
        {
            RuleFor(x => x.Count)
                .NotEqual(0)
                .WithMessage("اطلاعاتی جهت ثبت وجود ندارد");
            RuleForEach(x => x)
                .SetValidator(x => new FulfillmentPromotionConditionBR());

        }
        #endregion
    }

    class FulfillmentPromotionConditionBR : AbstractValidator<FulfillmentPromotionCondition>
    {
        public FulfillmentPromotionConditionBR()
        {
            RuleFor(x => x)
                .Must(x=>x.ManagerPromotion.HasValue || x.PrivateReceiptFulfilledPercent.HasValue
                || x.SellerPromotion.HasValue || x.TotalReceiptFulfilledPercent.HasValue
                || x.TotalSalesFulfilledPercent.HasValue)
                .WithMessage("یکی از فیلد های اطلاعاتی باید مشخص شود");
            RuleFor(x => x)
                .Must(x => x.SellerPromotion.HasValue && x.ManagerPromotion.HasValue)
                .WithMessage("امکان ثبت رکورد در زمانی که هر دو پورسانت فروشنده و مدیر مشخص شده است ،وجود ندارد");
        }
    }
}
