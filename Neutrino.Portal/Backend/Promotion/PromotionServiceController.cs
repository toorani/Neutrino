﻿using System;
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
                    .LoadAsync(includes: c => c.Status
                    , pageNumber: dataTablesModel.iDisplayStart
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

        [Route("getBranchPromotionsForCEOReview")]
        public async Task<HttpResponseMessage> GetBranchPromotionsForCEOReviewAsync()
        {
            IBusinessResultValue<List<BranchPromotion>> result_bizloading = await businessService.LoadBranchPromotions(PromotionReviewStatusEnum.ReleasedStep1ByBranchManager);
            if (result_bizloading.ReturnStatus == false)
                return CreateErrorResponse(result_bizloading);
            var mapper = GetMapper();

            var result = mapper.Map<List<BranchPromotion>, List<BranchPromotionViewModel>>(result_bizloading.ResultValue);
            return CreateSuccessedListResponse(result);
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