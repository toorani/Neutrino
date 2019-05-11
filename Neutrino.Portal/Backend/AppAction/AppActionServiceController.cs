using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Portal;
using jQuery.DataTables.WebApi;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/appActionService")]
    public class AppActionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListByPagingLoader<ApplicationAction> appActionListByPagingLoader;
        private readonly IEntityListByPagingLoader<ApplicationAction> appActionListLoader;
        private readonly IEntityLoader<ApplicationAction> appActionLoader;
        #endregion

        #region [ Constructor(s) ]
        public AppActionServiceController(IEntityListByPagingLoader<ApplicationAction> appActionListByPagingLoader
            , IEntityListByPagingLoader<ApplicationAction> appActionListLoader
            , IEntityLoader<ApplicationAction> appActionLoader)
        {
            this.appActionListByPagingLoader = appActionListByPagingLoader;
            this.appActionListLoader = appActionListLoader;
            this.appActionLoader = appActionLoader;
        }
        public AppActionServiceController()
        {

        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataGrid"), HttpPost]
        public virtual async Task<HttpResponseMessage> GetDataGrid(JQueryDataTablesModel dataTablesModel)
        {
            var entities = await appActionListByPagingLoader.LoadAsync(
                    pageNumber: dataTablesModel.iDisplayStart
                    , pageSize: dataTablesModel.iDisplayLength
                    , orderBy: UIHelper.GetOrderBy<ApplicationAction, ApplicationActionViewModel>(dataTablesModel.GetSortedColumns())
                    , where: or => or.ActionUrl.Contains(dataTablesModel.sSearch)
                    || or.HtmlUrl.Contains(dataTablesModel.sSearch)
                    || dataTablesModel.sSearch == "");

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            List<ApplicationActionViewModel> dataSource = mapper.Map<List<ApplicationAction>, List<ApplicationActionViewModel>>(entities.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK
                , DataTablesJson(items: dataSource
                , totalRecords: entities.TotalRows
                , totalDisplayRecords: entities.TotalRows
                , sEcho: dataTablesModel.sEcho));

        }
        [Route("getTreeActionsList")]
        public async Task<HttpResponseMessage> GetTreeActionList(int? selectedId)
        {
            var entities = await appActionListLoader.LoadAsync();

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            List<jsTreeStaticViewModel> result = new List<jsTreeStaticViewModel>();
            result.AddRange(from act in entities.ResultValue
                            select new jsTreeStaticViewModel()
                            {
                                Id = act.Id.ToString(),
                                ExtraData = act.ActionUrl + "," + act.HtmlUrl,
                                State = new NodeState { Opened = true, Selected = selectedId == act.Id }
                            });


            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("getDataItem")]
        public async Task<HttpResponseMessage> GetDataItem(int id)
        {
            var entity = await appActionLoader.LoadAsync(id);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<ApplicationActionViewModel>(entity.ResultValue);

            return CreateViewModelResponse(dataModelView, entity);
        }
        [Route("getAllAction"), HttpGet]
        public HttpResponseMessage GetAllActions()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var api_actions = asm.GetTypes()
                .Where(type => typeof(ApiControllerBase).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && method.IsDefined(typeof(RouteAttribute)))
                .GroupBy(x => x.DeclaringType.CustomAttributes.Select(z => z.ConstructorArguments).FirstOrDefault().FirstOrDefault().Value)
                .Select(x => new
                {
                    Api = x.Key,
                    Actions = x.Select(y => y.CustomAttributes.Where(z => z.AttributeType == typeof(RouteAttribute))
                    .Select(u => u.ConstructorArguments)
                    .FirstOrDefault()
                    .FirstOrDefault().Value).ToList()
                });
            var result = new List<jsTreeStaticViewModel>();
            var counter = 1;
            foreach (var item in api_actions)
            {
                result.Add(new jsTreeStaticViewModel()
                {
                    Id = counter.ToString(),
                    Text = item.Api.ToString(),
                    State = new NodeState { Opened = false },
                    Parent = "0"
                });

                result.AddRange(from act in item.Actions
                                select new jsTreeStaticViewModel()
                                {
                                    Id = counter + "_" + act.ToString(),
                                    Text = item.Api + "/" + act,
                                    Parent = counter.ToString(),
                                    State = new NodeState { Opened = true }
                                });
                counter++;

            }

            //var result = (from api in api_actions
            //              from act in api.Actions
            //              select api.Api + "/" + act);


            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("addOrModify"),HttpPost]
        public Task<HttpResponseMessage> AddOrModify(UrlActionViewModel postedViewModel)
        {

        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationAction, AppActionPermissionViewModel>()
                .ConstructUsing(vm => new AppActionPermissionViewModel())
                .ReverseMap();

                cfg.CreateMap<ApplicationAction, ApplicationActionViewModel>()
                .ConstructUsing(vm => new ApplicationActionViewModel())
                .ReverseMap();

            });
            return config.CreateMapper();
        }

        #endregion

    }

}