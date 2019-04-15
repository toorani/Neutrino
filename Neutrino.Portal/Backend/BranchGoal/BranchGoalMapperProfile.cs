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
    public class BranchGoalMapperProfile : Profile
    {
        public BranchGoalMapperProfile()
        {
            CreateMap<BranchGoalIndex, BranchBenefitGoalViewModel>()
                 .ReverseMap();

            CreateMap<BranchGoalItemViewModel, BranchGoalItem>()
                .ReverseMap();

            CreateMap<BranchGoalViewModel, BranchGoalDTO>()
                .ReverseMap();
        }
    }
}