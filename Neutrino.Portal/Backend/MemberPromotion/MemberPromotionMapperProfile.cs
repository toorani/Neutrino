﻿using AutoMapper;
using Espresso.Core;
using Espresso.Entites;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Neutrino.Portal
{
    public class MemberPromotionMapperProfile : Profile
    {

        public MemberPromotionMapperProfile()
        {
            CreateMap<MemberPromotionViewModel, MemberPromotion>()
                .Ignore(x => x.Member)
                .ReverseMap()
                .ForMember(x => x.ManagerPromotion, opt => opt.ResolveUsing(x => getTotalPromotion(x, ReviewPromotionStepEnum.Manager)))
                .ForMember(x => x.CEOPromotion, opt => opt.ResolveUsing(x => getTotalPromotion(x, ReviewPromotionStepEnum.CEO)))
                .ForMember(x => x.FinalPromotion, opt => opt.ResolveUsing(x => getTotalPromotion(x, ReviewPromotionStepEnum.Final)));

            CreateMap<BranchPromotion, BranchPromotionViewModel>()
                .ForMember(x => x.BranchName, opt => opt.ResolveUsing(x => x.Branch.Name))
                .ForMember(x => x.TotalPromotion, opt => opt.ResolveUsing(x => x.PrivateReceiptPromotion.Value + x.TotalReceiptPromotion.Value + x.SupplierPromotion + x.TotalSalesPromotion))
                .ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(x => x.Name + " " + x.LastName))
                .ForMember(x => x.PositionTitle, opt => opt.ResolveUsing(x => x.PositionType?.Description));

            CreateMap<MemberPromotionDetail, MemberPromotionDetailViewModel>()
                .ReverseMap();
            CreateMap<MemberPromotion, MemberPromotionManagerViewModel>()
                .ForMember(x => x.SupplierPromotion, opt => opt.ResolveUsing(x => x.Details.FirstOrDefault()?.SupplierPromotion))
                .ForMember(x => x.ReceiptPromotion, opt => opt.ResolveUsing(x => x.Details.FirstOrDefault()?.ReceiptPromotion))
                .ForMember(x => x.CompensatoryPromotion, opt => opt.ResolveUsing(x => x.Details.FirstOrDefault()?.CompensatoryPromotion))
                .ForMember(x => x.BranchSalesPromotion, opt => opt.ResolveUsing(x => x.Details.FirstOrDefault()?.BranchSalesPromotion))
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(x => x.Member.Name + " " + x.Member.LastName))
                .ForMember(x => x.MemberCode, opt => opt.ResolveUsing(x => x.Member.Code))
                .ForMember(x => x.PositionTitle, opt => opt.ResolveUsing(x => x.Member.PositionType?.Description))
              .ReverseMap()
              .Ignore(x => x.Member)
              .ForMember(x => x.Details, opt => opt.ResolveUsing(x =>
              {
                  return new List<MemberPromotionDetail>()
                  {
                     new MemberPromotionDetail(){
                         CompensatoryPromotion = x.CompensatoryPromotion,
                         ReceiptPromotion =x.ReceiptPromotion,
                         SupplierPromotion = x.SupplierPromotion,
                         BranchSalesPromotion = x.BranchSalesPromotion,
                         MemberId = x.MemberId,
                         MemberPromotionId = x.Id,
                         ReviewPromotionStepId = ReviewPromotionStepEnum.Manager
                     }
                  };
              }));


        }

        private decimal getTotalPromotion(MemberPromotion memberPromotion, ReviewPromotionStepEnum reviewPromotionStepId)
        {
            return memberPromotion.Details.Where(c => c.ReviewPromotionStepId == reviewPromotionStepId).Sum(c => c.BranchSalesPromotion + c.CompensatoryPromotion + c.ReceiptPromotion + c.SupplierPromotion);
        }
    }
}