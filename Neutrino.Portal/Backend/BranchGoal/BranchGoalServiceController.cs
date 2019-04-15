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
    [RoutePrefix("api/branchGoalService")]
    public class BranchGoalServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IBranchGoalBS businessService;
        private readonly IEntityListLoader<Goods> goodsListLoader;
        #endregion

        #region [ Constructor(s) ]
        public BranchGoalServiceController(IBranchGoalBS businessService
            , IEntityListLoader<Goods> goodsListLoader)
        {
            this.businessService = businessService;
            this.goodsListLoader = goodsListLoader;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getbranchGoalDTO")]
        public async Task<HttpResponseMessage> GetBranchGoalDTOAsync(int goalId)
        {
            var entities = await businessService.LoadBranchGoalDTOAsync(goalId);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            var result = mapper.Map<BranchGoalDTO, BranchGoalViewModel>(entities.ResultValue);
            return CreateViewModelResponse(result,entities);
        }
        [Route("batchUpdate"), HttpPost]
        public async Task<HttpResponseMessage> BatchUpdate(BranchGoalViewModel postedViewModel)
        {
            var mapper = GetMapper();
            BranchGoalDTO batchData = mapper.Map<BranchGoalViewModel, BranchGoalDTO>(postedViewModel);
            var businessAction = await businessService.BatchUpdateAsync(batchData);
            
            if (!businessAction.ReturnStatus)
            {
                return CreateErrorResponse(businessAction);
            }
            postedViewModel.Items.Where(x => (x.Percent != null && x.Percent != 0) || x.Amount != null)
                .ToList()
                .ForEach(vm =>
                {
                    vm.BranchGoalId = businessAction.ResultValue.Items.FirstOrDefault(x => x.BranchId == vm.BranchId).BranchGoalId.Value;
                });
            return Request.CreateResponse(HttpStatusCode.OK, businessAction);
        }
      
        #endregion
        
        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BranchGoalMapperProfile());
                cfg.AddProfile(new GoalMapperProfile());
                cfg.AddProfile(new GoalStepMapperProfile());
            });
            return config.CreateMapper();
        }


        #endregion


    }

}