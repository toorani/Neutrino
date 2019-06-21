using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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
            var result_bizloading = await businessService.LoadAsync(branchId, (PromotionReviewStatusEnum)statusId);
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
                , branchId);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            return Request.CreateResponse(HttpStatusCode.OK, entities);
        }

        [Route("addOrModfiyFinalPromotion"), HttpPost]
        public async Task<HttpResponseMessage> AddOrModfiyFinalPromotion(List<MemberSharePromotionViewModel> promotionViewModels)
        {
            var mapper = GetMapper();
            var entities = mapper.Map<List<MemberSharePromotionViewModel>, List<MemberSharePromotion>>(promotionViewModels);

            var result_biz = await businessService.AddOrModfiyFinalPromotionAsync(entities);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { returnMessage = result_biz.ReturnMessage.ConcatAll() });
        }
        [Route("determinedPromotion"), HttpPost]
        public async Task<HttpResponseMessage> DeterminedPromotion(List<MemberSharePromotionViewModel> postedViewModels)
        {
            var mapper = GetMapper();
            var entities = mapper.Map<List<MemberSharePromotionViewModel>, List<MemberSharePromotion>>(postedViewModels);

            var result_biz = await businessService.DeterminedPromotion(entities);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = (int)result_biz.ResultValue, returnMessage = result_biz.ReturnMessage.ConcatAll() });
        }

        [Route("getMemberSharePromotionForManager")]
        public async Task<HttpResponseMessage> GetMemberSharePromotionForManagerAsync(int memberId, int month, int year)
        {
            var result_biz = await businessService.LoadMemberSharePromotionAsync(memberId, month, year, SharePromotionTypeEnum.Manager);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            var mapper = GetMapper();
            var result = mapper.Map<MemberSharePromotion, MemberSharePromotionManagerViewModel>(result_biz.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getMemberSharePromotionList4Manager"),HttpGet]
        public async Task<HttpResponseMessage> GetMemberSharePromotionList4ManagerAsync(int month, int year)
        {
            var branchId = IdentityConfig.GetBranchId(User);
            var result_biz = await businessService.LoadMemberSharePromotionListAsync(branchId, month, year, SharePromotionTypeEnum.Manager);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<MemberSharePromotion>, List<MemberSharePromotionManagerViewModel>>(result_biz.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK, result);
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