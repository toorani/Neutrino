using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;

using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class FulfillmentPercentMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public FulfillmentPercentMapperProfile()
        {
            CreateMap<FulfillmentPercent, FulfillmentPercentViewModel>()
            .ForMember(goalVM => goalVM.YearMonth, opt => opt.ResolveUsing(goalDTO =>
                 {
                     var monthName = Utilities.PersianMonthNames().Single(x => x.Key == goalDTO.Month).Value;
                     return $"{monthName} - {goalDTO.Year}";
                 }))
            .ReverseMap()
            .ForMember(dto => dto.Month, opt => opt.ResolveUsing(x => x.Goal.Month))
            .ForMember(dto => dto.Year, opt => opt.ResolveUsing(x => x.Goal.Year));
            
        }
        #endregion


    }
}