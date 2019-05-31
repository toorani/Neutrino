using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Data.EntityFramework;
using Z.EntityFramework.Plus;

namespace Neutrino.Business
{
    public class PromotionBR : NeutrinoValidator<Promotion>
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Constructor(s) ]
        public PromotionBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

            RuleFor(x => x.StartDate)
                .IsValidDateTime()
                .WithMessage(".تاریخ محاسبه پورسانت مشخص نمیباشد");

            RuleFor(x => x)
                .Must(entity => !unitOfWork.PromotionDataService.Exist(where: x => x.StartDate == entity.StartDate && entity.EndDate == entity.EndDate))
                .WithMessage("اطلاعات وارد شده تکراری میباشد");

            RuleFor(x => x)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(entity => CheckExistBranchSales(entity))
                .WithMessage("اطلاعات فروش مراکز وجود ندارد");

            RuleFor(x => x)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => CheckGoal(x, GoalGoodsCategoryTypeEnum.TotalSalesGoal))
                .WithMessage("هدف کل برای ماه و سال انتخاب شده مشخص نشده است")
                .Must(x => CheckTotalFulfillPromotions(x))
                .WithMessage("مقادیر 'درصد از هدف کل ' برای هدف کل مشخص نشده است");
            //.Must(x => CheckBranchGoal(x, GoalGoodsCategoryTypeEnum.TotalSalesGoal))
            //.WithMessage("سهم مراکز به صورت کامل در هدف کل تعریف نشده است");


            RuleFor(x => x)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => CheckGoal(x, GoalGoodsCategoryTypeEnum.ReceiptTotalGoal))
                .WithMessage("هدف وصول برای ماه و سال انتخاب شده مشخص نشده است")
                .Must(x => CheckGoal(x, GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal))
                .WithMessage("هدف وصول برای ماه و سال انتخاب شده مشخص نشده است");


            //.Must(x => CheckBranchGoal(x, GoalGoodsCategoryTypeEnum.ReceiptTotalGoal))
            //.WithMessage("سهم مراکز به صورت کامل در هدف وصول کل تعریف نشده است")
            //.Must(x => CheckBranchGoal(x, GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal))
            //.WithMessage("سهم مراکز به صورت کامل در هدف وصول خصوصی تعریف نشده است");



        }
        private bool CheckExistBranchSales(Promotion entity)
        {
            return unitOfWork.BranchSalesDataService
                .GetQuery()
                .AsNoTracking()
                .Where(x => x.Month == entity.Month && x.Year == entity.Year && x.Deleted == false)
                .Any();
        }
        private bool CheckTotalFulfillPromotions(Promotion entity)
        {
            return unitOfWork.TotalFulfillPromotionPercentDataService
                .GetQuery()
                .AsNoTracking()
                .Count(x => x.Deleted == false) != 0;
        }
        private bool CheckGoal(Promotion entity, GoalGoodsCategoryTypeEnum goalGoodsTypeId)
        {
            var result = unitOfWork.GoalDataService
                .GetQuery()
                .Any(x => x.GoalGoodsCategoryTypeId == goalGoodsTypeId && x.Month == entity.Month && x.Year == entity.Year
            && x.IsUsed == false && x.Deleted == false);
            return result;
        }

        #endregion
    }
   
    public class BranchPromotionBR : NeutrinoValidator<BranchPromotion>
    {
        public BranchPromotionBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            When(x => x.PromotionReviewStatusId == PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview, () =>
            {
                

            });
        }
    }
}
