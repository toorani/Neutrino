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
    public class OrgStructureShareMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public OrgStructureShareMapperProfile()
        {
            CreateMap<OrgStructureShareDTO, OrgStructureShareDTOViewModel>()
                .ReverseMap();


            CreateMap<OrgStructureShare, OrgStructureShareViewModel>()
                .ReverseMap()
                .Ignore(x => x.OrgStructure);
                
                
        }

        private List<OrgStructureShare> setBranch(OrgStructureShareDTOViewModel personelShareDTO)
        {
            List<OrgStructureShare> result = new List<OrgStructureShare>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrgStructureShare, OrgStructureShareViewModel>()
                .ReverseMap();
                cfg.AddProfile(new BranchMapperProfile());
                cfg.AddProfile(new OrgStructureMapperProfile());
            });
            var mapper = config.CreateMapper();
            personelShareDTO.Items.ForEach((Action<OrgStructureShareViewModel>)(x =>
            {
                var personelShare = mapper.Map<OrgStructureShareViewModel, OrgStructureShare>(x);
                personelShare.BranchId = personelShareDTO.Branch.Id;
                personelShare.OrgStructure.Branch = null;
                if (personelShare.PrivateReceiptPercent.HasValue || personelShare.SalesPercent.HasValue
                || personelShare.TotalReceiptPercent.HasValue)
                    result.Add(personelShare);
            }));
            return result;
        }
        #endregion
    }
}