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
    [RoutePrefix("api/branchReceiptGoalPercentService")]
    public class BranchReceiptGoalPercentServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IBranchReceiptGoalPercentBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public BranchReceiptGoalPercentServiceController(IBranchReceiptGoalPercentBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getGoalPercent")]
        public async Task<HttpResponseMessage> GetGoalPercent(int goalId)
        {
            var entity = await businessService.LoadReceiptGoalAsync(goalId);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }
            var mapper = GetMapper();
            var dataModelView = mapper.Map<BranchReceiptGoalPercentViewModel>(entity.ResultValue);
            return CreateViewModelResponse(dataModelView, entity);
        }

        [Route("addOrUdate"), HttpPost]
        public async Task<HttpResponseMessage> AddOrUpdate(BranchReceiptGoalPercentItemViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityCreating = mapper.Map<BranchReceiptGoalPercentDTO> (postedViewModel);
            var entityCreated = await businessService.CreateOrUpdateAsync(entityCreating);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }
            postedViewModel = mapper.Map<BranchReceiptGoalPercentItemViewModel>(entityCreated.ResultValue);
            return CreateViewModelResponse(postedViewModel, entityCreated);
        }

        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(BranchReceiptGoalPercentItemViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityDeleteing = mapper.Map<BranchReceiptGoalPercentDTO>(postedViewModel);
            var result = await businessService.DeleteAsync(entityDeleteing);

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
                cfg.AddProfile(new BranchReceiptGoalPercentMapperProfile());
            });
            return config.CreateMapper();
        }


        #endregion


    }

}