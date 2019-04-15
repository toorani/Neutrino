using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Core.Ninject.Http;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class GoalGoodsCategoryMapperProfile : Profile
    {
        public GoalGoodsCategoryMapperProfile()
        {
            CreateMap<GoalGoodsCategory, GoalGoodsCategoryViewModel>()
                .ForMember(x => x.CompanySelected, opt => opt.ResolveUsing(dto => dto.GoodsCollection.Select(x => x.Goods.CompanyId).Distinct()))
                .ForMember(x => x.GoodsSelected, opt => opt.ResolveUsing(dto => dto.GoodsCollection.Select(x => x.GoodsId)))
                .ForMember(x => x.GoodsCollection, opt => opt.ResolveUsing(dto => dto.GoodsCollection.Select(x => x.Goods)))
                .ReverseMap()
                .ForMember(x => x.GoodsCollection, opt => opt.ResolveUsing(vm => vm.GoodsSelected
                .Select(x => new GoalGoodsCategoryGoods { GoodsId = x })));

        }
    }
}