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
        #endregion

        #region [ Constructor(s) ]
        public BranchServiceController(IEntityListLoader<Branch> branchListLoader)
        {
            this.branchListLoader = branchListLoader;
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