using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IPermissionDS : IEntityBaseRepository<Permission>
    {
        Task<List<Permission>> GetUserAccessAsync(int userId);
        List<Permission> GetUserAccess(int userId);
        
    }
}
