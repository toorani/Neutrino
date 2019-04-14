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
    public class QuantityConditionMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public QuantityConditionMapperProfile()
        {
            CreateMap<QuantityCondition, QuantityConditionViewModel>()
                .ForMember(x => x.GoalCategoryName, opt => opt.MapFrom(x => x.Goal.GoalGoodsCategory.Name))
                .ReverseMap();

            CreateMap<GoodsQuantityCondition, GoodsQuantityConditionVModel>()
                .ForMember(vm => vm.EnName, optgds => optgds.MapFrom(x => x.Goods.EnName))
                .ForMember(vm => vm.FaName, optgds => optgds.MapFrom(x => x.Goods.FaName))
                .ForMember(vm => vm.Code, optgds => optgds.MapFrom(x => x.Goods.GoodsCode))
                //.ForMember(vm => vm.Quantity, opt => opt.ResolveUsing(src =>
                //{
                //    int? result = null;
                //    if (src.Quantity != 0)
                //        result = src.Quantity;
                //    return result;
                //}))
                .ReverseMap()
                .Ignore(x => x.Goods);

            CreateMap<BranchQuantityCondition, BranchQuantityConditionVModel>()
                .ForMember(x => x.BranchName, opt => opt.MapFrom(x => x.Branch.Name))
                .ForMember(vm => vm.GoodsId, optgds => optgds.MapFrom(x => x.GoodsQuantityCondition.GoodsId))
                //.ForMember(vm => vm.Quantity, opt => opt.ResolveUsing(src =>
                //{
                //    int? result = null;
                //    if (src.Quantity != 0)
                //        result = src.Quantity;
                //    return result;
                //}))
                .ReverseMap()
                .Ignore(x => x.GoodsQuantityCondition)
                .Ignore(x => x.Branch);


        }
        #endregion
    }
}