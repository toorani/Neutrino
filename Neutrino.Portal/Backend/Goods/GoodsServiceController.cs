using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using jQuery.DataTables.WebApi;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/GoodsService")]
    public class GoodsServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<Goods> lstLoaderBs;
        private readonly IEntityListByPagingLoader<Goods> lstByPagingLoaderBs;
        #endregion

        #region [ Constructor(s) ]
        public GoodsServiceController(IEntityListLoader<Goods> lstLoader
            , IEntityListByPagingLoader<Goods> lstByPagingLoader)
        {
            this.lstLoaderBs = lstLoader;
            this.lstByPagingLoaderBs = lstByPagingLoader;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataGrid"), HttpPost]
        public async Task<HttpResponseMessage> GetDataGrid(JQueryDataTablesModel dataTablesModel)
        {
            IBusinessLoadByPagingResult<Goods> entities = await lstByPagingLoaderBs.LoadAsync(
                    includes: ent => new { ent.Company },
                    orderBy: Utilities.GetOrderBy<Goods>("id", "asc"));
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            List<GoodsViewModel> dataSource = mapper.Map<List<Goods>, List<GoodsViewModel>>(entities.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK
                , DataTablesJson(items: dataSource,
               totalRecords: entities.TotalRows,
               totalDisplayRecords: entities.TotalRows,
               sEcho: dataTablesModel.sEcho));
        }
        [Route("getTreeGoodsList")]
        public HttpResponseMessage GetTreeGoodsList(int Id)
        {
            IBusinessResultValue<List<Goods>> entities = new BusinessResultValue<List<Goods>>();
            List<jsTreeViewModel> result = new List<jsTreeViewModel>();
            if (Id != 1)
            {
                entities = lstLoaderBs.LoadList(
                     where: goods => goods.CompanyId == Id,
                     orderBy: goods => goods.OrderBy(g => g.FaName));

                result.AddRange(from co in entities.ResultValue
                                select new jsTreeViewModel()
                                {
                                    Children = false,
                                    Text = co.EnName,
                                    Id = co.Id
                                });
            }
            else
            {
                entities = lstLoaderBs.LoadList(
                        includes: ent => new { ent.Company },
                        orderBy: Utilities.GetOrderBy<Goods>("id", "asc"));


                var groupCompanies = entities.ResultValue.GroupBy(co => co.CompanyId);
                var lstCompanies = groupCompanies.Select(grp => grp.OrderBy(item => item.Company.FaName).First()).ToList();

                result.AddRange(from co in lstCompanies
                                select new jsTreeViewModel()
                                {
                                    Text = co.Company.FaName,
                                    Id = co.CompanyId
                                });
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("getCompanyGoods")]
        public async Task<HttpResponseMessage> GetCompanyGoods(int companyId, string enName = null, string faName = null, string goodsCode = null)
        {
            List<GoodsViewModel> result = new List<GoodsViewModel>();

            IBusinessResultValue<List<Goods>> entities = await lstLoaderBs.LoadListAsync(where: gd => gd.CompanyId == companyId
            && (gd.EnName.StartsWith(enName) || enName == null
            || gd.FaName.StartsWith(faName) || faName == null
            || gd.GoodsCode.StartsWith(goodsCode) || goodsCode == null)
            , includes: x => new { x.Company }
            , orderBy: Utilities.GetOrderBy<Goods>("FaName", "asc"));


            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            result = mapper.Map<List<Goods>, List<GoodsViewModel>>(entities.ResultValue);

            return CreateSuccessedListResponse<GoodsViewModel>(result);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GoodsMapperProfile());
            });

            return config.CreateMapper();
        }
        #endregion

    }

}