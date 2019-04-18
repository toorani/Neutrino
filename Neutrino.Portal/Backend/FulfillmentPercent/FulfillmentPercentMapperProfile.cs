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
    public class FulfillmentPercentMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public FulfillmentPercentMapperProfile()
        {
            CreateMap<FulfillmentPercent, FulfillmentPercentViewModel>()
            .ReverseMap();
            
            
        }
        #endregion


    }
}