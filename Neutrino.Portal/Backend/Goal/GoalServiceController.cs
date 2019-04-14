using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using jQuery.DataTables.WebApi;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/GoalService")]
    public class GoalServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IGoalBS businessService;
        
        #endregion

        #region [ Constructor(s) ]
        public GoalServiceController(IGoalBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getGoalByGoodsCategoryId"), HttpGet]
        public async Task<HttpResponseMessage> GetGoalByGoodsCategoryId(int goodsCategoryId)
        {
            IBusinessResultValue<Goal> goalEntity = await businessService
                .EntityLoader
                .LoadAsync(where: x => x.GoalGoodsCategoryId == goodsCategoryId && x.IsUsed == false
                , includes: x => new { x.GoalSteps });
            if (goalEntity.ReturnStatus == false)
            {
                return CreateErrorResponse(goalEntity);
            }

            var mapper = GetMapper();
            var result = mapper.Map<Goal, GoalViewModel>(goalEntity.ResultValue);
            return CreateViewModelResponse(result, goalEntity);
        }
        [Route("getDataGrid"), HttpPost]
        public virtual async Task<HttpResponseMessage> GetDataGrid(JQueryDataTablesModel dataTablesModel)
        {
            GoalTypeEnum? goalTypeId = GoalTypeEnum.Distributor;
            bool? isUsed = null;
            var lstGoalGoodsCategoryType = new List<GoalGoodsCategoryTypeEnum>();
            string goalTypeParam = "goalType";
            string isUsedParam = "isUsed";
            string goalGoodsCatTypeParam = "goalGoodsCatType";
            if (dataTablesModel.ExternalParam.ContainsKey(goalTypeParam))
            {
                goalTypeId = Utilities.ToEnum<GoalTypeEnum>(dataTablesModel.ExternalParam[goalTypeParam]);
            }

            if (dataTablesModel.ExternalParam.ContainsKey(isUsedParam))
            {
                isUsed = Convert.ToBoolean(dataTablesModel.ExternalParam[isUsedParam]);
            }

            if (dataTablesModel.ExternalParam.ContainsKey(goalGoodsCatTypeParam))
            {

                foreach (var item in dataTablesModel.ExternalParam[goalGoodsCatTypeParam].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    lstGoalGoodsCategoryType.Add(Utilities.ToEnum<GoalGoodsCategoryTypeEnum>(item).Value);
                }

            }


            if (goalTypeId.HasValue)
            {
                var entities = await businessService.EntityListByPagingLoader
                    .LoadAsync(
                    pageNumber: dataTablesModel.iDisplayStart
                    , pageSize: dataTablesModel.iDisplayLength
                    , includes: goal => new { goal.GoalGoodsCategory, goal.Company, goal.GoalGoodsCategoryType, goal.GoalSteps }
                    , orderBy: UIHelper.GetOrderBy<Goal, GoalViewModel>(dataTablesModel.GetSortedColumns())
                    , where: or => (or.GoalGoodsCategory.Name.Contains(dataTablesModel.sSearch)
                    || or.GoalGoodsCategoryType.Description.Contains(dataTablesModel.sSearch)
                    )
                    && or.GoalTypeId == goalTypeId.Value
                    && (isUsed == null || or.IsUsed == isUsed.Value)
                    && (lstGoalGoodsCategoryType.Count == 0 || lstGoalGoodsCategoryType.Contains(or.GoalGoodsCategoryTypeId)));

                if (entities.ReturnStatus == false)
                {
                    return CreateErrorResponse(entities);
                }

                var mapper = GetMapper();
                List<GoalViewModel> dataSource = mapper.Map<List<Goal>, List<GoalViewModel>>(entities.ResultValue);

                return Request.CreateResponse(HttpStatusCode.OK
                    , DataTablesJson(items: dataSource
                    , totalRecords: entities.TotalRows
                    , totalDisplayRecords: entities.TotalRows
                    , sEcho: dataTablesModel.sEcho));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new List<GoalViewModel>());
            }
        }
        [Route("getDataItem")]
        public async Task<HttpResponseMessage> GetDataItem(int id)
        {
            var entity = await businessService.LoadGoalAync(id);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<GoalViewModel>(entity.ResultValue);

            return CreateViewModelResponse(dataModelView, entity);
        }
        [Route("getPreviousAggregationValue")]
        public async Task<HttpResponseMessage> GetPreviousAggregationValue(int month, int year)
        {
            var entity = await businessService.LoadPreviousAggregationValueAync(month, year);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            return Request.CreateResponse(HttpStatusCode.OK, entity.ResultValue);
        }
        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(GoalViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityDeleteing = mapper.Map<GoalViewModel, Goal>(postedViewModel);
            var result = await businessService.DeleteGoalAsync(entityDeleteing);

            if (result.ReturnStatus == false)
            {
                return CreateErrorResponse(result);
            }

            return CreateViewModelResponse(postedViewModel, result);
        }
        [Route("add"), HttpPost]
        public async Task<HttpResponseMessage> Add(GoalViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityCreating = mapper.Map<GoalViewModel, Goal>(postedViewModel);
            var entityCreated = await businessService.CreateGoalAsync(entityCreating);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }
            postedViewModel = mapper.Map<GoalViewModel>(entityCreated.ResultValue);
            return CreateViewModelResponse(postedViewModel, entityCreated);
        }
        [Route("edit"), HttpPost]
        public async Task<HttpResponseMessage> Edit(GoalViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityUpdating = mapper.Map<GoalViewModel, Goal>(postedViewModel);
            var actionResult = await businessService.UpdateGoalAsync(entityUpdating);

            if (actionResult.ReturnStatus == false)
            {
                return CreateErrorResponse(actionResult);
            }

            return CreateViewModelResponse(postedViewModel, actionResult);
        }
        [Route("getGeneralGoalList"), HttpPost]
        public async Task<HttpResponseMessage> GetGeneralGoalDataGrid(JQueryDataTablesModel dataTablesModel)
        {
            var businessResult = await businessService.EntityListLoader.LoadListAsync(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.TotalSalesGoal && x.GoalTypeId == GoalTypeEnum.Distributor
            , orderBy: ord => ord.OrderBy(g => g.StartDate));

            if (businessResult.ReturnStatus == false)
            {
                return CreateErrorResponse(businessResult);
            }

            var mapper = GetMapper();

            var result = mapper.Map<List<Goal>, List<GoalViewModel>>(businessResult.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK
                   , DataTablesJson(items: result
                   , totalRecords: result.Count
                   , totalDisplayRecords: result.Count
                   , sEcho: 0));
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new GoalMapperProfile());
                    cfg.AddProfile(new CompanyMapperProfile());
                    cfg.AddProfile(new FulfillmentPromotionConditionMapperProfile());
                    cfg.AddProfile(new GoalStepMapperProfile());
                });
            return config.CreateMapper();
        }
        #endregion

    }
}
