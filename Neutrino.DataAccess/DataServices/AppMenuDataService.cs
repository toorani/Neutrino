using Espresso.DataAccess;
using Neutrino.Entities;
using Espresso.DataAccess.Interfaces;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class AppMenuDataService : RepositoryBase<AppMenu>, IAppMenu
    {
        #region [ Constructor(s) ]
        public AppMenuDataService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }
        #endregion

    }
}
