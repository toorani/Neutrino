using System.Collections.Generic;
using System.Linq;
using Neutrino.Data.EntityFramework;
using System.Data.Entity;
using Espresso.Identity.Models;

namespace Neutrino.Identity
{
    class UserRolesRepository
    {
        private readonly NeutrinoContext _databaseContext;

        public UserRolesRepository(NeutrinoContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public IList<string> FindByUserId(int userId)
        {
            return _databaseContext.Users
                .Include(x => x.UserRoles.Select(y => y.Role))
                .Where(user => user.Id == userId)
                .SelectMany(role => role.UserRoles)
                .Select(r => r.Role.Name).ToList();
        }

        public List<int> FindIdsByUserIdAsync(int userId)
        {
            var result = _databaseContext.Users.Where(user => user.Id == userId).
               SelectMany(role => role.UserRoles).Select(r => r.Id).ToList();
            return result;
        }

        public List<Role> GetRolesByUserId(int userId)
        {
            var result = _databaseContext.Users.Where(user => user.Id == userId).
               SelectMany(role => role.UserRoles).Select(r => r.Role).ToList();
            return result;
        }
    }
}
