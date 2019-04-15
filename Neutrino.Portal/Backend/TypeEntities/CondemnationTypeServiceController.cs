using System.Collections.Generic;
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
    [RoutePrefix("api/CondemnationTypeService")]
    public class CondemnationTypeServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<CondemnationType> businessService;
        #endregion

        #region [ Constructor(s) ]
        public CondemnationTypeServiceController(IEntityListLoader<CondemnationType> businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getValues"), HttpGet]
        public async Task<HttpResponseMessage> GetValuesAsync()
        {
            IBusinessResultValue<List<CondemnationType>> entities = await businessService.LoadListAsync();
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<CondemnationType>, List<TypeEntityViewModel>>(entities.ResultValue);
            return CreateSuccessedListResponse<TypeEntityViewModel>(result);
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