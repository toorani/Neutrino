using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Portal;
using jQuery.DataTables.WebApi;
using Neutrino.Business;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal
{
    [RoutePrefix("api/memberSharePromotionService")]
    public class MemberSharePromotionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IMemberSharePromotionBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public MemberSharePromotionServiceController(IMemberSharePromotionBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("addOrModify"), HttpPost]
        public async Task<HttpResponseMessage> AddOrModify(MemberSharePromotionViewModel postedViewModel)
        {
            int branchId = IdentityConfig.GetBranchId(User);

            var mapper = GetMapper();
            var entityMapped = mapper.Map<MemberSharePromotionViewModel, MemberSharePromotion>(postedViewModel);
            entityMapped.BranchId = branchId;

            var result_biz = await businessService.CreateOrUpdateAsync(entityMapped);
            if (result_biz.ReturnStatus == false)
                return CreateErrorResponse(result_biz);

            return Request.CreateResponse(HttpStatusCode.OK, result_biz);

        }
        [Route("getMemberSharePromotion")]
        public async Task<HttpResponseMessage> GetMemberSharePromotionAsync(int statusId)
        {
            int branchId = IdentityConfig.GetBranchId(User);
            var result_bizloading = await businessService.LoadAsync(branchId,(PromotionReviewStatusEnum)statusId);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();

            var result = mapper.Map<List<MemberSharePromotion>, List<MemberSharePromotionViewModel>>(result_bizloading.ResultValue);
            return CreateSuccessedListResponse(result);
        }
        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(MemberSharePromotionViewModel postedViewModel)
        {
            int branchId = IdentityConfig.GetBranchId(User);

            var entities = await businessService.RemoveAsync(branchId, postedViewModel.MemberId);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            return Request.CreateResponse(HttpStatusCode.OK, entities);
        }
        [Route("releaseManagerStep1"), HttpPost]
        public async Task<HttpResponseMessage> ReleaseManagerStep1()
        {
            int branchId = IdentityConfig.GetBranchId(User);

            IBusinessResult entities = await businessService.ProceedMemberSharePromotionAsync(PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview
                , PromotionReviewStatusEnum.ReleadedStep1ByBranchManager
                ,branchId);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            return Request.CreateResponse(HttpStatusCode.OK, entities);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MemberSharePromotionMapperProfile());

                });
            return config.CreateMapper();
        }
        #endregion

    }


}