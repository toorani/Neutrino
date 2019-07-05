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
    [RoutePrefix("api/branchPromotionService")]
    public class BranchPromotionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IBranchPromotionBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public BranchPromotionServiceController(IBranchPromotionBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataList")]
        public async Task<HttpResponseMessage> GetDataList(int promotionId)
        {
            var entity = await businessService.LoadListAsync(promotionId);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<List<BranchPromotionViewModel>>(entity.ResultValue);

            return CreateSuccessedListResponse(dataModelView);
        }
        [Route("addOrModify"), HttpPost]
        public async Task<HttpResponseMessage> AddOrModifyAsync(List<BranchPromotion> lstBranchPromotions)
        {
            var mapper = GetMapper();
            var entites = mapper.Map<List<BranchPromotion>>(lstBranchPromotions);
            var result_biz = await businessService.AddOrUpdateAsync(entites);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            return Request.CreateResponse(HttpStatusCode.OK,result_biz);
        }
        [Route("confirmCompensatory"), HttpPost]
        public async Task<HttpResponseMessage> ConfirmCompensatoryAsync(List<BranchPromotion> lstBranchPromotions)
        {
            var mapper = GetMapper();
            var entites = mapper.Map<List<BranchPromotion>>(lstBranchPromotions);
            var result_biz = await businessService.ConfirmCompensatoryAsync(entites);
            if (result_biz.ReturnStatus == false)
            {
                return CreateErrorResponse(result_biz);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result_biz);
        }

        [Route("getBranchPromotionForStep1")]
        public async Task<HttpResponseMessage> GetBranchPromotionDetailForStep1BranchManager()
        {
            int branchId = IdentityConfig.GetBranchId(User);
            var entities = await businessService.LoadBranchPromotionAsync(branchId, PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var result = new List<BranchPromotionDetailViewModel>()
            {
                new  BranchPromotionDetailViewModel(){
                    BranchId = branchId,
                    BranchName = entities.ResultValue.Branch.Name,
                    GoalTypeId = 1,
                    GoalTypeTitle = "پورسانت فروش تامین کننده",
                    PromotionReviewStatusId = (int)entities.ResultValue.PromotionReviewStatusId,
                    TotalFinalPromotion = entities.ResultValue.TotalSalesPromotion.Value,
                    PositionPromotions = null,
                    Month = entities.ResultValue.Month,
                    Year =  entities.ResultValue.Year,
                },
                new  BranchPromotionDetailViewModel(){
                    BranchId = branchId,
                    BranchName = entities.ResultValue.Branch.Name,
                    GoalTypeId = 4,
                    GoalTypeTitle = "پورسانت ترمیمی",
                    PromotionReviewStatusId = (int)entities.ResultValue.PromotionReviewStatusId,
                    TotalFinalPromotion = entities.ResultValue.CompensatoryPromotion,
                    PositionPromotions = null,
                    Month = entities.ResultValue.Month,
                    Year =  entities.ResultValue.Year,
                },
                new  BranchPromotionDetailViewModel(){
                    BranchId = branchId,
                    GoalTypeId = 2,
                    PromotionReviewStatusId = (int)entities.ResultValue.PromotionReviewStatusId,
                    BranchName = entities.ResultValue.Branch.Name,
                    GoalTypeTitle = "پورسانت وصول کل",
                    Month = entities.ResultValue.Month,
                    Year =  entities.ResultValue.Year,
                    TotalFinalPromotion = entities.ResultValue.TotalReceiptPromotion.Value,
                    PositionPromotions =  (from brgpl in entities.ResultValue.BranchGoalPromotions
                                          where brgpl.Goal.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal
                                          from posi in brgpl.PositionReceiptPromotions
                                          select new PositionPromotion
                                          {
                                              PositionTitle = posi.PositionType.Description,
                                              Promotion= posi.Promotion
                                          }).ToList()
                },
                new  BranchPromotionDetailViewModel(){
                    BranchId = branchId,
                    GoalTypeId = 3,
                    BranchName = entities.ResultValue.Branch.Name,
                    PromotionReviewStatusId = (int)entities.ResultValue.PromotionReviewStatusId,
                    GoalTypeTitle = "پورسانت وصول خصوصی",
                    Month = entities.ResultValue.Month,
                    Year =  entities.ResultValue.Year,
                    TotalFinalPromotion = entities.ResultValue.PrivateReceiptPromotion.Value,
                    PositionPromotions =  (from brgpl in entities.ResultValue.BranchGoalPromotions
                                          where brgpl.Goal.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal
                                          from posi in brgpl.PositionReceiptPromotions
                                          select new PositionPromotion
                                          {
                                              PositionTitle = posi.PositionType.Description,
                                              Promotion= posi.Promotion
                                          }).ToList()
                }
            };


            return CreateSuccessedListResponse(result);
        }
        [Route("getBranchPromotionReleasedStep1ByBranchManager")]
        public async Task<HttpResponseMessage> GetActiveBranchPromotion(int branchId)
        {
            var result_bizloading = await businessService.LoadBranchPromotionAsync(branchId, PromotionReviewStatusEnum.ReleasedStep1ByBranchManager);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();

            var result = mapper.Map<BranchPromotion, BranchPromotionViewModel>(result_bizloading.ResultValue);
            return CreateViewModelResponse(result, result_bizloading);
        }

        [Route("getBranchPromotionReleasedByCEO")]
        public async Task<HttpResponseMessage> GetActiveBranchPromotion()
        {
            int branchId = IdentityConfig.GetBranchId(User);
            IBusinessResultValue<BranchPromotion> result_bizloading = await businessService.LoadBranchPromotionAsync(branchId, PromotionReviewStatusEnum.ReleasedByCEO);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();

            var result = mapper.Map<BranchPromotion, BranchPromotionViewModel>(result_bizloading.ResultValue);
            return CreateViewModelResponse(result, result_bizloading);
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new BranchPromotionMapperProfile());

                });
            return config.CreateMapper();
        }
        #endregion

    }


}