using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class BranchReceiptGoalPercentMapperProfile : Profile
    {
        public BranchReceiptGoalPercentMapperProfile()
        {
            CreateMap<Goal, BranchReceiptGoalPercentViewModel>()
                .ForMember(x => x.GoalDate, entity => entity.ResolveUsing(gol =>
                {
                    return Utilities.PersianMonthNames().FirstOrDefault(x => x.Key == gol.Month).Value + " - " + gol.Year;
                }))
                .ForMember(x => x.GoalGoodsCategoryName, entity => entity.ResolveUsing(gol => gol.GoalGoodsCategory.Name))
                .ForMember(x => x.GoalGoodsCategoryTypeId, entity => entity.ResolveUsing(gol => gol.GoalGoodsCategory.GoalGoodsCategoryTypeId))
                .ForMember(x => x.GoalId, entity => entity.ResolveUsing(gol => gol.Id))
                .ForMember(x => x.IsGoalUsed, entity => entity.ResolveUsing(gol => gol.IsUsed))
                .ForMember(x => x.GoalAmount, entity => entity.ResolveUsing(gol =>
                {
                    if (gol.GoalSteps.Count != 0)
                        return gol.GoalSteps.FirstOrDefault().ComputingValue;
                    return 0;
                }))
                .ForMember(x => x.Items, entity => entity.ResolveUsing(gol =>
                  {
                      var result = gol.BranchReceiptGoalPercent
                      .GroupBy(x => new { x.NotReachedPercent, x.ReachedPercent })
                      .Select(x => new BranchReceiptGoalPercentItemViewModel()
                      {
                          NotReachedPercent = x.Key.NotReachedPercent,
                          ReachedPercent = x.Key.ReachedPercent,
                          GoalId = gol.Id,
                          Branches = x.Where(br => br.BranchId != 0).Select(br => br.Branch.Id).ToList()
                      });
                      return result;
                  }));

            CreateMap<BranchReceiptGoalPercentItemViewModel, BranchReceiptGoalPercentDTO>()
                .ReverseMap();

        }

        class BranchReceiptGoalPercentItemConvertor : ITypeConverter<BranchReceiptGoalPercentItemViewModel, List<BranchReceiptGoalPercent>>
        {
            public List<BranchReceiptGoalPercent> Convert(BranchReceiptGoalPercentItemViewModel source, List<BranchReceiptGoalPercent> destination, ResolutionContext context)
            {
                var result = new List<BranchReceiptGoalPercent>();
                source.Branches.ForEach(x =>
                {
                    result.Add(new BranchReceiptGoalPercent
                    {
                        BranchId = x,
                        GoalId = source.GoalId,
                        NotReachedPercent = source.NotReachedPercent,
                        ReachedPercent = source.ReachedPercent
                    });
                });
                return result;

            }
        }
    }
}