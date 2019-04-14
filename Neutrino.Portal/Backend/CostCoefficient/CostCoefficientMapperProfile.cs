using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal
{
    public class CostCoefficientMapperProfile : Profile
    {
        public CostCoefficientMapperProfile()
        {
            CreateMap<CostCoefficient, CostCoefficientViewModel>()
                .ForMember(x => x.GoodsCategoryName, opt => opt.ResolveUsing(x => x.GoodsCategoryType.Name))
                .ForMember(x => x.GoodsCategoryId, opt => opt.ResolveUsing(x => x.GoodsCategoryTypeId))
                .ReverseMap()
                .ForMember(x => x.Records, opt => opt.ResolveUsing(x => GetRecords(x.Records)));
        }

        private List<CostCoefficient> GetRecords(List<CostCoefficientViewModel> records)
        {
            List<CostCoefficient> result = new List<CostCoefficient>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CostCoefficientViewModel, CostCoefficient>()
                .Ignore(x => x.Records);
                cfg.AddProfile(new TypeEntityMapperProfile());
            });
            var mapper = config.CreateMapper();
            records.ForEach(x =>
            {
                if (x.Coefficient.HasValue)
                {
                    CostCoefficient entity = mapper.Map<CostCoefficientViewModel, CostCoefficient>(x);
                    entity.GoodsCategoryType = null;
                    result.Add(entity);
                }
            });
            return result;
        }
    }
}