using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Interfaces
{
    public interface IMemberSharePromotionBS : IBusinessService
    {
        Task<IBusinessResult> CreateOrUpdateAsync(List<MemberSharePromotion> memberSharePromotions);
        Task<IBusinessResultValue<List<MemberSharePromotion>>> LoadAsync(int branchId, PromotionReviewStatusEnum promotionReviewStatusId);
        Task<IBusinessResult> RemoveAsync(int branchId, int memberId);
        Task<IBusinessResultValue<PromotionReviewStatusEnum>> ProceedMemberSharePromotionAsync(PromotionReviewStatusEnum currentStep, PromotionReviewStatusEnum nextStep, int branchId);
        Task<IBusinessResult> AddOrModfiyFinalPromotionAsync(List<MemberSharePromotion> entities);
        Task<IBusinessResultValue<PromotionReviewStatusEnum>> DeterminedPromotion(List<MemberSharePromotion> entities);
        Task<IBusinessResultValue<MemberSharePromotion>> LoadMemberSharePromotionAsync(int memberId, int month, int year,SharePromotionTypeEnum sharePromotionTypeId);
        Task<IBusinessResultValue<List<MemberSharePromotion>>> LoadMemberSharePromotionListAsync(int branchId, int month, int year, SharePromotionTypeEnum sharePromotionTypeId);
    }
}
