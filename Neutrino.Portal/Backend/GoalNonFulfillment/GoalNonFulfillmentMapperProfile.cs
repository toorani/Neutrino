using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class GoalNonFulfillmentMapperProfile : Profile
    {
        public GoalNonFulfillmentMapperProfile()
        {
            CreateMap<Goal, GoalNonFulfillmentCollectionViewModel>()
                .ForMember(x => x.EndGoalDate, opt => opt.ResolveUsing(x => Utilities.ToPersianDate(x.EndDate)))
                .ForMember(x => x.StartGoalDate, opt => opt.ResolveUsing(x => Utilities.ToPersianDate(x.StartDate)))
                .ForMember(x => x.GoalGoodsCategoryName, opt => opt.ResolveUsing(x => x.GoalGoodsCategory.Name))
                .ForMember(x => x.GoalGoodsCategoryId, opt => opt.ResolveUsing(x => x.GoalGoodsCategoryId))
                .ForMember(x => x.GoalId, opt => opt.ResolveUsing(x => x.Id))
                .ForMember(x => x.IsGoalUsed, opt => opt.ResolveUsing(x => x.IsUsed))
                .ForMember(x => x.Items, opt => opt.ResolveUsing(dto => dto.GoalNonFulfillmentPercents));

            CreateMap<GoalNonFulfillmentPercent, GoalNonFulfillmentViewModel>()
                .ForMember(x => x.Branches, opt => opt.ResolveUsing(dto => dto.GoalNonFulfillmentBranches.Select(x => new GoalNonFulfillmentBranchViewModel()
                {
                    GoalNonFulfillmentBranchId = x.Id,
                    Id = x.BranchId,
                    Name = x.Branch.Name
                })))
                .ReverseMap()
                .ForMember(x => x.GoalNonFulfillmentBranches, opt => opt.ResolveUsing(vm => vm.Branches.Select(x => new GoalNonFulfillmentBranch()
                {
                    BranchId = x.Id,
                    GoalNonFulfillmentPercentId = vm.Id,
                    Id = x.GoalNonFulfillmentBranchId
                })));

        }
    }
}