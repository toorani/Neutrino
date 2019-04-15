using System.Net;
using System.Net.Http;
using System.Web.Http;

using jQuery.DataTables.WebApi;

using Neutrino.Entities;
using Espresso.Core;
using Neutrino.Portal.Models;
using Ninject;
using AutoMapper;
using Neutrino.Portal.ProfileMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using Espresso.Portal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using Microsoft.Owin.Security;
using System;
using Espresso.Utilities.Interfaces;
using Neutrino.Interfaces;
using Espresso.BusinessService;

namespace Neutrino.Portal.WebApiControllers
{
    //[Authorize]
    [RoutePrefix("api/accountService")]
    public class AccountServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager signInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }
        private ApplicationUserManager userManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        private IAuthenticationManager authenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        private readonly IAppSettingManager appSettingManager;
        private readonly INeutrinoUserBS userBusinessService;
        private readonly INeutrinoRoleBS roleBusinessService;
        #endregion

        #region [ Public Property(ies) ]

        #endregion

        #region [ Constructor(s) ]
        public AccountServiceController(IAppSettingManager appSettingManager
            , INeutrinoUserBS userBusinessService
            , INeutrinoRoleBS roleBusinessService)
        {
            this.appSettingManager = appSettingManager;
            this.userBusinessService = userBusinessService;
            this.roleBusinessService = roleBusinessService;
        }

        #endregion

        #region [ User Method(s) ]

        [Route("getUsers"), HttpPost]
        public HttpResponseMessage GetGridData(JQueryDataTablesModel dataTablesModel)
        {
            var entities = userBusinessService.EntityListByPagingLoader.Load(
                where: UIHelper.GetWhere<NeutrinoUser>(dataTablesModel.sSearch),
                orderBy: UIHelper.GetOrderBy<NeutrinoUser, RegisterViewModel>(dataTablesModel.GetSortedColumns()),
                pageNumber: dataTablesModel.iDisplayStart,
                pageSize: dataTablesModel.iDisplayLength);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            List<RegisterViewModel> dataSource = mapper.Map<List<NeutrinoUser>, List<RegisterViewModel>>(entities.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK
                   , DataTablesJson(items: dataSource
                   , totalRecords: entities.TotalRows
                   , totalDisplayRecords: entities.TotalRows
                   , sEcho: dataTablesModel.sEcho));
        }

        [Route("getDataItem")]
        public async Task<HttpResponseMessage> GetDataItem(int id)
        {
            var entity = await userBusinessService.LoadUserAsync(id);

            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            RegisterViewModel dataModelView = mapper.Map<NeutrinoUser, RegisterViewModel>(entity.ResultValue);
            return CreateViewModelResponse(dataModelView, entity);
        }
        [Route("register"), HttpPost]
        public async Task<HttpResponseMessage> RegisterAsync(HttpRequestMessage request, RegisterViewModel postedViewModel)
        {
            var user = new ApplicationUser
            {
                UserName = postedViewModel.UserName,
                Email = postedViewModel.Email,
                Name = postedViewModel.Name,
                LastName = postedViewModel.LastName,
                PhoneNumber = postedViewModel.MobileNumber,

            };
            postedViewModel.Roles.ForEach((item) =>
            {
                IdentityUserRole<int> role = new IdentityUserRole<int>();
                role.RoleId = item.Id;
                user.Roles.Add(role);
            });
            var result = await userManager.CreateAsync(user, postedViewModel.Password);

            postedViewModel.ActionResult.ReturnStatus = result.Succeeded;
            if (postedViewModel.ActionResult.ReturnStatus == false)
            {
                postedViewModel.ActionResult.ReturnMessage.AddRange(result.Errors);
                return CreateErrorResponse(postedViewModel.ActionResult);
            }
            postedViewModel.ActionResult.ReturnMessage.Add("اطلاعات کاربری با موفقیت ثبت شد");
            postedViewModel.Id = user.Id;

            return Request.CreateResponse(HttpStatusCode.OK, postedViewModel);
        }

        [Route("login"), HttpPost, AllowAnonymous]
        public async Task<HttpResponseMessage> LoginAsync(HttpRequestMessage request, RegisterViewModel postedViewModel)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await signInManager.PasswordSignInAsync(postedViewModel.UserName, postedViewModel.Password, postedViewModel.RememberMe, shouldLockout: false);
            postedViewModel.ActionResult = new BusinessResult();
            switch (result)
            {
                case SignInStatus.Success:
                    
                    postedViewModel.ActionResult.ReturnStatus = true;
                    var user = userManager.Find(postedViewModel.UserName, postedViewModel.Password);
                    List<string> roles = await userManager.GetRolesAsync(user.Id) as List<string>;
                    //var userPermissions = PermissionManager.Instance.GetUserAccess(user.Id);
                    //userPermissions.ForEach(x => {
                    //    postedViewModel.UserAccessTokens.Add(new UserAccessToken
                    //    {
                    //        ActionId = x.ApplicationActionId,
                    //        RoleId = x.RoleId,
                    //        HtmlUrl = x.ApplicationAction.HtmlUrl,
                    //        ActionUrl = x.ApplicationAction.ActionUrl
                    //    });
                    //});
                    if (string.IsNullOrWhiteSpace(postedViewModel.ReturnUrl))
                        postedViewModel.ReturnUrl = "/home/index";
                    break;
                case SignInStatus.Failure:
                default:
                    postedViewModel.ActionResult.ReturnStatus = false;
                    postedViewModel.ActionResult.ReturnMessage.Add("اطلاعات کاربری معتبر نمیاشد");
                    return CreateErrorResponse(postedViewModel.ActionResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, postedViewModel);
        }

        [Route("logOff"), HttpPut]
        public HttpResponseMessage LogOff(HttpRequestMessage request, RegisterViewModel postedViewModel)
        {
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            postedViewModel.ActionResult = new BusinessResult();
            postedViewModel.ActionResult.ReturnStatus = true;
            postedViewModel.ReturnUrl = "/account/login";
            //PermissionManager.Instance.SignOut(User.Identity.GetUserId<int>());

            return Request.CreateResponse(HttpStatusCode.OK, postedViewModel);
        }

        [Route("edit"), HttpPost]
        public async Task<HttpResponseMessage> Edit(HttpRequestMessage request, RegisterViewModel postedViewModel)
        {
            var user = new ApplicationUser
            {
                Id = postedViewModel.Id,
                UserName = postedViewModel.UserName,
                Email = postedViewModel.Email,
                Name = postedViewModel.Name,
                LastName = postedViewModel.LastName,
                PhoneNumber = postedViewModel.MobileNumber,

            };
            postedViewModel.Roles.ForEach((item) =>
            {
                IdentityUserRole<int> role = new IdentityUserRole<int>();
                role.RoleId = item.Id;
                user.Roles.Add(role);
            });
            var result = await userManager.UpdateAsync(user);



            postedViewModel.ActionResult.ReturnStatus = result.Succeeded;
            if (postedViewModel.ActionResult.ReturnStatus == false)
            {
                postedViewModel.ActionResult.ReturnMessage.AddRange(result.Errors);
                return CreateErrorResponse(postedViewModel.ActionResult);
            }
            postedViewModel.ActionResult.ReturnMessage.Add("اطلاعات کاربری با موفقیت ویرایش شد");
            postedViewModel.Id = user.Id;

            return Request.CreateResponse<RegisterViewModel>(HttpStatusCode.OK, postedViewModel); ;
        }


        //[Route("delete"), HttpPost]
        //public HttpResponseMessage Delete(HttpRequestMessage request, [FromBody]TViewModel postedViewModel)
        //{
        //    InitialBusinessService();
        //    var mapper = GetMapper();
        //    TEntity entityDeleteing = mapper.Map<TViewModel, TEntity>(postedViewModel);
        //    Delete(entityDeleteing, postedViewModel);

        //    postedViewModel.transactionalData = TransactionalData;
        //    if (TransactionalData.ReturnStatus == false)
        //    {
        //        var responseError = Request.CreateResponse<TViewModel>(HttpStatusCode.BadRequest, postedViewModel);
        //        return responseError;
        //    }

        //    var response = Request.CreateResponse<TViewModel>(HttpStatusCode.OK, postedViewModel);
        //    return response;
        //}
        #endregion

        #region [ Role ]

        [Route("getRoles"), HttpGet]
        public HttpResponseMessage GetRoles(HttpRequestMessage request)
        {

            bool loadRoleSystem = appSettingManager.GetValue<bool>("loadRoleSystem").Value;
            var entities = roleBusinessService.EntityListLoader.LoadList(where: x => x.IsUsingBySystem == false || loadRoleSystem);

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            List<RoleViewModel> dataSource = mapper.Map<List<NeutrinoRole>, List<RoleViewModel>>(entities.ResultValue);

            return CreateSuccessedListResponse<RoleViewModel>(dataSource);
        }

        [Route("getRoleInfo"), HttpGet]
        public async Task<HttpResponseMessage> GetRoleInfo(HttpRequestMessage request, int roleId)
        {
            var entity = await roleBusinessService.EntityLoader.LoadAsync(roleId);

            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataViewModel = mapper.Map<NeutrinoRole, RoleViewModel>(entity.ResultValue);
            return CreateViewModelResponse(dataViewModel, entity);
        }

        [Route("getRoleDataGrid"), HttpPost]
        public async Task<HttpResponseMessage> GetRoleDataGrid(HttpRequestMessage request, [FromBody]JQueryDataTablesModel dataTablesModel)
        {
            bool loadRoleSystem = appSettingManager.GetValue<bool>("loadRoleSystem").Value;
            var entities = await roleBusinessService.EntityListByPagingLoader.LoadAsync(where: x => (x.IsUsingBySystem == false || loadRoleSystem)
            && (x.FaName.Contains(dataTablesModel.sSearch)
            || x.Name.Contains(dataTablesModel.sSearch)
            || dataTablesModel.sSearch == "")
            );

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            List<RoleViewModel> dataSource = mapper.Map<List<NeutrinoRole>, List<RoleViewModel>>(entities.ResultValue);


            return Request.CreateResponse(HttpStatusCode.OK
                  , DataTablesJson(items: dataSource
                  , totalRecords: entities.TotalRows
                  , totalDisplayRecords: entities.TotalRows
                  , sEcho: dataTablesModel.sEcho));

        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccountMapperProfile());
            });
            return config.CreateMapper();
        }
       
        #endregion



    }
}
