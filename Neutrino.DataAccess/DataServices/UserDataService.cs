using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class UserDataService : NeutrinoRepositoryBase<NeutrinoUser>, INeutrinoUserDS
    {
        #region [ Constructor(s) ]
        public UserDataService(NeutrinoContext context)
            : base(context)
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<NeutrinoUser> GetUserAsync(int userId)
        {
            return await DataSet
                .IncludeFilter(x => x.UserRoles.Where(y => !y.Deleted))
                .IncludeFilter(x => x.UserRoles.Where(y => !y.Deleted)
                    .Select(y => y.Role))
                .SingleOrDefaultAsync(x => x.Id == userId);

        }
        #endregion
    }
}
