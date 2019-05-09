using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class RoleDataService : NeutrinoRepositoryBase<Role>,INeutrinoRoleDS
    {
        #region [ Constructor(s) ]
        public RoleDataService(NeutrinoContext context) 
            : base(context)
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public Task<List<int>> GetRoleIdsByUserIdAsync(int userId)
        {
            return dbContext.Users
                .Where(user => user.Id == userId)
                .SelectMany(role => role.Roles)
                .Select(r => r.RoleId).ToListAsync();
        }
        #endregion


    }
}
