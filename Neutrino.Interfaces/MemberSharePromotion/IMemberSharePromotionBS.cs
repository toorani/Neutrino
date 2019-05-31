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
        Task<IBusinessResult> CreateOrUpdateAsync(MemberSharePromotion entity);
        Task<IBusinessResultValue<List<MemberSharePromotion>>> LoadAsync(int branchId, PromotionReviewStatusEnum promotionReviewStatusId);
        Task<IBusinessResult> RemoveAsync(int branchId, int memberId);
        Task<IBusinessResult> ProceedMemberSharePromotionAsync(PromotionReviewStatusEnum currentStep, PromotionReviewStatusEnum nextStep, int branchId);
    }
}
