
using System;
using System.Linq;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Entities;
using System.Data.Entity;

namespace Neutrino.Business
{
    public class GoalPercentBusinessRules : NeutrinoValidator<GoalFulfillment>
    {
        #region [ Constructor(s) ]
        public GoalPercentBusinessRules(IEntityRepository<GoalFulfillment> dataService)
            : base(dataService)
        {
            //RuleFor(gCat => gCat.Name)
            //    .NotEmpty()
            //    .WithMessage(".نام گروه دسته دارویی مشخص نشده است");

            //RuleFor(gCat => gCat.GoodsCategoryTypeId)
            //    .NotEmpty()
            //    .WithMessage(".نوع دسته دارویی مشخص نشده است");

            //RuleFor(gCat => gCat.GoodsCollection)
            //    .Must(coll => coll == null || coll.Count > 0)
            //    .WithMessage("دارو(ها) دسته دارویی را مشخص نماید");

            //When(x => x.GoalTypeId == GoalTypeEnum.Distributor, () => {

            //    RuleFor(x => x)
            //    .Must(entity => !IsDuplicate(entity))
            //    .WithMessage(".امکان ثبت دسته دارویی تکراری وجود ندارد");
            //});
            

        }
        #endregion

        #region [ Private Method(s) ]
        private bool IsDuplicate(GoalGoodsCategory goodsCategory)
        {
            var dbGoodsCat = dbContext.GoalGoodsCategories
                                .AsNoTracking()
                                .Where(x => x.GoalGoodsCategoryTypeId == goodsCategory.GoalGoodsCategoryTypeId
                                && x.Name == goodsCategory.Name
                                && (x.Deleted == false || x.IsActive))
                                //.Include(x => x.GoodsCollection)
                                .FirstOrDefault();


            if (dbGoodsCat == null)
                return false;

            //bool result = dbGoodsCat.GoodsCollection
            //    .ToList()
            //    .TrueForAll(x => goodsCategory.GoodsCollection.Any(i => i.Id == x.Id));
            //if (result)
            //    return result;

            return !(dbGoodsCat.Id == dbGoodsCat.Id);


        }
        #endregion

    }
}
