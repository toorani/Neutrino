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
    public class TypeEntityMapperProfile : Profile
    {

        #region [ Constructor(s) ]
        public TypeEntityMapperProfile()
        {
            CreateMap<CondemnationType, TypeEntityViewModel>()
                .ReverseMap();
            CreateMap<RewardType, TypeEntityViewModel>()
                .ReverseMap();
            CreateMap<OtherRewardType, TypeEntityViewModel>()
                .ReverseMap();
            CreateMap<GoodsCategory, TypeEntityViewModel>()
               .ReverseMap();
            CreateMap<TherapeuticType, TypeEntityViewModel>()
               .ReverseMap();
            CreateMap<GoalGoodsCategoryType, TypeEntityViewModel>()
              .ReverseMap();
            CreateMap<PositionType, TypeEntityViewModel>()
              .ReverseMap();
        }
        #endregion
        
    }
}