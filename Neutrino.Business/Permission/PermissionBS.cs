using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Business
{
    public class PermissionBS : NeutrinoBusinessService, IPermissionBS
    {
        #region [ Constructor(s) ]
        public PermissionBS(NeutrinoUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResult> CreateOrModifyPermissionAsync(Permission permission)
        {
            var result = new BusinessResult();

            try
            {
                var lst_existPermissions = await unitOfWork.PermissionDataService.GetAsync(x => x.RoleId == permission.RoleId
                , includes: x => x.ApplicationAction);

                var lst_newAppActions = await unitOfWork.ApplicationActionDataService
                    .GetAsync(x => permission.Urls.Contains(x.HtmlUrl));

                var lst_existAppActions = lst_existPermissions.Select(x => x.ApplicationAction);

                var lst_removeAppActions = lst_existAppActions.Except(lst_newAppActions, x => x.Id);
                var lst_insertAppActions = lst_newAppActions.Except(lst_existAppActions, x => x.Id);

                (from revAct in lst_removeAppActions
                 join extPermission in lst_existPermissions
                 on revAct.Id equals extPermission.ApplicationActionId
                 select extPermission)
                .ToList()
                .ForEach(extPermission =>
                {
                    unitOfWork.PermissionDataService.Delete(extPermission);
                });

                foreach (var act in lst_insertAppActions)
                {
                    unitOfWork.PermissionDataService.Insert(new Permission
                    {
                        RoleId = permission.RoleId,
                        ApplicationActionId = act.Id
                    });
                }

                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add("سطوح دسترسی برای نقش اننخاب شده با موفقیت تعریف شد");
            }
            catch (DbEntityValidationException ex)
            {
                result.PopulateValidationErrors(ex);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;

        }
        
        public async Task<IBusinessResultValue<List<Permission>>> LoadRolePermission(int roleId)
        {
            var result = new BusinessResultValue<List<Permission>>();
            try
            {
                result.ResultValue = await unitOfWork.PermissionDataService.GetAsync(x => x.RoleId == roleId
                , includes: x => x.ApplicationAction);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        #endregion


    }
}
