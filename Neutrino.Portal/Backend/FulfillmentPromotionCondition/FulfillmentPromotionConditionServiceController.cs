using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Portal;
using jQuery.DataTables.WebApi;

using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/fulfillmentPromotionConditionService")]
    public class FulfillmentPromotionConditionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IFulfillmentPromotionConditionBS businessService;
        private readonly IEntityEraser<FulfillmentPromotionCondition> totalFulfillPromotionEraser;
        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPromotionConditionServiceController(IFulfillmentPromotionConditionBS businessService
            ,IEntityEraser<FulfillmentPromotionCondition> totalFulfillPromotionEraser)
        {
            this.businessService = businessService;
            this.totalFulfillPromotionEraser = totalFulfillPromotionEraser;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getFulfillPromotions")]
        public async Task<HttpResponseMessage> GetFulfillPromotions()
        {
            var entities = await businessService.EntityListLoader.LoadListAsync();
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            var result = mapper.Map<List<FulfillmentPromotionCondition>, List<FulfillmentPromotionConditionViewModel>>(entities.ResultValue);
            return CreateSuccessedListResponse(result);
        }
        [Route("submitData"), HttpPost]
        public async Task<HttpResponseMessage> SubmitData(List<FulfillmentPromotionConditionViewModel> lstGeneralRange)
        {
            var mapper = GetMapper();
            var entityCreating = mapper.Map<List<FulfillmentPromotionConditionViewModel>, List<FulfillmentPromotionCondition>>(lstGeneralRange);
            var result = await businessService.SaveAsync(entityCreating);

            if (result.ReturnStatus == false)
            {
                return CreateErrorResponse(result);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(FulfillmentPromotionConditionViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityDeleteing = mapper.Map<FulfillmentPromotionCondition>(postedViewModel);
            var result = await totalFulfillPromotionEraser.DeleteAsync(entityDeleteing);

            if (result.ReturnStatus == false)
            {
                return CreateErrorResponse(result);
            }

            return CreateViewModelResponse(postedViewModel, result);
        }
        
        #endregion

        #region [ Private Method(s) ]

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new FulfillmentPromotionConditionMapperProfile());
                });
            return config.CreateMapper();
        }
        #endregion

    }
}
