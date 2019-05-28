using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/branchService")]
    public class BranchServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<Branch> branchListLoader;
        private readonly IEntityLoader<Branch> branchLoader;
        #endregion

        #region [ Constructor(s) ]
        public BranchServiceController(IEntityListLoader<Branch> branchListLoader
            , IEntityLoader<Branch> branchLoader)
        {
            this.branchListLoader = branchListLoader;
            this.branchLoader = branchLoader;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getBranches")]
        public async Task<HttpResponseMessage> GetBranches()
        {
            var entities = await branchListLoader.LoadAllAsync(
               orderBy: Utilities.GetOrderBy<Branch>("Name", "asc"));

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            var result = mapper.Map<List<Branch>, List<BranchViewModel>>(entities.ResultValue);

            return CreateSuccessedListResponse(result);
        }

        [Route("getBranchInfo")]
        public async Task<HttpResponseMessage> GetBranchInfo(int branchId)
        {
            var entities = await branchLoader.LoadAsync(x => x.Id == branchId);

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            var result = mapper.Map<Branch, BranchViewModel>(entities.ResultValue);

            return CreateViewModelResponse(result, entities);
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BranchMapperProfile());

            });
            return config.CreateMapper();
        }
        #endregion

    }

}