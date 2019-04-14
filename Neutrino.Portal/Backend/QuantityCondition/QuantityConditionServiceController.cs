using System.Net;
using System.Net.Http;
using System.Web.Http;
using Espresso.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Interfaces;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Portal;
using AutoMapper;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/quantityConditionService")]
    public class QuantityConditionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private IQuantityConditionBS quantityConditionBusiness;
        #endregion

        #region [ Constructor(s) ]
        public QuantityConditionServiceController(IQuantityConditionBS bizQuantityCondition)
        {
            this.quantityConditionBusiness = bizQuantityCondition;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getQuantityCondition"), HttpGet]
        public async Task<HttpResponseMessage> GetQuantityConditionAsync(int goalId)
        {
            IBusinessResultValue<QuantityCondition> loadResultValue = await quantityConditionBusiness.LoadQuantityConditionAsync(goalId);
            if (!loadResultValue.ReturnStatus)
            {
                return CreateErrorResponse(loadResultValue);
            }

            QuantityConditionViewModel result = new QuantityConditionViewModel();
            var mapper = GetMapper();

            result = mapper.Map<QuantityConditionViewModel>(loadResultValue.ResultValue);
            return CreateViewModelResponse(result, loadResultValue);
        }

        [Route("getQuantityConditionType"), HttpGet]
        public async Task<HttpResponseMessage> GetQuantityConditionTypeAsync(int goalId)
        {
            IBusinessResultValue<QuantityConditionTypeEnum?> loadResultValue = await quantityConditionBusiness.LoadQuantityConditionTypeAsync(goalId);
            if (!loadResultValue.ReturnStatus)
            {
                return CreateErrorResponse(loadResultValue);
            }
            return Request.CreateResponse(HttpStatusCode.OK, loadResultValue.ResultValue);
        }


        [Route("addOrUpdate"), HttpPost]
        public async Task<HttpResponseMessage> AddOrUpdateAsync(QuantityConditionViewModel postedData)
        {
            var mapper = GetMapper();
            QuantityCondition quantityCondition = mapper.Map<QuantityCondition>(postedData);

            var resultValue = await quantityConditionBusiness.AddOrUpdateQuantityConditionAsync(quantityCondition);
            if (!resultValue.ReturnStatus)
            {
                return CreateErrorResponse(resultValue);
            }

            return CreateViewModelResponse(postedData, resultValue);
        }
        #endregion

        #region [ Protected Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new QuantityConditionMapperProfile());

            });
            return config.CreateMapper();
        }
        #endregion

    }
}
