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
    [RoutePrefix("api/goalNonFulfillmentService")]
    public class GoalNonFulfillmentServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IGoalNonFulfillmentPercentBS businessService;
        private readonly IEntityLoader<Goal> goalLoader;
        #endregion

        #region [ Constructor(s) ]
        public GoalNonFulfillmentServiceController(IGoalNonFulfillmentPercentBS businessService
            , IEntityLoader<Goal> goalListLoader)
        {
            this.businessService = businessService;
            goalLoader = goalListLoader;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataInfo")]
        public async Task<HttpResponseMessage> GetDataInfo(int goalId)
        {
            var entity = await businessService.LoadGoalAsync(goalId);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<GoalNonFulfillmentCollectionViewModel>(entity.ResultValue);

            return CreateViewModelResponse(dataModelView, entity);

        }

        [Route("addOrUdate"), HttpPost]
        public async Task<HttpResponseMessage> AddOrUpdate(GoalNonFulfillmentViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityCreating = mapper.Map<GoalNonFulfillmentPercent>(postedViewModel);
            var entityCreated = await businessService.CreateOrUpdateAsync(entityCreating);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }
            //postedViewModel = mapper.Map<BranchReceiptGoalPercentItemViewModel>(entityCreated.ResultValue);
            return CreateViewModelResponse(postedViewModel, entityCreated);
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GoalNonFulfillmentMapperProfile());
                cfg.AddProfile(new BranchMapperProfile());

            });
            return config.CreateMapper();
        }


        #endregion


    }

}