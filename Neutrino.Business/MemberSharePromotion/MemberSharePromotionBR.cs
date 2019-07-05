using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
                case PromotionReviewStatusEnum.ReleasedStep1ByBranchManager:
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
        StringBuilder @string = new StringBuilder();
        int branchPromotionId = 0;
        BranchPromotion branchPromotion = null;
        #region [ Constructor(s) ]
        public MemberSharePromotionCollectionBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

            RuleFor(x => x.Count)
                .NotEmpty()
                .WithMessage("اطلاعاتی جهت ثبت وجود ندارد")
                .Configure(x => x.CascadeMode = CascadeMode.StopOnFirstFailure);

            RuleFor(x => x)
                .Must(x => checkTotalSalesPromotion(x))
                .WithMessage("مجموع پورسانت پرسنل برای تامین کننده نمیتواند از پورسانت تامین کننده مرکز بیشتر باشد");

            RuleFor(x => x)
                .Must(x => checkTotalReceiptPromotion(x))
                .WithMessage("مجموع پورسانت پرسنل برای وصول نمیتواند از پورسانت وصول مرکز بیشتر باشد");

            RuleFor(x => x)
               .Must(x => checkTotalSellerPromotion(x))
               .WithMessage("مجموع پورسانت عوامل فروش نمیتواند از جمع پورسانت عوامل بیشتر باشد");
        }

        private bool checkTotalSellerPromotion(List<MemberSharePromotion> entities)
        {
            if (branchPromotion == null)
            {
                branchPromotionId = entities.First().BranchPromotionId;
                branchPromotion = unitOfWork.BranchPromotionDataService.GetQuery()
                    .AsNoTracking()
                    .Single(x => x.Id == branchPromotionId);
            }

            var sellerTotalPromotion_input = (from memp in entities
                                              from mempde in memp.Details
                                              select mempde.CompensatoryPromotion).Sum();


            var sellerTotalPromotion = unitOfWork.MemberPromotionDataService.GetQuery()
                .AsNoTracking()
                .Where(x => x.Deleted == false && x.BranchPromotionId == branchPromotionId)
                .Sum(x => x.Promotion);
            return sellerTotalPromotion >= sellerTotalPromotion_input;
        }

        private bool checkTotalReceiptPromotion(List<MemberSharePromotion> entities)
        {
            if (branchPromotion == null)
            {
                branchPromotionId = entities.First().BranchPromotionId;
                branchPromotion = unitOfWork.BranchPromotionDataService.GetQuery()
                    .AsNoTracking()
                    .Single(x => x.Id == branchPromotionId);
            }

            var receiptTotalPromotion = (from memp in entities
                                         from mempde in memp.Details
                                         select mempde.ReceiptPromotion).Sum();
            return branchPromotion.TotalReceiptPromotion + branchPromotion.PrivateReceiptPromotion >= receiptTotalPromotion;
        }

        private bool checkTotalSalesPromotion(List<MemberSharePromotion> entities)
        {
            branchPromotionId = entities.First().BranchPromotionId;
            branchPromotion = unitOfWork.BranchPromotionDataService.GetQuery()
                .AsNoTracking()
                .Single(x => x.Id == branchPromotionId);

            var branchSalesTotalPromotion = (from memp in entities
                                             from mempde in memp.Details
                                             select mempde.BranchSalesPromotion).Sum();
            return branchPromotion.TotalSalesPromotion >= branchSalesTotalPromotion;
        }

        
        #endregion
    }

}
