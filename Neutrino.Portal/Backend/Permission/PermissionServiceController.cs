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
        #endregion

        #region [ Constructor(s) ]
        public PermissionServiceController(IPermissionManager permissionManager, IPermissionBS permissionBS)
        {
            this.permissionManager = permissionManager;
            businessService = permissionBS;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("addOrModifyPermission"),HttpPost]
        public async Task<HttpResponseMessage> AddOrModifyPermission(PermissionViewModel postedModel)
        {
            var mapper = GetMapper();
            var entity = mapper.Map<PermissionViewModel, Permission>(postedModel);

            IBusinessResult result = await businessService.CreateOrModifyPermissionAsync(entity);
            if (result.ReturnStatus == false)
            {
                return CreateErrorResponse(result);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getRolePermission")]
        public async Task<HttpResponseMessage> GetRolePermission(int roleId)
        {
            var lst_permissions = await businessService.LoadRolePermission(roleId);
            if (lst_permissions.ReturnStatus == false)
            {
                return CreateErrorResponse(lst_permissions);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<Permission>, PermissionViewModel>(lst_permissions.ResultValue);
            result.RoleId = roleId;
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getUserPermission"), HttpPost]
        public HttpResponseMessage GetUserPermission()
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
                cfg.AddProfile(new PermissionMapperProfile());
                cfg.AddProfile(new AccountMapperProfile());
            });
            return config.CreateMapper();
        }

        #endregion

    }

}