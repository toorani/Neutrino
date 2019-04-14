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
    [RoutePrefix("api/RewardTypeService")]
    public class RewardTypeServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<RewardType> businessService;
        #endregion

        #region [ Constructor(s) ]
        public RewardTypeServiceController(IEntityListLoader<RewardType> businessService)
        {
            this.businessService = businessService;
        }
        
        #endregion


        [Route("getValues"), HttpGet]
        public async Task<HttpResponseMessage> GetValuesAsync()
        {
            IBusinessResultValue<List<RewardType>> entities = await businessService.LoadListAsync();
            if (entities.ReturnStatus == false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, entities.ReturnMessage);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<RewardType>, List<TypeEntityViewModel>>(entities.ResultValue);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        protected IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TypeEntityMapperProfile());
            });
            return config.CreateMapper();
        }

    }

}