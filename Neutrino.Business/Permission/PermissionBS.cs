﻿using Espresso.Core;
using FluentValidation;
using Neutrino.Interfaces;
using Espresso.Identity.Models;
using Neutrino.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Data.Entity.Validation;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class PermissionBS : NeutrinoBusinessService, IPermissionBS
    {
        #region [ Constructor(s) ]
        public PermissionBS(NeutrinoUnitOfWork unitOfWork):base(unitOfWork)
        {
            
        }

        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResult> ModifyPermissionAsync(List<Permission> lstAddPermissions, List<Permission> lstRemovePermissions)
        {
            var result = new BusinessResult();

            try
            {
                lstAddPermissions.ForEach(x => unitOfWork.PermissionDataService.Insert(x));
                lstRemovePermissions.ForEach(x => unitOfWork.PermissionDataService.Delete(x));
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
        public async Task<IBusinessResult> DeleteAsync(int roleId)
        {
            var result = new BusinessResult();
            try
            {
                List<Permission> permissions = await unitOfWork.PermissionDataService.GetAsync(where: x => x.RoleId == roleId);
                permissions.ForEach(x =>
                {
                    unitOfWork.PermissionDataService.Delete(x);
                });
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add("سطوح دسترسی برای نقش اننخاب شده با موفقیت حذف شد");
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
        #endregion


    }
}
