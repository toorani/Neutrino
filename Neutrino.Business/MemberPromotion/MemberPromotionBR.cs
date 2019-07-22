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
    public class MemberPromotionBR : NeutrinoValidator<MemberPromotion>
    {
        #region [ Constructor(s) ]
        public MemberPromotionBR(NeutrinoUnitOfWork unitOfWork)
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
        private bool checkTotalAssigned(MemberPromotion entity)
        {
            var branchPromotion = unitOfWork.BranchPromotionDataService.GetQuery()
                .IncludeFilter(x => x.MemberPromotions.Where(y => y.Deleted == false && y.Id != entity.Id))
                .AsNoTracking()
                .Single(x => x.Id == entity.BranchPromotionId);
            bool result = false;
            decimal totalAssigned = 0;
            switch (branchPromotion.PromotionReviewStatusId)
            {
                case PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview:
                case PromotionReviewStatusEnum.ReleasedStep1ByBranchManager:
                    totalAssigned = branchPromotion.MemberPromotions.Sum(x => (decimal?)x.ManagerPromotion) ?? 0 + entity.ManagerPromotion;
                    break;
                case PromotionReviewStatusEnum.ReleasedByCEO:
                    totalAssigned = branchPromotion.MemberPromotions.Sum(x => x.FinalPromotion) ?? 0 + entity.FinalPromotion.Value;
                    break;
            }
            result = totalAssigned < (branchPromotion.PrivateReceiptPromotion + branchPromotion.TotalReceiptPromotion + branchPromotion.TotalSalesPromotion);
            return result;
        }
        private bool isDuplicate(MemberPromotion entity)
        {
            return unitOfWork.MemberPromotionDataService.GetQuery()
                .AsNoTracking()
                .Any(x => x.BranchPromotionId == entity.BranchPromotionId && x.MemberId == entity.MemberId
                && x.Id != entity.Id && x.Deleted == false);
        }
        #endregion
    }

    public class MemberPromotionCollectionBR : NeutrinoValidator<List<MemberPromotion>>
    {
        private StringBuilder @string = new StringBuilder();
        int branchPromotionId = 0;
        BranchPromotion branchPromotion = null;
        #region [ Constructor(s) ]
        public MemberPromotionCollectionBR(NeutrinoUnitOfWork unitOfWork)
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

        private bool checkTotalSellerPromotion(List<MemberPromotion> entities)
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


            var sellerTotalPromotion = unitOfWork.SellerPromotionDataService.GetQuery()
                .AsNoTracking()
                .Where(x => x.Deleted == false && x.BranchPromotionId == branchPromotionId)
                .Sum(x => x.Promotion);
            return sellerTotalPromotion >= sellerTotalPromotion_input;
        }

        private bool checkTotalReceiptPromotion(List<MemberPromotion> entities)
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

        private bool checkTotalSalesPromotion(List<MemberPromotion> entities)
        {
            branchPromotionId = entities.First().BranchPromotionId;
            branchPromotion = unitOfWork.BranchPromotionDataService.GetQuery()
                .AsNoTracking()
                .Single(x => x.Id == branchPromotionId);

            var branchSalesTotalPromotion = (from memp in entities
                                             from mempde in memp.Details
                                             select mempde.SupplierPromotion).Sum();
            return branchPromotion.TotalSalesPromotion >= branchSalesTotalPromotion;
        }

        
        #endregion
    }

}
