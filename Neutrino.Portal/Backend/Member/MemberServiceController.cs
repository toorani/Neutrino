using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/memberService")]
    public class MemberServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<Member> businessService;
        #endregion

        #region [ Constructor(s) ]
        public MemberServiceController(IEntityListLoader<Member> memberLoader)
        {
            businessService = memberLoader;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getBranchMembers")]
        public async Task<HttpResponseMessage> GetBranchMembers()
        {
            int branchId = 2397;
            //TODO : apply access
            //var claimPrincipal = User as ClaimsPrincipal;
            //if (claimPrincipal.HasClaim(x=>x.Type == ApplicationClaimTypes.BranchId))
            //{
            //    branchId = Convert.ToInt32(claimPrincipal.FindFirst(x => x.Type == ApplicationClaimTypes.BranchId).Value);
            //}

            var result_loadData = await businessService.LoadListAsync(x=>x.BranchId == branchId);
            if (result_loadData.ReturnStatus == false)
            {
                return CreateErrorResponse(result_loadData);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<Member>, List<MemberViewModel>>(result_loadData.ResultValue);
            return CreateSuccessedListResponse(result);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MemberMapperProfile());
            });
            return config.CreateMapper();
        }

        #endregion

    }

}