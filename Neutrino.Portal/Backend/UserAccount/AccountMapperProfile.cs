using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Espresso.Core;
using Espresso.Identity.Models;

using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class AccountMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public AccountMapperProfile()
        {
            CreateMap<RegisterViewModel, NeutrinoUser>()
                .ForMember(x => x.UserRoles, opt => opt.ResolveUsing<List<UserRole>>((vm) =>
                {
                    List<UserRole> userRoles = new List<UserRole>();
                    vm.Roles.ForEach(x => userRoles.Add(new UserRole { RoleId = x.Id, UserId = vm.Id }));
                    return userRoles;
                }))
                .ReverseMap()
                .ConstructUsing(x => new RegisterViewModel())
                .ForMember(x => x.Roles, opt => opt.ResolveUsing<List<RoleViewModel>>((dto) =>
                {
                    List<RoleViewModel> roles = new List<RoleViewModel>();
                    dto.UserRoles
                    .ToList()
                    .ForEach(x =>
                    {
                        NeutrinoRole neutrinoRole = x.Role as NeutrinoRole;
                        roles.Add(new RoleViewModel()
                        {
                            Id = neutrinoRole.Id,
                            FaName = neutrinoRole.FaName != null ? neutrinoRole.FaName : neutrinoRole.Name,
                            Name = neutrinoRole.Name != null ? neutrinoRole.Name : neutrinoRole.FaName
                        });
                    });
                    return roles;
                }));

            CreateMap<RegisterViewModel, ApplicationUser>()
                .ReverseMap()
                .ConstructUsing(x => new RegisterViewModel());

            CreateMap<NeutrinoRole, RoleViewModel>()
                .ConstructUsing(x => new RoleViewModel())
                .ForMember(x => x.FaName, opt => opt.ResolveUsing(x => x.FaName == null ? x.Name : x.FaName))
                .ReverseMap();

            CreateMap<DataList<NeutrinoUser>, DataList<RegisterViewModel>>();
            CreateMap<DataList<NeutrinoRole>, DataList<RoleViewModel>>();
            //CreateMap<DataList<Role>, DataList<NeutrinoRoleViewModel>>();

        }
        #endregion

      
    }


}