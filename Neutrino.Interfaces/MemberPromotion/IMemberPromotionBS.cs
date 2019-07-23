using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Interfaces
{
    public interface IMemberPromotionBS : IBusinessService
    {
        Task<IBusinessResult> CreateOrUpdateAsync(List<MemberPromotion> memberPromotions);
        Task<IBusinessResultValue<List<MemberPromotion>>> LoadAsync(int branchId, PromotionReviewStatusEnum promotionReviewStatusId);
        Task<IBusinessResult> RemoveAsync(int branchId, int memberId);
        Task<IBusinessResultValue<PromotionReviewStatusEnum>> ProceedMemberPromotionAsync(PromotionReviewStatusEnum currentStep, PromotionReviewStatusEnum nextStep, int branchId);
        Task<IBusinessResult> AddOrModfiyFinalPromotionAsync(List<MemberPromotion> entities);
        Task<IBusinessResultValue<PromotionReviewStatusEnum>> DeterminedPromotion(List<MemberPromotion> entities);
        Task<IBusinessResultValue<MemberPromotion>> LoadMemberPromotionAsync(int memberId, int month, int year,ReviewPromotionStepEnum sharePromotionTypeId);
        Task<IBusinessResultValue<List<MemberPromotion>>> LoadMemberPromotionListAsync(int branchId, int month, int year, ReviewPromotionStepEnum sharePromotionTypeId);
    }
}
