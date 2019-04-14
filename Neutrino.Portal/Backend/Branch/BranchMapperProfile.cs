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
    public class BranchMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public BranchMapperProfile()
        {
            CreateMap<Branch, BranchViewModel>()
                  .ReverseMap();
        }
        #endregion
    }
}