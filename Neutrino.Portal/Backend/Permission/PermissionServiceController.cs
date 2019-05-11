using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.Core;

using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using Neutrino.Core.SecurityManagement;
using Espresso.Portal;
using Neutrino.Interfaces;
using Espresso.BusinessService.Interfaces;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/permissionService")]
    public class PermissionServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IPermissionManager permissionManager;
        private readonly IPermissionBS businessService;
        private readonly IEntityListLoader<ApplicationAction> appActionLoader;
        #endregion

        #region [ Constructor(s) ]
        public PermissionServiceController(IPermissionManager permissionManager
            , IPermissionBS permissionBS
            , IEntityListLoader<ApplicationAction> appActionLoader)
        {
            this.permissionManager = permissionManager;
            businessService = permissionBS;
            this.appActionLoader = appActionLoader;
        }
        #endregion

        #region [ Public Method(s) ]
      
        public async Task<HttpResponseMessage> RemovePermission(HttpRequestMessage request, PermissionViewModel postedModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "the model is invalid");
            }

            IBusinessResult result = await businessService.DeleteAsync(postedModel.RoleId);
            if (result.ReturnStatus == false)
            {
                return CreateErrorResponse(result);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result.ReturnMessage);
        }

        [Route("getUserPermission"), HttpPost]
        public HttpResponseMessage GetUserPermission(HttpRequestMessage request)
        {
            List<UserAccessToken> userAccess = new List<UserAccessToken>();
            int userId = User.GetUserId();
            List<Permission> userPermissions = permissionManager.GetUserAccess(userId);

            userPermissions.ForEach(x =>
            {
                userAccess.Add(new UserAccessToken
                {
                    ActionId = x.ApplicationActionId,
                    RoleId = x.RoleId,
                    HtmlUrl = x.ApplicationAction.HtmlUrl,
                    ActionUrl = x.ApplicationAction.ActionUrl,

                });
            });
            return Request.CreateResponse(HttpStatusCode.OK, userAccess);
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Permission, PermissionViewModel>()
                .ReverseMap();
                cfg.CreateMap<DataList<Permission>, DataList<PermissionViewModel>>();
                cfg.AddProfile(new AccountMapperProfile());
            });
            return config.CreateMapper();
        }
        
        #endregion

    }

}