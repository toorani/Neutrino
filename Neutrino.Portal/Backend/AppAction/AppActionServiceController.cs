using AutoMapper;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/appActionService")]
    public class AppActionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IApplicationActionBS applicationActionBS;
        #endregion

        #region [ Constructor(s) ]
        public AppActionServiceController(IApplicationActionBS applicationActionBS)
        {
            this.applicationActionBS = applicationActionBS;
        }
        public AppActionServiceController()
        {

        }
        #endregion

        #region [ Public Method(s) ]
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
        [Route("getActionsByUrl"), HttpGet]
        public async Task<HttpResponseMessage> GetActionsByUrl(string htmlUrl)
        {
            var entity = await applicationActionBS.EntityListLoader.LoadListAsync(x => x.HtmlUrl == htmlUrl);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<ApplicationAction>, UrlActionViewModel>(entity.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("addOrModify"), HttpPost]
        public async Task<HttpResponseMessage> AddOrModify(UrlActionViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var applicationActions = mapper.Map<UrlActionViewModel, List<ApplicationAction>>(postedViewModel);
            var businessResult = await applicationActionBS.CreateOrModify(applicationActions);
            if (businessResult.ReturnStatus == false)
            {
                return CreateErrorResponse(businessResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, businessResult);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationActionMapperProfile());
            });
            return config.CreateMapper();
        }

        #endregion

    }

}