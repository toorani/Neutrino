using AutoMapper;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class GoodsMapperProfile : Profile
    {
        public GoodsMapperProfile()
        {
            CreateMap<Goods, GoodsViewModel>()
               .ForMember(viewModel => viewModel.CompanyName
               , opt => opt.ResolveUsing(goalDTO => goalDTO.Company == null ? string.Empty : goalDTO.Company.FaName));
            //.Ignore(viewModel => viewModel.FaName)
            //.Ignore(viewModel => viewModel.GoodsCode)
            //.Ignore(viewModel => viewModel.transactionalData);
        }
    }
}