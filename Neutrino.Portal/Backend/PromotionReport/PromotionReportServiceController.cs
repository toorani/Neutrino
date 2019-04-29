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
        #endregion

        #region [ Constructor(s) ]
        public PromotionReportServiceController(IEntityListLoader<BranchPromotion> entityListLoader)
        {
            this.branchPromotionLoader = entityListLoader;
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
            string caption = $"{month} ماه  - {year} عملکرد نهایی سال";
            var excelTemplate = HostingEnvironment.MapPath("/Views/Promotion/overviewrpt/OverviewExcelTemplate.html");
            var result = ExportToExcel.WriteHtmlTable<BranchPromotionViewModel>(dataModelView, "OverView", excelTemplate, caption);
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