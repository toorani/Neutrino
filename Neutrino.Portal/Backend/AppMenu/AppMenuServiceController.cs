using System.Net;
using System.Net.Http;
using System.Web.Http;
using Espresso.Core;

using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using AutoMapper;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.Portal;
using System.Collections.Generic;

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


        [Route("getAppMenu"), HttpGet]
        public async Task<HttpResponseMessage> GetAppMenuAsync(HttpRequestMessage request)
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