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
    public class GoalMapperProfile : Profile
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Constructor(s) ]
        public GoalMapperProfile()
        {
            CreateMap<Goal, GoalViewModel>()
            .ForMember(goalVM => goalVM.StartDate,
                opt => opt.ResolveUsing(goalDTO => Utilities.ToPersianDate(goalDTO.StartDate)))
            .ForMember(goalVM => goalVM.EndDate,
                opt => opt.ResolveUsing(goalDTO => Utilities.ToPersianDate(goalDTO.EndDate)))
            .ForMember(x => x.GoodsSelectionList,
                opt => opt.ResolveUsing(x => x.GoalTypeId == GoalTypeEnum.Supplier ? GetGoodsSelectedList(x.GoalGoodsCategory) : new List<GoodsViewModel>()))
                .ForMember(x => x.Amount, opt => opt.ResolveUsing(x =>
                {

                    if (x.GoalSteps.Count != 0)
                    {
                        decimal result = 0;
                        if (x.GoalGoodsCategoryTypeId >= GoalGoodsCategoryTypeEnum.TotalSalesGoal
                        && x.GoalGoodsCategoryTypeId <= GoalGoodsCategoryTypeEnum.AggregationGoal)
                        {
                            result = x.GoalSteps.FirstOrDefault().ComputingValue;
                        }
                        return result;
                    }
                    else
                    {
                        return x.Amount;
                    }
                }))
                //.Ignore(x => x.GoalSteps)
                .ReverseMap()
                .ForMember(goalDTO => goalDTO.StartDate,
                    opt => opt.ResolveUsing(goalVM => Utilities.ToDateTime(goalVM.StartDate)))
                .ForMember(goalDTO => goalDTO.EndDate,
                    opt => opt.ResolveUsing(goalVM => Utilities.ToDateTime(goalVM.EndDate)))
                .Ignore(x => x.GoalSteps)
                .Ignore(x => x.GoalGoodsCategory);

            CreateMap<GoodsViewModel, Goods>()
                .ReverseMap()
                .ConstructUsing(x => new GoodsViewModel());

            CreateMap<GoalGoodsCategoryViewModel, GoalGoodsCategory>()
                .Ignore(x => x.GoodsCollection)
                .ReverseMap()
                .Ignore(x => x.GoodsCollection)
                .Ignore(x => x.GoodsSelected);

            CreateMap<TypeEntityViewModel, GoalGoodsCategoryType>()
                .ReverseMap()
                .ConstructUsing(x => new TypeEntityViewModel());

        }
        #endregion

        #region [ Private Method(s) ]
        private List<GoodsViewModel> GetGoodsSelectedList(GoalGoodsCategory entity)
        {
            if (entity != null)
            {
                //TODO : will be back it
                //var config = new MapperConfiguration(cfg => { cfg.AddProfile(new GoalGoodsCategoryMapperProfile()); });
                //var mapper = config.CreateMapper();
                //GoalGoodsCategoryViewModel viewModel = mapper.Map<GoalGoodsCategoryViewModel>(entity);
                //return viewModel.GoodsCollection;
            }
            return new List<GoodsViewModel>();
        }
        private GoalStepItemInfoViewModel getStopItemInfo(ICollection<GoalStepItemInfo> items, GoalStepActionTypeEnum actionTypeId)
        {
            GoalStepItemInfo goalStepItemInfo = items.FirstOrDefault(x => x.ActionTypeId == actionTypeId);
            return getStepItemInfoViewModel(goalStepItemInfo);

        }
        private GoalStepItemInfoViewModel getStepItemInfoViewModel(GoalStepItemInfo goalStepItemInfo)
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<GoalStepItemInfo, GoalStepItemInfoViewModel>()
                   .ConstructUsing(vm => new GoalStepItemInfoViewModel());
                });
            var mapper = config.CreateMapper();
            return mapper.Map<GoalStepItemInfo, GoalStepItemInfoViewModel>(goalStepItemInfo);
        }
        #endregion
    }
}