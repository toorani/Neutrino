using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IMemberPenaltyBS : IBusinessService
    {
        Task<IBusinessResultValue<List<MemberPenaltyDTO>>> LoadPenaltiesForPromotionAsync(int branchId);
        Task<IBusinessResultValue<List<MemberPenalty>>> CreateOrModifyAsync(List<MemberPenalty> entities);
        Task<IBusinessResultValue<PromotionReviewStatusEnum>> ReleaseCEOPromotion(List<MemberPenalty> entities);
    }
}
