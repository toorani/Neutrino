using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using Espresso.Portal;

using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using Ninject;
using Ninject.Extensions.Logging;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/GoalGoodsCategoryService")]

    public class GoalGoodsCategoryServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IGoalGoodsCategoryBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public GoalGoodsCategoryServiceController(IGoalGoodsCategoryBS businessService)
        {
            this.businessService = businessService;

        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataList"), HttpGet]
        public async Task<HttpResponseMessage> GetDataList(int goodsCategoryTypeId, bool? isActive = null, int? iGoalTypeId = null)
        {
            GoalGoodsCategoryTypeEnum catTypeId = default(GoalGoodsCategoryTypeEnum);
            GoalTypeEnum goalTypeId = default(GoalTypeEnum);
            if (goodsCategoryTypeId != 0)
            {
                catTypeId = Utilities.ToEnum<GoalGoodsCategoryTypeEnum>(goodsCategoryTypeId).Value;
            }
            if (iGoalTypeId.HasValue)
            {
                goalTypeId = Utilities.ToEnum<GoalTypeEnum>(iGoalTypeId).Value;
            }
            IBusinessResultValue<List<GoalGoodsCategory>> entities = await businessService.LoadVisibleGoalGoodsCategoryListAsync(
                where: x => (x.GoalGoodsCategoryTypeId == catTypeId || goodsCategoryTypeId == 0)
                && (x.GoalTypeId == goalTypeId || iGoalTypeId.HasValue == false)
                && (x.IsActive == isActive || isActive.HasValue == false));

            if (entities.ReturnStatus == false)
            {
                CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            var dataSource = mapper.Map<List<GoalGoodsCategoryViewModel>>(entities.ResultValue);

            return CreateSuccessedListResponse(dataSource);
        }
        [Route("getDataItem"), HttpGet]
        public async Task<HttpResponseMessage> GetDataItem(int id)
        {
            var entity = await businessService.LoadGoalGoodsCategoryAsync(id);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<GoalGoodsCategoryViewModel>(entity.ResultValue);

            return CreateViewModelResponse(dataModelView, entity);
        }
        [Route("add"), HttpPost]
        public virtual async Task<HttpResponseMessage> Add(GoalGoodsCategoryViewModel postedViewModel)
        {
            var mapper = GetMapper();
            GoalGoodsCategory entityCreating = mapper.Map<GoalGoodsCategory>(postedViewModel);
            IBusinessResultValue<GoalGoodsCategory> entityCreated = await businessService.CreateGoalGoodsCategoryAsync(entityCreating, postedViewModel.GoalCategorySimilarId);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }

            postedViewModel = mapper.Map<GoalGoodsCategoryViewModel>(entityCreated.ResultValue);
            return CreateViewModelResponse(postedViewModel, entityCreated);

        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GoalGoodsCategoryMapperProfile());
                cfg.AddProfile(new GoodsMapperProfile());
                cfg.AddProfile(new CompanyMapperProfile());
            });
            return config.CreateMapper();
        }
        #endregion

    }
}
