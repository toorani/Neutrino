using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IBranchPromotionBS : IBusinessService
    {
        Task<IBusinessResult> AddOrUpdateAsync(List<BranchPromotion> lstEntities);
        Task<IBusinessResultValue<PromotionReviewStatusEnum>> ConfirmCompensatoryAsync(List<BranchPromotion> lstEntities);
        Task<IBusinessResultValue<List<BranchPromotion>>> LoadListAsync(int promotionId);
        Task<IBusinessResultValue<BranchPromotion>> LoadBranchPromotionAsync(int branchId, PromotionReviewStatusEnum promotionReviewStatusId);
        Task<IBusinessResultValue<List<BranchPromotion>>> LoadCompensatoryListAsync(int promotionId);

    }
}
