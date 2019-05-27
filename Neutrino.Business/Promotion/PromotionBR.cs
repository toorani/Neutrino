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
    public class MemberSharePromotionBR : NeutrinoValidator<MemberSharePromotion>
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Constructor(s) ]
        public MemberSharePromotionBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

            RuleFor(x => x.BranchPromotionId)
                .NotEmpty()
                .WithMessage(".اطلاعات مربوط به پورسانت مرکز مشخص نشده است");

            RuleFor(x => x.MemberId)
                .NotEmpty()
                .WithMessage(".اطلاعات مربوط به پرسنل مشخص نشده است");

            RuleFor(x => x)
                .Must(x => x.CEOPromotion.HasValue || x.FinalPromotion.HasValue || x.ManagerPromotion != 0)
                .WithMessage(".فیلد پورسانت مشخص نشده است");
            RuleFor(x => x)
                .Must(x => checkTotalAssigned(x))
                .WithMessage(".مقدار مشخص شده برای پرسنل نباید از جمع پورسانت مرکز بیشتر باشد");
            RuleFor(x => x)
                .Must(x => !isDuplicate(x))
                .WithMessage("اطلاعات وارد شده تکراری میباشد");
        }

        private bool checkTotalAssigned(MemberSharePromotion entity)
        {
            var branchPromotion = unitOfWork.BranchPromotionDataService.GetQuery()
                .IncludeFilter(x => x.MemberSharePromotions.Where(y => y.Deleted == false && y.Id != entity.Id))
                .AsNoTracking()
                .Single(x => x.Id == entity.BranchPromotionId);
            bool result = false;
            decimal totalAssigned = 0;
            switch (branchPromotion.PromotionReviewStatusId)
            {
                case PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview:
                case PromotionReviewStatusEnum.ReleadedStep1ByBranchManager:
                    totalAssigned = branchPromotion.MemberSharePromotions.Sum(x => (decimal?)x.ManagerPromotion) ?? 0 + entity.ManagerPromotion;
                    break;
                case PromotionReviewStatusEnum.WaitingForCEOReview:
                case PromotionReviewStatusEnum.ReleasedByCEO:
                    totalAssigned = branchPromotion.MemberSharePromotions.Sum(x => x.CEOPromotion) ?? 0 + entity.CEOPromotion.Value;
                    break;
                case PromotionReviewStatusEnum.WaitingForStep2BranchManagerReview:
                case PromotionReviewStatusEnum.ReleadedStep2ByBranchManager:
                    totalAssigned = branchPromotion.MemberSharePromotions.Sum(x => x.FinalPromotion) ?? 0 + entity.FinalPromotion.Value;
                    break;
            }
            result = totalAssigned < (branchPromotion.PrivateReceiptPromotion + branchPromotion.TotalReceiptPromotion + branchPromotion.TotalSalesPromotion);
            return result;
        }

        private bool isDuplicate(MemberSharePromotion entity)
        {
            return unitOfWork.MemberSharePromotionDataService.GetQuery()
                .AsNoTracking()
                .Any(x => x.BranchPromotionId == entity.BranchPromotionId && x.MemberId == entity.MemberId
                && x.Id != entity.Id && x.Deleted == false);
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
