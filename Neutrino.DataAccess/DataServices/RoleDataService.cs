using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Espresso.Identity.Models;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class RoleDataService : NeutrinoRepositoryBase<NeutrinoRole>,INeutrinoRoleDS
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
                .SelectMany(role => role.UserRoles)
                .Select(r => r.RoleId).ToListAsync();
        }
        #endregion


    }
}
