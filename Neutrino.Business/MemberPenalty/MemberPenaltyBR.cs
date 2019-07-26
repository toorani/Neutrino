using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class MemberPenaltyCollectionBR : NeutrinoValidator<List<MemberPenalty>>
    {
        public MemberPenaltyCollectionBR(NeutrinoUnitOfWork unitOfWork) : base(unitOfWork)
        {
            RuleFor(x => x.Count)
                .NotEmpty()
                .WithMessage("اطلاعاتی جهت ثبت وجود ندارد")
                .Configure(x => x.CascadeMode = CascadeMode.StopOnFirstFailure);
            RuleFor(x => x)
                .Must(x => lessOrEqualTotalPromotion(x))
                .WithMessage("جمع پورسانت پرسنل باید برابر با پورسانت مرکز باشد ");

            RuleForEach(x => x)
                .SetValidator(x => new MemberPenaltyBR());
        }

        private bool lessOrEqualTotalPromotion(List<MemberPenalty> memberPenalties)
        {
            var branchPromotionId = memberPenalties.First().BranchPromotionId;
            var branchPromotion = unitOfWork.BranchPromotionDataService.GetQuery()
                .AsNoTracking()
                .Single(x=>x.Id == branchPromotionId);
            var totalBranchPromotion = branchPromotion.TotalReceiptPromotion + branchPromotion.SupplierPromotion + branchPromotion.PrivateReceiptPromotion + branchPromotion.TotalSalesPromotion;
            return memberPenalties.Sum(x => x.CEOPromotion) <= totalBranchPromotion;
        }
    }
    public class MemberPenaltyBR : NeutrinoValidator<MemberPenalty>
    {
        #region [ Constructor(s) ]
        public MemberPenaltyBR()
        {
            RuleFor(x => x.MemberId)
                .NotNull()
                .WithMessage("اطلاعات پرسنل وجود ندارد");

            RuleFor(x => x.MemberPromotionId)
                .NotNull()
                .WithMessage("اطلاعات پورسانت تایید شده مدیر مرکز وجود ندارد");

            RuleFor(x => x.BranchPromotionId)
                .NotNull()
                .WithMessage("اطلاعات پورسانت موجود نمیباشد");
        }
        #endregion
    }

}
