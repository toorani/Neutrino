using AutoMapper;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.WebApiControllers
{
    internal class MemberMapperProfile : Profile
    {
        public MemberMapperProfile()
        {
            CreateMap<Member, MemberViewModel>()
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(x => x.Name + " " + x.LastName));
        }
    }
}