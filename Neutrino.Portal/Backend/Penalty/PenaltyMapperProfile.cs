using AutoMapper;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    internal class PenaltyMapperProfile : Profile
    {
        public PenaltyMapperProfile()
        {
            CreateMap<PenaltyViewModel, MemberPenalty>()
                .ForMember(x => x.CEOPromotion, opt => opt.ResolveUsing(record => record.ManagerPromotion - record.Deduction + record.Credit))
                .ReverseMap()
                .ForMember(x => x.MemberName, opt => opt.ResolveUsing(x => x.Member.Name + " " + x.Member.LastName))
                .ForMember(x => x.ManagerPromotion, opt => opt.ResolveUsing(x => x.MemberSharePromotion.ManagerPromotion));



        }
    }
}