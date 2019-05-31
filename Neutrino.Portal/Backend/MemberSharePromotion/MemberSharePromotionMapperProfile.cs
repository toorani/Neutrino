using AutoMapper;
using Espresso.Core;
using Espresso.Entites;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using System.Globalization;
using System.Linq;

namespace Neutrino.Portal
{
    public class MemberSharePromotionMapperProfile : Profile
    {

        public MemberSharePromotionMapperProfile()
        {
            CreateMap<MemberSharePromotionViewModel, MemberSharePromotion>()
                .Ignore(x => x.Member)
                .ReverseMap()
                .ForMember(x => x.CEOPromotion, opt => opt.ResolveUsing(x => x.CEOPromotion ?? x.ManagerPromotion))
                .ForMember(x => x.FinalPromotion, opt => opt.ResolveUsing(x => x.FinalPromotion ?? x.CEOPromotion));

            CreateMap<BranchPromotion, BranchPromotionViewModel>()
                .ForMember(x => x.BranchName, opt => opt.ResolveUsing(x => x.Branch.Name))
                .ForMember(x => x.TotalPromotion, opt => opt.ResolveUsing(x => x.PrivateReceiptPromotion.Value + x.TotalReceiptPromotion.Value + x.TotalSalesPromotion))
                .ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(x => x.Name + " " + x.LastName));

        }
    }
}