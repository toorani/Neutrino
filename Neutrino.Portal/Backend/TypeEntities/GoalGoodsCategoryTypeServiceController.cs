using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Portal;

using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/GoalGoodsCategoryTypeService")]
    public class GoalGoodsCategoryTypeServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<GoalGoodsCategoryType> businessService;
        #endregion

        #region [ Constructor(s) ]
        public GoalGoodsCategoryTypeServiceController(IEntityListLoader<GoalGoodsCategoryType> businessService)
        {
            this.businessService = businessService;
        }

        #endregion

        #region [ Public Method(s) ]
        [Route("getValues"), HttpGet]
        public async Task<HttpResponseMessage> GetValuesAsync()
        {
            var entities = await businessService.LoadListAsync();
            if (entities.ReturnStatus == false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, entities.ReturnMessage);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<GoalGoodsCategoryType>, List<TypeEntityViewModel>>(entities.ResultValue);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TypeEntityMapperProfile());
            });
            return config.CreateMapper();
        }
        #endregion


    }

}