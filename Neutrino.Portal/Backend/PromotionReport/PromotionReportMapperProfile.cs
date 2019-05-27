using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AutoMapper;
using Espresso.Core;
using Espresso.Entites;
using Neutrino.Entities;

namespace Neutrino.Portal
{
    public class PromotionReportMapperProfile : Profile
    {
        public PromotionReportMapperProfile()
        {
            //CreateMap<BranchPromotion, BranchPromotionViewModel>()
            //    .ForMember(x => x.Branch, opt => opt.ResolveUsing(x => x.Branch.Name))
            //    .ForMember(x => x.TotalAndAggregationReached, opt => opt.ResolveUsing(x => x.TotalSalesReachedPercent > x.AggregationReachedPercent ? x.TotalSalesReachedPercent : x.AggregationReachedPercent))
            //    .ReverseMap();
        }
    }
}