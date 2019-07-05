using AutoMapper;
using Neutrino.Entities;

namespace Neutrino.Portal
{
    public class BranchPromotionMapperProfile : Profile
    {

        public BranchPromotionMapperProfile()
        {
            CreateMap<BranchPromotion, BranchPromotionViewModel>()
                .ForMember(x => x.BranchName, opt => opt.ResolveUsing(x => x.Branch.Name));
        }
    }
}