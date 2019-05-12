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
    public class PermissionMapperProfile : Profile
    {
        public PermissionMapperProfile()
        {
            CreateMap<PermissionViewModel, Permission>();
            CreateMap<List<Permission>, PermissionViewModel>()
                .ConvertUsing(dto =>
                {
                    var vm = new PermissionViewModel();
                    if (dto.Count != 0)
                    {
                        var permission = dto.First();
                        if (permission.ApplicationAction != null)
                            vm.Urls.AddRange(dto.Select(x => x.ApplicationAction.HtmlUrl));
                    }
                    return vm;
                });

        }


    }
}