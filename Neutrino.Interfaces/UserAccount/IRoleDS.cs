using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neutrino.Interfaces
{
    public interface IRoleDS : IEntityBaseRepository<Role>
    {
        Task<List<int>> GetRoleIdsByUserIdAsync(int userId);
    }
}
