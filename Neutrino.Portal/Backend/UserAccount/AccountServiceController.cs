using AutoMapper;
using Espresso.BusinessService;
using Espresso.Core;
using Espresso.Portal;
using Espresso.Utilities.Interfaces;
using jQuery.DataTables.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace Neutrino.Portal.WebApiControllers
{
    //[Authorize]
    [RoutePrefix("api/accountService")]
    public class AccountServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationManager authenticationManager;
        private readonly IAppSettingManager appSettingManager;
        private readonly IUserBS userBusinessService;
        private readonly INeutrinoRoleBS roleBusinessService;
        #endregion

        #region [ Public Property(ies) ]

        #endregion

        #region [ Constructor(s) ]
        public AccountServiceController(IAppSettingManager appSettingManager
            , INeutrinoRoleBS roleBusinessService
            , IUserBS userBusinessService
            , ApplicationUserManager userManager
            , ApplicationSignInManager signInManager
            , IAuthenticationManager authenticationManager)
        {
            this.appSettingManager = appSettingManager;
            this.roleBusinessService = roleBusinessService;
            this.userBusinessService = userBusinessService;
            _userManager = userManager;
            _signInManager = signInManager;
            this.authenticationManager = authenticationManager;

        }

        #endregion

        #region [ User Method(s) ]

        [Route("getUsers"), HttpPost]
        public async Task<HttpResponseMessage> GetGridData(JQueryDataTablesModel dataTablesModel)
        {
            var entities = await userBusinessService.LoadAsync(dataTablesModel.sSearch

                , orderBy: UIHelper.GetOrderBy<User, RegisterViewModel>(dataTablesModel.GetSortedColumns())
                , pageNumber: dataTablesModel.iDisplayStart
                , pageSize: dataTablesModel.iDisplayLength);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            var dataSource = mapper.Map<List<User>, List<UserIndexViewModel>>(entities.ResultValue);

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
            RegisterViewModel dataModelView = mapper.Map<User, RegisterViewModel>(entity.ResultValue);
            return CreateViewModelResponse(dataModelView, entity);
        }
        [Route("registerOrModify"), HttpPost]
        public async Task<HttpResponseMessage> RegisterOrModifyAsync(RegisterViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var user = mapper.Map<RegisterViewModel, User>(postedViewModel);
            IdentityResult result = null;
            if (user.Id == 0)
            {
                result = await _userManager.CreateAsync(user, postedViewModel.Password);
            }
            else
            {
                result = await _userManager.UpdateAsync(user);
            }

            postedViewModel.ActionResult = new BusinessResult();
            if (result.Succeeded == false)
            {
                postedViewModel.ActionResult.ReturnMessage.AddRange(result.Errors);
                return CreateErrorResponse(postedViewModel.ActionResult);
            }
            postedViewModel.ActionResult.ReturnMessage.Add("اطلاعات کاربری با موفقیت ثبت شد");
            postedViewModel.Id = user.Id;


            return Request.CreateResponse(HttpStatusCode.OK, postedViewModel);
        }
        [Route("resetPassword"), HttpPost]
        public async Task<HttpResponseMessage> ResetPasswordAsync(RegisterViewModel postedViewModel)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(postedViewModel.Id);
            IdentityResult result = await _userManager.ResetPasswordAsync(postedViewModel.Id, token, postedViewModel.Password);

            postedViewModel.ActionResult = new BusinessResult();
            if (result.Succeeded == false)
            {
                postedViewModel.ActionResult.ReturnMessage.AddRange(result.Errors);
                return CreateErrorResponse(postedViewModel.ActionResult);
            }
            var resetPasswordEmailBody = File.ReadAllText(HostingEnvironment.MapPath("/Backend/UserAccount/RestpasswordTemplate.html"));
            resetPasswordEmailBody = resetPasswordEmailBody.Replace("$DateTime$", Utilities.ToPersianDateTime(DateTime.Now))
                .Replace("$UserName$", postedViewModel.UserName)
                .Replace("$password$", postedViewModel.Password);
            await _userManager.SendEmailAsync(postedViewModel.Id, "تغییر رمز عبور", resetPasswordEmailBody);
            postedViewModel.ActionResult.ReturnMessage.Add("رمز عبور با موفقیت تغییر داده شد");

            return Request.CreateResponse(HttpStatusCode.OK, postedViewModel);
        }

        [Route("login"), HttpPost, AllowAnonymous]
        public async Task<HttpResponseMessage> LoginAsync(HttpRequestMessage request, RegisterViewModel postedViewModel)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(postedViewModel.UserName, postedViewModel.Password, postedViewModel.RememberMe, shouldLockout: false);
            postedViewModel.ActionResult = new BusinessResult();
            switch (result)
            {
                case SignInStatus.Success:

                    postedViewModel.ActionResult.ReturnStatus = true;
                    var user = _userManager.Find(postedViewModel.UserName, postedViewModel.Password);
                    List<string> roles = await _userManager.GetRolesAsync(user.Id) as List<string>;
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

        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> DeleteAsync(RegisterViewModel postedViewModel)
        {
            var user = await _userManager.FindByIdAsync(postedViewModel.Id);
            var result = await _userManager.DeleteAsync(user);

            postedViewModel.ActionResult = new BusinessResult();
            if (result.Succeeded == false)
            {
                postedViewModel.ActionResult.ReturnMessage.AddRange(result.Errors);
                return CreateErrorResponse(postedViewModel.ActionResult);
            }

            postedViewModel.ActionResult.ReturnMessage.Add("کاربر انتخاب شده با موفقیت حذف شد");
            return Request.CreateResponse(HttpStatusCode.OK, postedViewModel);
        }
        #endregion

        #region [ Role ]

        [Route("getRoles"), HttpGet]
        public HttpResponseMessage GetRoles()
        {
            bool loadRoleSystem = appSettingManager.GetValue<bool>("loadRoleSystem").Value;
            var entities = roleBusinessService.EntityListLoader.LoadList(where: x => x.IsUsingBySystem == false || loadRoleSystem);

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            List<RoleViewModel> dataSource = mapper.Map<List<Role>, List<RoleViewModel>>(entities.ResultValue);

            return CreateSuccessedListResponse(dataSource);
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
            var dataViewModel = mapper.Map<Role, RoleViewModel>(entity.ResultValue);
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
            List<RoleViewModel> dataSource = mapper.Map<List<Role>, List<RoleViewModel>>(entities.ResultValue);


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
