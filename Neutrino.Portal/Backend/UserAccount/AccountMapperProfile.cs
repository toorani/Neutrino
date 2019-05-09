using AutoMapper;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using System.Collections.Generic;
using System.Linq;

namespace Neutrino.Portal.ProfileMapper
{
    public class AccountMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public AccountMapperProfile()
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.Roles, opt => opt.ResolveUsing((vm) => new List<UserRole>() { new UserRole { RoleId = vm.RoleId, UserId = vm.Id } }))
                .ForMember(x => x.Claims, opt => opt.ResolveUsing((vm) => vm.BranchesUnderControl.Select(x => new UserClaim { ClaimType = "branch", ClaimValue = x.ToString(), UserId = vm.Id }).ToList()))
                .ReverseMap()
                .ForMember(x => x.RoleId, opt => opt.ResolveUsing((dto) => dto.Roles.FirstOrDefault().RoleId))
                .ForMember(x => x.BranchesUnderControl, opt => opt.ResolveUsing((dto) => dto.Claims.Select(x => x.ClaimValue)));

            CreateMap<User, UserIndexViewModel>()
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(x => x.Name + " " + x.LastName))
                .ForMember(x => x.RoleName, opt => opt.ResolveUsing(x => x.Roles.FirstOrDefault().Role.FaName));


            CreateMap<Role, RoleViewModel>()
                .ConstructUsing(x => new RoleViewModel())
                .ForMember(x => x.FaName, opt => opt.ResolveUsing(x => x.FaName == null ? x.Name : x.FaName))
                .ReverseMap();

        }
        #endregion


    }


}