using AutoMapper;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    internal class PenaltyMapperProfile : Profile
    {
        public PenaltyMapperProfile()
        {
            CreateMap<PenaltyViewModel, MemberPenaltyDTO>()
                .ForMember(x => x.CEOPromotion, opt => opt.ResolveUsing(record => record.ManagerPromotion - record.Deduction + record.Credit))
                .ReverseMap();
            CreateMap<PenaltyViewModel, MemberPenalty>()
                .ForMember(x => x.CEOPromotion, opt => opt.ResolveUsing(record => record.ManagerPromotion - record.Deduction + record.Credit))
                .ReverseMap();
        }
    }
}