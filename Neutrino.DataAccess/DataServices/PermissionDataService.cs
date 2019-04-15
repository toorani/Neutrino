using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Espresso.Identity.Models;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class PermissionDataService : NeutrinoRepositoryBase<Permission>, IPermissionDS
    {
        #region [ Constructor(s) ]
        public PermissionDataService(NeutrinoContext context)
            : base(context)
        {
        }

        #endregion
        public Task<List<Permission>> GetUserAccessAsync(int userId)
        {
            
            var roleIds = dbContext.Users.Where(x => x.Id == userId)
                .SelectMany(x => x.UserRoles)
                .Select(x => x.RoleId);
            var result = dbContext.Permissions
                .Where(x => roleIds.Contains(x.RoleId) && x.Deleted == false)
                .Include(x => x.ApplicationAction)
                .ToListAsync();

            return result;
        }

        public List<Permission> GetUserAccess(int userId)
        {
            var roleIds = dbContext.Users.Where(x => x.Id == userId)
               .SelectMany(x => x.UserRoles)
               .Select(x => x.RoleId);
            return dbContext.Permissions
                .Where(x => roleIds.Contains(x.RoleId) && x.Deleted == false)
                .Include(x => x.ApplicationAction)
                .ToList();
        }

    }
}
