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
    [RoutePrefix("api/promotionService")]
    public class PromotionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IPromotionBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public PromotionServiceController(IPromotionBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataGrid"), HttpPost]
        public virtual async Task<HttpResponseMessage> GetDataGrid(JQueryDataTablesModel dataTablesModel)
        {
            var entities = await businessService.EntityListByPagingLoader
                    .LoadAsync(
                    pageNumber: dataTablesModel.iDisplayStart
                    , pageSize: dataTablesModel.iDisplayLength
                    , orderBy: ord => ord.OrderByDescending(x => x.StartDate));

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            var dataSource = mapper.Map<List<Promotion>, List<PromotionViewModel>>(entities.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK
                , DataTablesJson(items: dataSource
                , totalRecords: entities.TotalRows
                , totalDisplayRecords: entities.TotalRows
                , sEcho: dataTablesModel.sEcho));

        }
        [Route("add"), HttpPost]
        public async Task<HttpResponseMessage> Add(PromotionViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityCreating = mapper.Map<Promotion>(postedViewModel);
            var entityCreated = await businessService.AddPromotionAsync(entityCreating);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }

            return CreateViewModelResponse(postedViewModel, entityCreated);
        }
        [Route("getDataItem")]
        public async Task<HttpResponseMessage> GetDataItem(int id)
        {
            var entity = await businessService.EntityLoader.LoadAsync(id);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<PromotionViewModel>(entity.ResultValue);

            return CreateViewModelResponse(dataModelView, entity);

        }
        [Route("startCalculation"), HttpPost]
        public async Task<HttpResponseMessage> StartCalculation(PromotionViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var commission = mapper.Map<Promotion>(postedViewModel);
            var entityCreated = await businessService.PutInProcessQueueAsync(postedViewModel.Year, postedViewModel.Month);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }

            return CreateViewModelResponse(postedViewModel, entityCreated);
        }

        [Route("addOrModifyMemberPromotion"), HttpPost]
        public async Task<HttpResponseMessage> AddOrModifyMemberSharePromotion(MemberSharePromotionViewModel postedViewModel)
        {
            int branchId = IdentityConfig.GetBranchId(User);

            var mapper = GetMapper();
            var entityMapped = mapper.Map<MemberSharePromotionViewModel, MemberSharePromotion>(postedViewModel);
            entityMapped.BranchId = branchId;

            var result_biz = await businessService.CreateOrUpdateMemberSharePromotionAsync(entityMapped);
            if (result_biz.ReturnStatus == false)
                return CreateErrorResponse(result_biz);

            return Request.CreateResponse(HttpStatusCode.OK, result_biz);

        }
        [Route("getMemberSharePromotion")]
        public async Task<HttpResponseMessage> GetMemberSharePromotionAsync(int statusId)
        {
            int branchId = IdentityConfig.GetBranchId(User);
            var result_bizloading = await businessService.LoadMemberSharePromotionAsync(branchId,(PromotionReviewStatusEnum)statusId);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();

            var result = mapper.Map<List<MemberSharePromotion>, List<MemberSharePromotionViewModel>>(result_bizloading.ResultValue);
            return CreateSuccessedListResponse(result);
        }
        [Route("deleteMemberSahrePromotion"), HttpPost]
        public async Task<HttpResponseMessage> DeleteMemberSahrePromotion(MemberSharePromotionViewModel postedViewModel)
        {
            int branchId = IdentityConfig.GetBranchId(User);

            var entities = await businessService.RemoveMemberSharePromotion(branchId, postedViewModel.MemberId);
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

        [Route("getMemberSharePromotion")]
        public async Task<HttpResponseMessage> GetMemberSharePromotionAsync(int statusId,int branchId)
        {
            var result_bizloading = await businessService.LoadMemberSharePromotionAsync(branchId, (PromotionReviewStatusEnum)statusId);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();

            var result = mapper.Map<List<MemberSharePromotion>, List<MemberSharePromotionViewModel>>(result_bizloading.ResultValue);
            return CreateSuccessedListResponse(result);
        }
        [Route("getBranchPromotionsForCEOReview")]
        public async Task<HttpResponseMessage> GetBranchPromotionsForCEOReviewAsync()
        {
            IBusinessResultValue<List<BranchPromotion>> result_bizloading = await businessService.LoadBranchPromotions(PromotionReviewStatusEnum.ReleadedStep1ByBranchManager);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();

            var result = mapper.Map<List<BranchPromotion>, List<BranchPromotionViewModel>>(result_bizloading.ResultValue);
            return CreateSuccessedListResponse(result);
        }

        [Route("getLastBranchPromotion")]
        public async Task<HttpResponseMessage> GetLastBranchPromotionAsync(int branchId,int statusId)
        {
            IBusinessResultValue<BranchPromotion> result_bizloading = await businessService.LoadBranchPromotion(branchId,(PromotionReviewStatusEnum)statusId);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();

            var result = mapper.Map<BranchPromotion, BranchPromotionViewModel>(result_bizloading.ResultValue);
            return CreateViewModelResponse(result,result_bizloading);
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new PromotionMapperProfile());

                });
            return config.CreateMapper();
        }
        #endregion

    }


}