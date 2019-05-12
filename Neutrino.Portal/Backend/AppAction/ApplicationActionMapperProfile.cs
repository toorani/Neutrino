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
    public class ApplicationActionMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public ApplicationActionMapperProfile()
        {
            CreateMap<ApplicationAction, ApplicationActionViewModel>()
            .ReverseMap();

            CreateMap<UrlActionViewModel, List<ApplicationAction>>()
                .ConvertUsing((vm) => (from urla in vm.Actions
                                       select new ApplicationAction
                                       {
                                           HtmlUrl = vm.HtmlUrl,
                                           ActionUrl = urla
                                       }).ToList());
            CreateMap<List<ApplicationAction>, UrlActionViewModel>()
                .ConvertUsing((dto) =>
                {
                    return dto.GroupBy(x => x.HtmlUrl)
                    .Select(x => new UrlActionViewModel
                    {
                        HtmlUrl = x.Key,
                        Actions = x.Select(y => y.ActionUrl).ToList()
                    }).FirstOrDefault();
                    
                });
        }
        #endregion
    }
}