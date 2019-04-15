using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataGrid"), HttpPost]
        public virtual async Task<HttpResponseMessage> GetDataGrid(JQueryDataTablesModel dataTablesModel)
        {
            var entities = await appActionListByPagingLoader.LoadAsync(
                    pageNumber: dataTablesModel.iDisplayStart
                    , pageSize: dataTablesModel.iDisplayLength
                    , orderBy: UIHelper.GetOrderBy<ApplicationAction, ApplicationActionViewModel>(dataTablesModel.GetSortedColumns())
                    , where: or => or.FaTitle.Contains(dataTablesModel.sSearch)
                    || or.Title.Contains(dataTablesModel.sSearch)
                    || or.ActionUrl.Contains(dataTablesModel.sSearch)
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
                                Text = act.FaTitle,
                                Id = act.Id.ToString(),
                                Parent = act.ParentId == null ? "#" : act.ParentId.ToString(),
                                ExtraData = act.ActionUrl + "," + act.HtmlUrl,
                                EnName = act.Title,
                                State = new NodeState { Opened = true, Selected = selectedId == act.Id }
                            });


            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getActionForPermission")]
        public async Task<HttpResponseMessage> GetActionForPermission(HttpRequestMessage request, int roleId)
        {
            var entities = await appActionListLoader.LoadAsync(includes: x => x.Permissions);
            
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();

            List<AppActionPermissionViewModel> result = new List<AppActionPermissionViewModel>();
            var rootAppAction = entities.ResultValue.FirstOrDefault(x => x.ParentId == null);
            AppActionPermissionViewModel viewModel = null;
            foreach (var item in entities.ResultValue.Where(x => x.ParentId == rootAppAction.Id))
            {
                viewModel = mapper.Map<ApplicationAction, AppActionPermissionViewModel>(item);
                viewModel.CanCreate = CheckAccess(entities.ResultValue, item.Id, AppActionTypes.Create, roleId);
                viewModel.CanDelete = CheckAccess(entities.ResultValue, item.Id, AppActionTypes.Delete, roleId);
                viewModel.CanRead = CheckAccess(entities.ResultValue, item.Id, AppActionTypes.Read, roleId);
                viewModel.CanUpdate = CheckAccess(entities.ResultValue, item.Id, AppActionTypes.Update, roleId);
                result.Add(viewModel);
            }

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
        private bool? CheckAccess(List<ApplicationAction> entities, int actionId, AppActionTypes appActionTypes, int roleId)
        {
            var appAction = entities.FirstOrDefault(x => (x.ParentId == actionId || x.Id == actionId) && x.ActionTypeId == appActionTypes);
            if (appAction == null)
                return null;
            return appAction != null && appAction.Permissions.Any(x => x.RoleId == roleId && x.Deleted == false);

        }
        #endregion

    }

}