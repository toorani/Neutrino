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
using System.Linq;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/positionTypeService")]
    public class PositionTypeServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<PositionType> businessService;
        #endregion

        #region [ Constructor(s) ]
        public PositionTypeServiceController(IEntityListLoader<PositionType> businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getValues"), HttpGet]
        public async Task<HttpResponseMessage> GetValuesAsync()
        {
            IBusinessResultValue<List<PositionType>> entities = await businessService.LoadListAsync(orderBy: ord => ord.OrderBy(x => x.Description));
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<PositionType>, List<TypeEntityViewModel>>(entities.ResultValue);
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