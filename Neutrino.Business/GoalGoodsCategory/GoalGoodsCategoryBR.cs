
using System;
using System.Linq;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Entities;
using System.Data.Entity;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class GoalGoodsCategoryBR : NeutrinoValidator<GoalGoodsCategory>
    {
        #region [ Constructor(s) ]
        public GoalGoodsCategoryBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            RuleFor(gCat => gCat.Name)
                .NotEmpty()
                .WithMessage(".نام گروه دسته دارویی مشخص نشده است");

            RuleFor(gCat => gCat.GoalGoodsCategoryTypeId)
                .NotEmpty()
                .WithMessage(".نوع دسته دارویی مشخص نشده است");
            When(x => x.GoalGoodsCategoryTypeId != GoalGoodsCategoryTypeEnum.TotalSalesGoal, () =>
            {
                RuleFor(gCat => gCat.GoodsCollection)
                .Must(coll => coll == null || coll.Count > 0)
                .WithMessage("دارو(ها) دسته دارویی را مشخص نماید");
            });

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
            var dbGoodsCat = unitOfWork.GoalGoodsCategoryDataService
                .GetQuery()
                .AsNoTracking()
                .Where(x => x.GoalGoodsCategoryTypeId == goodsCategory.GoalGoodsCategoryTypeId
                && x.Name == goodsCategory.Name && (x.Deleted == false || x.IsActive))
                .FirstOrDefault();
            if (dbGoodsCat == null)
                return false;

            return !(dbGoodsCat.Id == goodsCategory.Id);
        }
        #endregion

    }
}
