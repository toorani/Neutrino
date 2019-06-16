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
            CreateMap<ReportBranchPromotionOverview, BranchPromotionViewModel>()
                .ForMember(x => x.TotalAndAggregationReached, opt => opt.ResolveUsing(x => x.TotalSalesFulfilledPercent > x.AggregationFulfilledPercent ? x.TotalSalesFulfilledPercent : x.AggregationFulfilledPercent))
                .ReverseMap();
        }
    }
}