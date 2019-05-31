using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Neutrino.Business
{
    public class MemberSharePromotionBR : NeutrinoValidator<MemberSharePromotion>
    {
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
                case PromotionReviewStatusEnum.ReleasedByCEO:
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

    public class MemberSharePromotionCollectionBR : NeutrinoValidator<List<MemberSharePromotion>>
    {
        #region [ Constructor(s) ]
        public MemberSharePromotionCollectionBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

            RuleFor(x => x.Count)
                .NotEmpty()
                .WithMessage("اطلاعاتی جهت ثبت وجود ندارد")
                .Configure(x => x.CascadeMode = CascadeMode.StopOnFirstFailure);
            RuleFor(x => x)
                .Must(x => lessOrEqualTotalPromotion(x))
                .WithMessage("جمع پورسانت پرسنل باید برابر با پورسانت مرکز باشد ");
        }
        private bool lessOrEqualTotalPromotion(List<MemberSharePromotion> memberPenalties)
        {
            var branchPromotionId = memberPenalties.First().BranchPromotionId;
            var branchPromotion = unitOfWork.BranchPromotionDataService.GetQuery()
                .AsNoTracking()
                .Single(x => x.Id == branchPromotionId);
            var totalBranchPromotion = branchPromotion.TotalReceiptPromotion + branchPromotion.TotalSalesPromotion + branchPromotion.PrivateReceiptPromotion;
            return memberPenalties.Sum(x => x.FinalPromotion) <= totalBranchPromotion;
        }
        #endregion
    }

}
