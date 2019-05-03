using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using jQuery.DataTables.WebApi;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using Neutrino.Portal.Tools;

namespace Neutrino.Portal
{
    [RoutePrefix("api/promotionReportService")]
    public class PromotionReportServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<BranchPromotion> branchPromotionLoader;
        private readonly IPromotionBS promotionBS;
        #endregion

        #region [ Constructor(s) ]
        public PromotionReportServiceController(IEntityListLoader<BranchPromotion> entityListLoader
            , IPromotionBS promotionBS)
        {
            this.branchPromotionLoader = entityListLoader;
            this.promotionBS = promotionBS;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getOverView")]
        public async Task<HttpResponseMessage> GetOverView(int year, int month)
        {
            var entity = await branchPromotionLoader.LoadListAsync(x => x.Year == year && x.Month == month
            , includes: x => new { x.Branch }, orderBy: x => x.OrderBy(y => y.Branch.Order));
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
            var entity = await branchPromotionLoader.LoadListAsync(x => x.Year == year && x.Month == month
            , includes: x => new { x.Branch }, orderBy: x => x.OrderBy(y => y.Branch.Order));
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<List<BranchPromotionViewModel>>(entity.ResultValue);
            string caption = $"عملکرد نهایی سال {year} ماه {month} ";
            var excelTemplate = HostingEnvironment.MapPath("/Views/Promotion/overviewrpt/excelTemplate.html");
            var result = ExportToExcel.WriteHtmlTable<BranchPromotionViewModel>(dataModelView, "OverView", excelTemplate, caption);
            return result;
        }

        [Route("getBranchSaleGoals")]
        public async Task<HttpResponseMessage> GetBranchSaleGoals(string startDate, string endDate, int goalGoodsCategoryId)
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

                var lst_responses = entity.ResultValue.GroupBy(x => new { x.BranchName })
                    .Select(x => new ReportBranchSalesGoalViewModel
                    {
                        BranchName = x.Key.BranchName,
                        TotalSales = x.FirstOrDefault(y => y.BranchName == x.Key.BranchName).TotalSales,
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
                    var result = ExportToExcel.WriteHtmlTable<ReportBranchSalesGoalViewModel>(lst_responses
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