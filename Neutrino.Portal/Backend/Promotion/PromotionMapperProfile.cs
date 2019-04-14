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
    public class PromotionMapperProfile : Profile
    {

        public PromotionMapperProfile()
        {
            var perCal = new PersianCalendar();
            CreateMap<Promotion, PromotionViewModel>()
                .ForMember(x => x.DisplayDate, opt => opt.ResolveUsing(x =>
                {
                    var month = perCal.GetMonth(x.StartDate);
                    var year = perCal.GetYear(x.StartDate);
                    var monthTitle = Utilities.PersianMonthNames().Single(mo => mo.Key == month).Value;

                    return $"{monthTitle} - {year} ";
                }))
                .ForMember(x => x.Status, opt => opt.ResolveUsing(x => x.StatusId.GetEnumDescription()))

                .ReverseMap()
                .Ignore(x => x.Status);

        }
    }
}