using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IPermissionBS : IBusinessService
    {
        Task<IBusinessResult> ModifyPermissionAsync(List<Permission> lstAddPermissions, List<Permission> lstRemovePermissions);
        Task<IBusinessResult> DeleteAsync(int roleId);
    }
}
