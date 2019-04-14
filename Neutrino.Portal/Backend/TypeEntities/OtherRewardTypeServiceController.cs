using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using Espresso.Portal;
using jQuery.DataTables.WebApi;

using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using Neutrino.Portal.WebApiControllers;
using Ninject;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/OtherRewardTypeService")]
    public class OtherRewardTypeServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<OtherRewardType> businessService;
        #endregion

        #region [ Constructor(s) ]
        public OtherRewardTypeServiceController(IEntityListLoader<OtherRewardType> businessService)
        {
            this.businessService = businessService;
        }
        #endregion


        [Route("getValues"), HttpGet]
        public async Task<HttpResponseMessage> GetValuesAsync()
        {
            IBusinessResultValue<List<OtherRewardType>> entities = await businessService.LoadListAsync();
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<OtherRewardType>, List<TypeEntityViewModel>>(entities.ResultValue);
            return CreateSuccessedListResponse(result);
        }

        protected  IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TypeEntityMapperProfile());
            });
            return config.CreateMapper();
        }
    }

}