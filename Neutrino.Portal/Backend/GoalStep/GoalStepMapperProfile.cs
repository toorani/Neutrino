using System;
using AutoMapper;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class GoalStepMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public GoalStepMapperProfile()
        {
            CreateMap<GoalStepViewModel, GoalStep>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .AfterMap((vm, dto) =>
                {
                    var itemCfg = new MapperConfiguration(icfg =>
                    {
                        icfg.CreateMap<GoalStepItemInfoViewModel, GoalStepItemInfo>()
                        .ForMember(s => s.Amount, opt => opt.ResolveUsing(ds => Convert.ToDecimal(ds.Amount)))
                        .ForMember(s => s.GoalStepId, opt => opt.Ignore())
                        .ForMember(s => s.ActionTypeId, opt => opt.Ignore())
                        .ForMember(s => s.ItemTypeId, opt => opt.Ignore());
                    });

                    var iMapper = itemCfg.CreateMapper();

                    GoalStepItemInfo goalStepItemInfo = new GoalStepItemInfo();

                    if (vm.RewardInfo != null)
                    {
                        goalStepItemInfo = iMapper.Map<GoalStepItemInfoViewModel, GoalStepItemInfo>(vm.RewardInfo);
                        goalStepItemInfo.ActionTypeId = GoalStepActionTypeEnum.Reward;
                        goalStepItemInfo.ItemTypeId = vm.RewardTypeId.Value;
                        goalStepItemInfo.GoalStepId = vm.Id;
                        dto.Items.Add(goalStepItemInfo);
                    }
                    if (vm.CondemnationTypeId.HasValue)
                    {
                        goalStepItemInfo = iMapper.Map<GoalStepItemInfoViewModel, GoalStepItemInfo>(vm.CondemnationInfo);
                        goalStepItemInfo.ActionTypeId = GoalStepActionTypeEnum.Condemnation;
                        goalStepItemInfo.ItemTypeId = vm.CondemnationTypeId.Value;
                        goalStepItemInfo.GoalStepId = vm.Id;
                        dto.Items.Add(goalStepItemInfo);
                    }
                });

            CreateMap<GoalStep, GoalStepViewModel>()
               .AfterMap((dto, vm) =>
               {
                   var itemCfg = new MapperConfiguration(icfg =>
                   {
                       icfg.CreateMap<GoalStepItemInfo, GoalStepItemInfoViewModel>();
                   });

                   var iMapper = itemCfg.CreateMapper();


                   foreach (var item in dto.Items)
                   {
                       if (item.ActionTypeId == GoalStepActionTypeEnum.Reward)
                       {
                           vm.RewardInfo = iMapper.Map<GoalStepItemInfo, GoalStepItemInfoViewModel>(item);
                           vm.RewardTypeId = item.ItemTypeId;
                       }
                       else
                       {
                           vm.CondemnationInfo = iMapper.Map<GoalStepItemInfo, GoalStepItemInfoViewModel>(item);
                           vm.CondemnationTypeId = item.ItemTypeId;
                       }
                   }
               });


        }
        #endregion
    }
}