using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Portal.Attributes;
using Neutrino.Portal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Neutrino.Portal.WebApiControllers
{

    [RoutePrefix("api/AppMenuService")]
    public class AppMenuServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private IEntityListLoader<AppMenu> appMenuLoader;
        #endregion

        #region [ Constructor(s) ]
        public AppMenuServiceController(IEntityListLoader<AppMenu> appMenuLoader)
        {
            this.appMenuLoader = appMenuLoader;
        }
        #endregion


        [Route("getAppMenu"), HttpGet, WebApiAuthorize]
        public async Task<HttpResponseMessage> GetAppMenuAsync()
        {

            IBusinessResultValue<List<AppMenu>> entities = await appMenuLoader.LoadListAsync(
            orderBy: Utilities.GetOrderBy<AppMenu>("OrderId", "asc")
            , includes: x => new { x.ChildItems }
            , where: x => x.ParentId == null);

            if (entities.ReturnStatus == false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var mapper = GetMapper();
            DataList<ApplicationMenuViewModel> dataSource = mapper.Map<List<AppMenu>, DataList<ApplicationMenuViewModel>>(entities.ResultValue);
            return Request.CreateResponse(HttpStatusCode.OK, dataSource);
        }

        [Route("getTreeAppMenu"), HttpGet]
        public async Task<HttpResponseMessage> GetTreeAppMenuAsync()
        {
            var entities = await appMenuLoader.LoadListAsync(
            orderBy: Utilities.GetOrderBy<AppMenu>("OrderId", "desc")
            , includes: x => new { x.ChildItems });

            if (entities.ReturnStatus == false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var result = new List<jsTreeStaticViewModel>();
            
            result.AddRange(from menu in entities.ResultValue
                            where menu.ParentId == null
                            select new jsTreeStaticViewModel()
                            {
                                Text = menu.Title,
                                Id = menu.Id.ToString(),
                                Parent = "0",
                                ExtraData = menu.Url,
                                State = new NodeState { Opened = false }
                            });

            result.AddRange(from menu in entities.ResultValue
                            where menu.ParentId != null
                            select new jsTreeStaticViewModel()
                            {
                                Text = menu.Title,
                                Id = menu.Id.ToString(),
                                Parent = menu.ParentId.ToString(),
                                ExtraData = menu.Url,
                                State = new NodeState { Opened = false }
                            });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppMenu, ApplicationMenuViewModel>()
                .ConstructUsing(vm => new ApplicationMenuViewModel());
                cfg.CreateMap<List<AppMenu>, List<ApplicationMenuViewModel>>();
            });
            return config.CreateMapper();
        }

    }
}