using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IPermissionBS : IBusinessService
    {
        Task<IBusinessResult> CreateOrModifyPermissionAsync(Permission permission);
        Task<IBusinessResultValue<List<Permission>>> LoadRolePermission(int roleId);
    }
}
