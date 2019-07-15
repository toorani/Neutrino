using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/penaltyService")]
    public class PenaltyServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IMemberPenaltyBS memberPenaltyBS;
        #endregion

        #region [ Constructor(s) ]
        public PenaltyServiceController(IMemberPenaltyBS memberPenaltyBS)
        {
            this.memberPenaltyBS = memberPenaltyBS;
        }
        #endregion

        #region [ Public Method(s) ]

        [Route("getPenaltiesForPromotion")]
        public async Task<HttpResponseMessage> GetPenaltiesForPromotion(int branchId)
        {
            var result_biz = await memberPenaltyBS.LoadPenaltiesForPromotionAsync(branchId);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            var mapper = GetMapper();
            var viewModelMapped = mapper.Map<List<MemberPenaltyDTO>, List<PenaltyViewModel>>(result_biz.ResultValue);
            return CreateSuccessedListResponse(viewModelMapped);
        }

        [Route("addOrModify"),HttpPost]
        public async Task<HttpResponseMessage> AddOrModify(List<PenaltyViewModel> postedViewModel)
        {
            var mapper = GetMapper();
            var entities = mapper.Map<List<PenaltyViewModel>, List<MemberPenalty>>(postedViewModel);

            var result_biz = await memberPenaltyBS.CreateOrModifyAsync(entities);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            result_biz.ResultValue.ForEach(x =>
            {
                var penalty = postedViewModel.FirstOrDefault(y => y.MemberId == x.MemberId);
                penalty.Id = x.Id;
            });
            return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = postedViewModel, returnMessage = result_biz.ReturnMessage.ConcatAll() });
        }

        [Route("releaseCEOPromotion"), HttpPost]
        public async Task<HttpResponseMessage> ReleaseCEOPromotion(List<PenaltyViewModel> postedViewModel)
        {
            var mapper = GetMapper();
            var entities = mapper.Map<List<PenaltyViewModel>, List<MemberPenalty>>(postedViewModel);

            var result_biz = await memberPenaltyBS.ReleaseCEOPromotion(entities);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = (int)result_biz.ResultValue , returnMessage = result_biz.ReturnMessage.ConcatAll() });
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PenaltyMapperProfile());
            });
            return config.CreateMapper();
        }
        #endregion

    }

}