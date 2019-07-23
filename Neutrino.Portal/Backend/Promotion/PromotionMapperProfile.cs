using AutoMapper;
using Espresso.Core;
using Espresso.Entites;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using System.Globalization;
using System.Linq;

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
                .ForMember(x => x.Status, opt => opt.ResolveUsing(x => x.Status.Description))

                .ReverseMap()
                .Ignore(x => x.Status);

            CreateMap<MemberPromotionViewModel, MemberPromotion>()
                .Ignore(x => x.Member)
                .ReverseMap();
                //.ForMember(x => x.CEOPromotion, opt => opt.ResolveUsing(x => x.CEOPromotion ?? x.ManagerPromotion))
                //.ForMember(x => x.FinalPromotion, opt => opt.ResolveUsing(x => x.FinalPromotion ?? x.CEOPromotion));

            CreateMap<BranchPromotion, BranchPromotionViewModel>()
                .ForMember(x => x.BranchName, opt => opt.ResolveUsing(x => x.Branch.Name))
                .ForMember(x => x.TotalPromotion, opt => opt.ResolveUsing(x => x.PrivateReceiptPromotion.Value + x.TotalReceiptPromotion.Value + x.SupplierPromotion))
                .ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(x => x.Name + " " + x.LastName));

        }
    }
}