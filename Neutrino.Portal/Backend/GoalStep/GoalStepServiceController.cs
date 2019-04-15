using System.Net;
using System.Net.Http;
using System.Web.Http;

using Neutrino.Entities;
using Espresso.Core;
using Neutrino.Portal.Models;
using AutoMapper;
using Neutrino.Portal.ProfileMapper;
using System.Threading.Tasks;
using Espresso.Portal;
using System.Collections.Generic;
using Neutrino.Interfaces;
using jQuery.DataTables.WebApi;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/GoalStepService")]
    public class GoalStepServiceController : ApiControllerBase
    {

        #region [ Varibale(s) ]
        private readonly IGoalStepBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public GoalStepServiceController(IGoalStepBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataList"), HttpPost]
        public async Task<HttpResponseMessage> GetDataList(GoalStepViewModel postedViewModel)
        {
            var entities = await businessService.EntityListLoader.LoadListAsync(where: x => x.GoalId == postedViewModel.GoalId);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<GoalStep>, List<GoalStepViewModel>>(entities.ResultValue);
            return CreateSuccessedListResponse<GoalStepViewModel>(result);
        }

        [Route("add"), HttpPost]
        public async Task<HttpResponseMessage> Add(GoalStepViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityCreating = mapper.Map<GoalStep>(postedViewModel);
            var entityCreated = await businessService.CreateGoalStepAsync(entityCreating);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }
            postedViewModel = mapper.Map<GoalStepViewModel>(entityCreated.ResultValue);
            return CreateViewModelResponse(postedViewModel, entityCreated);
        }

        [Route("edit"), HttpPost]
        public async Task<HttpResponseMessage> Edit(GoalStepViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityUpdating = mapper.Map<GoalStep>(postedViewModel);
            var actionResult = await businessService.UpdateGoalStepAsync(entityUpdating);

            if (actionResult.ReturnStatus == false)
            {
                return CreateErrorResponse(actionResult);
            }

            return CreateViewModelResponse(postedViewModel, actionResult);
        }

        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(GoalStepViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityDeleteing = mapper.Map<GoalStep>(postedViewModel);
            var result = await businessService.DeleteGoalStepAsync(entityDeleteing);

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
                cfg.AddProfile(new GoalStepMapperProfile());
            });
            return config.CreateMapper();
        }
        #endregion
    }
}
