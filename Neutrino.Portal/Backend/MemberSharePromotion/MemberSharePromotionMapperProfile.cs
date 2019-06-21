using AutoMapper;
using Espresso.Core;
using Espresso.Entites;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using System.Collections.Generic;
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
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(x => x.Name + " " + x.LastName))
                .ForMember(x => x.PositionTitle, opt => opt.ResolveUsing(x => x.PositionType?.Description));

            CreateMap<MemberSharePromotionDetail, MemberSharePromotionDetailViewModel>()
                .ReverseMap();
            CreateMap<MemberSharePromotion, MemberSharePromotionManagerViewModel>()
                .ForMember(x => x.BranchSalesPromotion, opt => opt.ResolveUsing(x => x.Details.FirstOrDefault()?.BranchSalesPromotion))
                .ForMember(x => x.ReceiptPromotion, opt => opt.ResolveUsing(x => x.Details.FirstOrDefault()?.ReceiptPromotion))
                .ForMember(x => x.SellerPromotion, opt => opt.ResolveUsing(x => x.Details.FirstOrDefault()?.SellerPromotion))
              .ReverseMap()
              .ForMember(x => x.Details, opt => opt.ResolveUsing(x =>
              {
                  return new List<MemberSharePromotionDetail>()
                  {
                     new MemberSharePromotionDetail(){
                         SellerPromotion = x.SellerPromotion,
                         ReceiptPromotion =x.ReceiptPromotion,
                         BranchSalesPromotion = x.BranchSalesPromotion,
                         MemberId = x.MemberId,
                         MemberSharePromotionId = x.Id
                     }
                  };
              }));


        }
    }
}