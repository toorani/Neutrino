using AutoMapper;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class OrgStructureMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public OrgStructureMapperProfile()
        {
            CreateMap<OrgStructure, OrgStructureIndexViewModel>()
                .ReverseMap();
            CreateMap<OrgStructure, OrgStructureViewModel>()
                .ReverseMap();

        }
        #endregion
    }
}