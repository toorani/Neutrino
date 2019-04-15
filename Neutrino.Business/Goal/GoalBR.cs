using System;
using System.Data.Entity;
using System.Linq;
using Espresso.BusinessService;
using Espresso.Core;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class GoalBR : NeutrinoValidator<Goal>
    {
        #region [ Constructor(s) ]
        public GoalBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            RuleFor(x => x.IsUsed)
                .Equal(false)
                .WithMessage("با توجه به اینکه با هدف انتخاب شده محاسبات انجام شده است ،امکان اصلاح وجود ندارد")
                .Configure(x => x.CascadeMode = CascadeMode.StopOnFirstFailure);

            When(x => x.GoalTypeId == GoalTypeEnum.Distributor, () =>
           {
               When(x => x.GoalGoodsCategoryTypeId <= GoalGoodsCategoryTypeEnum.Group, () =>
               {
                   RuleFor(entity => entity.GoalGoodsCategoryId)
                   .NotEmpty()
                   .WithMessage(".دسته بندی دارویی مشخص نشده است");

                   RuleFor(entity => entity.GoalGoodsCategoryTypeId)
                   .NotEmpty()
                   .WithMessage(".نوع دسته بندی مشخص نشده است");
               });
               When(goalEntity => goalEntity.GoalGoodsCategoryTypeId >= GoalGoodsCategoryTypeEnum.TotalSalesGoal &&
                   goalEntity.GoalGoodsCategoryTypeId <= GoalGoodsCategoryTypeEnum.ReceiptGovernGoal, () =>
               {
                   RuleFor(entity => entity.Amount)
                   .NotNull()
                   .WithMessage(".مبلغ هدف مشخص نشده است");

               });

               When(goalEntity => goalEntity.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.AggregationGoal, () =>
               {
                   RuleFor(entity => entity.Amount)
                   .NotNull()
                   .WithMessage(".مبلغ هدف مشخص نشده است")
                   .GreaterThanOrEqualTo(x => unitOfWork.GoalDataService.GetPreviousAggregationValue(x.Month, x.Year))
                   .WithMessage(".مبلغ هدف تجمیعی باید از مبلغ تجمیعی ماه قبل بزرگتر یا مساوی باشد")
                   .LessThanOrEqualTo(x => unitOfWork.GoalDataService.GetNextAggregationValue(x.Month, x.Year, x.Amount.Value))
                   .WithMessage(".مبلغ هدف تجمیعی باید از مبلغ تجمیعی ماه بعد کوچکتر یا مساوی باشد");

               });


               RuleFor(entity => entity.ComputingTypeId)
               .NotEmpty()
               .WithMessage(".نحوه محاسبه مشخص نشده است");

           });
            //TODO : it should be reviewed at supplier goal developing time   
            //When(x => x.GoalTypeId == GoalTypeEnum.Supplier, () =>
            //{
            //    RuleFor(x => x.CompanyId)
            //    .NotNull()
            //    .WithMessage("لطفا شرکت تامین کننده را مشخص نماید");

            //    RuleFor(x => x.GoalGoodsCategory)
            //    .SetValidator(new GoalGoodsCategoryBusinessRules(goalGoodsCategoryDS));
            //});

            RuleFor(entity => entity.StartDate)
                .IsValidDateTime()
                .WithMessage(".مقدار تاریخ شروع هدف گذاری معتبر نمیباشد");

            RuleFor(entity => entity.EndDate)
               .IsValidDateTime()
               .WithMessage(".مقدار تاریخ پایان هدف گذاری معتبر نمیباشد");



            RuleFor(entity => entity.StartDate)
                .NotEmpty()
                .WithMessage(".تاریخ شروع هدف گذاری مشخص نشده است")
                .LessThan(m => m.EndDate)
                .WithMessage("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");


            RuleFor(entity => entity)
                .Must(entity => !IsDuplicate(entity))
                .WithMessage(".با توجه به تاریخ انتخاب شده هدفگذاری تکراری میباشد");


            //RuleFor(x => x)
            //    .Must(x => CheckStartDate(x))
            //    .WithMessage("به دلیل وجود هدفگذاری با تاریخ شروع بزرگتر از هدف وارد شده امکان ثبت وجود ندارد.");

        }

        #endregion

        #region [ Private Method(s) ]
        private bool CheckDefaultDate(DateTime arg)
        {
            return arg == DateTime.MinValue;
        }
        private bool IsDuplicate(Goal goal)
        {
            var query = unitOfWork.GoalDataService
                .GetQuery()
                .AsNoTracking()
                .FirstOrDefault(
               x => x.GoalGoodsCategoryId == goal.GoalGoodsCategoryId
               && (x.Deleted == false)
               && x.StartDate.CompareTo(goal.StartDate) == 0);


            if (query == null)
                return false;

            return !(query.Id == goal.Id);
        }
        private bool CheckStartDate(Goal entity)
        {
            var checkBeforeStartDate = unitOfWork.GoalDataService.FirstOrDefault(where: x =>
                x.Deleted == false
                && x.StartDate > entity.StartDate
                && x.GoalGoodsCategoryId == entity.GoalGoodsCategoryId
                && x.Id != entity.Id
                , orderBy: Utilities.GetOrderBy<Goal>("StartDate", "asc")
                , includes: x => new { x.GoalGoodsCategory });

            return checkBeforeStartDate == null;
        }


        #endregion
    }
}
