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
    public class CompanyMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public CompanyMapperProfile()
        {
            CreateMap<Company, CompanyViewModel>()
                 .ConstructUsing(vm => new CompanyViewModel())
                 .ForMember(x => x.FaName, opt => opt.MapFrom(x => x.FaName))
                 .ForMember(x => x.CompanyCode, opt => opt.MapFrom(x => x.Code));
            
        }
        #endregion
    }
}