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
    [RoutePrefix("api/memberPromotionService")]
    public class MemberPromotionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IMemberPromotionBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public MemberPromotionServiceController(IMemberPromotionBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("addOrModify"), HttpPost]
        public async Task<HttpResponseMessage> AddOrModify(List<MemberPromotionManagerViewModel> postedViewModel)
        {
            var mapper = GetMapper();
            var entityMapped = mapper.Map<List<MemberPromotionManagerViewModel>, List<MemberPromotion>>(postedViewModel);

            var result_biz = await businessService.CreateOrUpdateAsync(entityMapped);
            if (result_biz.ReturnStatus == false)
                return CreateErrorResponse(result_biz);

            return Request.CreateResponse(HttpStatusCode.OK, result_biz);
        }
        [Route("getMemberPromotion")]
        public async Task<HttpResponseMessage> GetMemberPromotionAsync(int statusId)
        {
            int branchId = IdentityConfig.GetBranchId(User);
            var result_bizloading = await businessService.LoadAsync(branchId, (PromotionReviewStatusEnum)statusId);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();
            var result = new List<MemberPromotionViewModel>();
            if (result_bizloading.ResultValue != null)
                result = mapper.Map<List<MemberPromotion>, List<MemberPromotionViewModel>>(result_bizloading.ResultValue);
            return CreateSuccessedListResponse(result);
        }
        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(MemberPromotionViewModel postedViewModel)
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

            IBusinessResult entities = await businessService.ProceedMemberPromotionAsync(PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview
                , PromotionReviewStatusEnum.ReleasedStep1ByBranchManager
                , branchId);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            return Request.CreateResponse(HttpStatusCode.OK, entities);
        }

        [Route("addOrModfiyFinalPromotion"), HttpPost]
        public async Task<HttpResponseMessage> AddOrModfiyFinalPromotion(List<MemberPromotionViewModel> promotionViewModels)
        {
            var mapper = GetMapper();
            var entities = mapper.Map<List<MemberPromotionViewModel>, List<MemberPromotion>>(promotionViewModels);

            var result_biz = await businessService.AddOrModfiyFinalPromotionAsync(entities);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { returnMessage = result_biz.ReturnMessage.ConcatAll() });
        }
        [Route("determinedPromotion"), HttpPost]
        public async Task<HttpResponseMessage> DeterminedPromotion(List<MemberPromotionViewModel> postedViewModels)
        {
            var mapper = GetMapper();
            var entities = mapper.Map<List<MemberPromotionViewModel>, List<MemberPromotion>>(postedViewModels);

            var result_biz = await businessService.DeterminedPromotion(entities);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = (int)result_biz.ResultValue, returnMessage = result_biz.ReturnMessage.ConcatAll() });
        }

        [Route("getMemberPromotionForManager")]
        public async Task<HttpResponseMessage> GetMemberPromotionForManagerAsync(int memberId, int month, int year)
        {
            var result_biz = await businessService.LoadMemberPromotionAsync(memberId, month, year, ReviewPromotionStepEnum.Manager);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            var mapper = GetMapper();
            var result = mapper.Map<MemberPromotion, MemberPromotionManagerViewModel>(result_biz.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getMemberPromotionList4Manager"), HttpGet]
        public async Task<HttpResponseMessage> GetMemberPromotionList4ManagerAsync(int month, int year)
        {
            var branchId = IdentityConfig.GetBranchId(User);
            var result_biz = await businessService.LoadMemberPromotionListAsync(branchId, month, year, ReviewPromotionStepEnum.Manager);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<MemberPromotion>, List<MemberPromotionManagerViewModel>>(result_biz.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MemberPromotionMapperProfile());

                });
            return config.CreateMapper();
        }
        #endregion

    }


}