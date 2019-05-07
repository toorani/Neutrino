using AutoMapper;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace Neutrino.Portal
{
    [RoutePrefix("api/promotionReportService")]
    public class PromotionReportServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IPromotionBS promotionBS;
        #endregion

        #region [ Constructor(s) ]
        public PromotionReportServiceController(IPromotionBS promotionBS)
        {
            this.promotionBS = promotionBS;
        }
        #endregion

        #region [ Public Method(s) ]

        [Route("getOverView")]
        public async Task<HttpResponseMessage> GetOverView(int year, int month)
        {
            var entity = await promotionBS.LoadReportOverView(year, month);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<List<BranchPromotionViewModel>>(entity.ResultValue);

            return CreateSuccessedListResponse(dataModelView);
        }
        [Route("exportExcelOverView"), HttpGet]
        public async Task<HttpResponseMessage> ExportExcelOverView(int year, int month)
        {
            var entity = await promotionBS.LoadReportOverView(year, month);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<List<BranchPromotionViewModel>>(entity.ResultValue);
            string caption = $"عملکرد نهایی سال {year} ماه {month} ";
            var excelTemplate = HostingEnvironment.MapPath("/Views/Promotion/overviewrpt/excelTemplate.html");
            var result = ExportToExcel.GetExcelFile<BranchPromotionViewModel>(dataModelView, "OverView", excelTemplate, caption);
            return result;
        }

        [Route("getBranchSaleGoals")]
        public async Task<HttpResponseMessage> GetBranchSaleGoals(string startDate, string endDate, int goalGoodsCategoryId)
        {
            DateTime? startDateTime = Utilities.ToDateTime(startDate);
            DateTime? endDateTime = Utilities.ToDateTime(endDate);
            if (startDateTime.HasValue && endDateTime.HasValue && goalGoodsCategoryId != 0)
            {
                var entity = await promotionBS.LoadReportBranchSalesGoal(startDateTime.Value, endDateTime.Value, goalGoodsCategoryId);
                if (entity.ReturnStatus == false)
                {
                    return CreateErrorResponse(entity);
                }

                var lst_responses = entity.ResultValue.GroupBy(x => new { x.BranchName })
                    .Select(x => new ReportBranchSalesGoalViewModel
                    {
                        BranchName = x.Key.BranchName,
                        TotalSales = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalSales,
                        TotalQuantity = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalQuantity,
                        FinalPromotion = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).FinalPromotion,
                        PromotionWithOutFulfillmentPercent = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).PromotionWithOutFulfillmentPercent,
                        PromotionGoalSteps = x.Select(y => new PromotionGoalStep
                        {
                            AmountSpecified = y.AmountSpecified,
                            FulfilledPercent = Math.Round(y.FulfilledPercent, MidpointRounding.AwayFromZero),
                            GoalAmount = y.GoalAmount,
                        }).ToList()
                    }).ToList();

                return CreateSuccessedListResponse(lst_responses);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("exportExcelSaleGoals"), HttpGet]
        public async Task<HttpResponseMessage> ExportExcelSaleGoals(string startDate, string endDate, int goalGoodsCategoryId)
        {
            DateTime? startDateTime = Utilities.ToDateTime(startDate);
            DateTime? endDateTime = Utilities.ToDateTime(endDate);
            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                var entity = await promotionBS.LoadReportBranchSalesGoal(startDateTime.Value, endDateTime.Value, goalGoodsCategoryId);
                if (entity.ReturnStatus == false)
                {
                    return CreateErrorResponse(entity);
                }
                if (entity.ResultValue.Count != 0)
                {
                    var goalGoodsCategoryName = entity.ResultValue.First().GoalGoodsCategoryName;
                    var lst_responses = entity.ResultValue.GroupBy(x => new { x.BranchName })
                        .Select(x => new ReportBranchSalesGoalViewModel
                        {
                            BranchName = x.Key.BranchName,
                            TotalSales = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalSales,
                            TotalQuantity = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalQuantity,
                            FinalPromotion = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).FinalPromotion,
                            PromotionWithOutFulfillmentPercent = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).PromotionWithOutFulfillmentPercent,
                            PromotionGoalSteps = x.Select(y => new PromotionGoalStep
                            {
                                AmountSpecified = y.AmountSpecified,
                                FulfilledPercent = Math.Round(y.FulfilledPercent, MidpointRounding.AwayFromZero),
                                GoalAmount = y.GoalAmount,
                            }).ToList()
                        }).ToList();

                    string caption = $" گزارش عملکرد اهداف فروش محدوده تاریخ {startDate} - {endDate} هدف {goalGoodsCategoryName}";
                    var excelTemplate = HostingEnvironment.MapPath("/Views/Promotion/branchsalesrpt/excelTemplate.html");
                    var result = ExportToExcel.GetExcelFile<ReportBranchSalesGoalViewModel>(lst_responses
                        , outputFileName: "branchSalegoals"
                        , excelTemplatePath: excelTemplate
                        , caption: caption
                        , generatorHeader: (List<ReportBranchSalesGoalViewModel> data, string template) =>
                        {
                            if (data.Count == 0)
                                return string.Empty;
                            var loop = string.Empty;
                            for (int i = 1; i <= data[0].PromotionGoalSteps.Count; i++)
                            {
                                loop += template.Replace("$index", i.ToString());
                            }

                            return loop;
                        }
                        , getLoopObjects: (ReportBranchSalesGoalViewModel record) => record.PromotionGoalSteps
                        );
                    return result;
                }
                else
                    Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("getBranchReceiptGoals")]
        public async Task<HttpResponseMessage> GetBranchReceiptGoals(int year, int month, int goalGoodsCategoryTypeId)
        {

            var entity = await promotionBS.LoadReportBranchReceipt(year, month, (GoalGoodsCategoryTypeEnum)goalGoodsCategoryTypeId);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }
            var lst_responses = entity.ResultValue.GroupBy(x => new { x.BranchName })
               .Select(x => new ReportBranchReceiptGoalViewModel
               {
                   BranchName = x.Key.BranchName,
                   TotalSales = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalSales,
                   TotalQuantity = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalQuantity,
                   FinalPromotion = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).FinalPromotion,
                   FulfilledPercent = Math.Round(x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).FulfilledPercent, MidpointRounding.AwayFromZero),
                   AmountSpecified = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).AmountSpecified,
                   ReceiptAmount = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).ReceiptAmount,
                   PositionTotalAmount = x.Sum(y => y.PositionPromotion),
                   PositionPromotions = x.Select(y => new PositionPromotion
                   {
                       PositionTitle = y.PositionTitle,
                       Promotion = y.PositionPromotion
                   }).ToList()
               }).ToList();
            return CreateSuccessedListResponse(lst_responses);
        }

        [Route("exportExcelBranchReceiptGoals"), HttpGet]
        public async Task<HttpResponseMessage> ExportExcelBranchReceiptGoals(int year, int month, int goalGoodsCategoryTypeId)
        {
            var entity = await promotionBS.LoadReportBranchReceipt(year, month, (GoalGoodsCategoryTypeEnum)goalGoodsCategoryTypeId);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }
            var lst_responses = entity.ResultValue.GroupBy(x => new { x.BranchName })
               .Select(x => new ReportBranchReceiptGoalViewModel
               {
                   BranchName = x.Key.BranchName,
                   TotalSales = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalSales,
                   TotalQuantity = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalQuantity,
                   FinalPromotion = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).FinalPromotion,
                   FulfilledPercent = Math.Round(x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).FulfilledPercent, MidpointRounding.AwayFromZero),
                   AmountSpecified = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).AmountSpecified,
                   ReceiptAmount = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).ReceiptAmount,
                   PositionTotalAmount = x.Sum(y => y.PositionPromotion),
                   GoalGoodsCategoryName = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).GoalGoodsCategoryName,
                   PositionPromotions = x.Select(y => new PositionPromotion
                   {
                       PositionTitle = y.PositionTitle,
                       Promotion = y.PositionPromotion
                   }).ToList()
               }).ToList();

            string caption = $" گزارش عملکرد {lst_responses.First().GoalGoodsCategoryName} سال {year} - ماه {month} ";
            var excelTemplate = HostingEnvironment.MapPath("/Views/Promotion/branchreceiptrpt/excelTemplate.html");
            var result = ExportToExcel.GetExcelFile<ReportBranchReceiptGoalViewModel>(lst_responses
                , outputFileName: "branchReceiptgoals"
                , excelTemplatePath: excelTemplate
                , caption: caption
                , getLoopObjects: (ReportBranchReceiptGoalViewModel record) => record.PositionPromotions
                );
            return result;
        }
        [Route("getBranchPromotionDetail"), HttpGet]
        public async Task<HttpResponseMessage> GetBranchPromotionDetail(string startDate, string endDate)
        {
            DateTime? startDateTime = Utilities.ToDateTime(startDate);
            DateTime? endDateTime = Utilities.ToDateTime(endDate);
            var entity = await promotionBS.LoadReportBranchPromotionDetail(startDateTime.Value, endDateTime.Value);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var dataResult = entity.ResultValue.GroupBy(x => x.BranchId)
                .Select(x => new ReportBranchPromotionDetailViewModel
                {
                    BranchId = x.Key,
                    BranchName = x.FirstOrDefault(y => y.BranchId == x.Key).BranchName,
                    ManagerFulfillmentPercent = x.FirstOrDefault(y => y.BranchId == x.Key).ManagerFulfillmentPercent,
                    SellerFulfillmentPercent = x.FirstOrDefault(y => y.BranchId == x.Key).SellerFulfillmentPercent,
                    TotalFinalPromotion = x.FirstOrDefault(y => y.BranchId == x.Key).TotalFinalPromotion,
                    TotalPromotionWithOutFulfillmentPercent = x.FirstOrDefault(y => y.BranchId == x.Key).TotalPromotionWithOutFulfillmentPercent,
                    GoalPromotions = x.Select(y => new GoalPromotion
                    {
                        FinalPromotion = y.FinalPromotion,
                        GoalGoodsCategoryName = y.GoalGoodsCategoryName,
                        PromotionWithOutFulfillmentPercent = y.PromotionWithOutFulfillmentPercent
                    }).ToList()
                }).ToList();

            return CreateSuccessedListResponse(dataResult);
        }
        [Route("exportExcelBranchPromotionDetail"), HttpGet]
        public async Task<HttpResponseMessage> ExportExcelBranchPromotionDetail(string startDate, string endDate)
        {
            DateTime? startDateTime = Utilities.ToDateTime(startDate);
            DateTime? endDateTime = Utilities.ToDateTime(endDate);
            var entity = await promotionBS.LoadReportBranchPromotionDetail(startDateTime.Value, endDateTime.Value);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var dataResult = entity.ResultValue.GroupBy(x => x.BranchId)
                .Select(x => new ReportBranchPromotionDetailViewModel
                {
                    BranchId = x.Key,
                    BranchName = x.FirstOrDefault(y => y.BranchId == x.Key).BranchName,
                    ManagerFulfillmentPercent = x.FirstOrDefault(y => y.BranchId == x.Key).ManagerFulfillmentPercent,
                    SellerFulfillmentPercent = x.FirstOrDefault(y => y.BranchId == x.Key).SellerFulfillmentPercent,
                    TotalFinalPromotion = x.FirstOrDefault(y => y.BranchId == x.Key).TotalFinalPromotion,
                    TotalPromotionWithOutFulfillmentPercent = x.FirstOrDefault(y => y.BranchId == x.Key).TotalPromotionWithOutFulfillmentPercent,
                    GoalPromotions = x.Select(y => new GoalPromotion
                    {
                        FinalPromotion = y.FinalPromotion,
                        GoalGoodsCategoryName = y.GoalGoodsCategoryName,
                        PromotionWithOutFulfillmentPercent = y.PromotionWithOutFulfillmentPercent
                    }).ToList()
                }).ToList();
            string caption = $" گزارش پورسانت مراکز از فروش {startDate} - {endDate} ";
            var excelTemplate = HostingEnvironment.MapPath("/Views/Promotion/branchsalesoverviewrpt/excelTemplate.html");
            var result = ExportToExcel.GetExcelFile<ReportBranchPromotionDetailViewModel>(dataResult
                , outputFileName: "branchpromotion"
                , excelTemplatePath: excelTemplate
                , caption: caption
                , getLoopObjects: (ReportBranchPromotionDetailViewModel record) => record.GoalPromotions
                );
            return result;
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new PromotionReportMapperProfile());
                });
            return config.CreateMapper();
        }
        #endregion

    }


}