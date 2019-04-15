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
        [Route("addPermission"), HttpPost]
        public async Task<HttpResponseMessage> AddPermission(PermissionViewModel postedModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "the model is invalid");
            }

            Permission permission = new Permission();

            IBusinessResultValue<List<ApplicationAction>> actions = await appActionLoader.LoadListAsync(includes: x => x.Permissions);
            if (actions.ReturnStatus)
            {
                List<Permission> lstAddPermissions = new List<Permission>();
                List<Permission> lstRemovePermissions = new List<Permission>();
                postedModel.Actions.ForEach(x =>
                {
                    AddOrRemovePermission(postedModel.RoleId, x.CanCreate, x.Id, actions.ResultValue, lstAddPermissions, lstRemovePermissions, AppActionTypes.Create);
                    AddOrRemovePermission(postedModel.RoleId, x.CanDelete, x.Id, actions.ResultValue, lstAddPermissions, lstRemovePermissions, AppActionTypes.Delete);
                    AddOrRemovePermission(postedModel.RoleId, x.CanRead, x.Id, actions.ResultValue, lstAddPermissions, lstRemovePermissions, AppActionTypes.Read);
                    AddOrRemovePermission(postedModel.RoleId, x.CanUpdate, x.Id, actions.ResultValue, lstAddPermissions, lstRemovePermissions, AppActionTypes.Update);
                });

                IBusinessResult result = await businessService.ModifyPermissionAsync(lstAddPermissions, lstRemovePermissions);

                if (result.ReturnStatus == false)
                {
                    return CreateErrorResponse(result);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result.ReturnMessage);
            }
            return CreateErrorResponse(actions);
        }
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
                    ActionTypeId = (int)x.ApplicationAction.ActionTypeId,
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
        private void AddOrRemovePermission(int roleId
            , bool? actionPermissionModel
            , int parentId
            , List<ApplicationAction> actions
            , List<Permission> lstAddPermissions
            , List<Permission> lstRemovePermissions
            , AppActionTypes actionTypeId)
        {
            if (actionPermissionModel.HasValue)
            {
                var selectActions = actions.Where(y => y.ActionTypeId == actionTypeId && (y.ParentId == parentId || y.Id == parentId)).ToList();
                selectActions.ForEach(act =>
                {
                    var permission = act.Permissions.FirstOrDefault(x => x.RoleId == roleId && x.Deleted == false);
                    if (actionPermissionModel.Value && permission == null)
                    {
                        lstAddPermissions.Add(new Permission
                        {
                            ApplicationActionId = act.Id,
                            RoleId = roleId
                        });
                    }
                    else if (!actionPermissionModel.Value && permission != null)
                    {
                        lstRemovePermissions.Add(permission);
                    }
                });

            }
        }
        #endregion

    }

}