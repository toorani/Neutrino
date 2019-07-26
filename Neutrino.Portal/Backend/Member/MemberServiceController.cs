using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMemberBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public MemberServiceController(IMemberBS memberLoader)
        {
            businessService = memberLoader;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getBranchMembers")]
        public async Task<HttpResponseMessage> GetBranchMembers()
        {
            var branchId = IdentityConfig.GetBranchId(User);
            return await GetMembers(branchId);
        }

        [Route("getMembersByBranchId")]
        public async Task<HttpResponseMessage> GetMembers(int branchId)
        {
            var result_loadData = await businessService.LoadMembersAsync(branchId, null);
            if (result_loadData.ReturnStatus == false)
            {
                return CreateErrorResponse(result_loadData);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<Member>, List<MemberViewModel>>(result_loadData.ResultValue);
            return CreateSuccessedListResponse(result.OrderBy(x => x.FullName).ToList());
        }

        [Route("toggleActivation"), HttpPost]
        public async Task<HttpResponseMessage> ToggleActivation(Member entity)
        {
            IBusinessResult result = await businessService.ToggleActivationAsync(entity.Id);
            if (result.ReturnStatus == false)
            {
                return CreateErrorResponse(result);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
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