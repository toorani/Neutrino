using Espresso.DataAccess;
using Neutrino.Entities;
using Espresso.DataAccess.Interfaces;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class ApplicationActionDataService : RepositoryBase<ApplicationAction>, IApplicationAction
    {
        #region [ Constructor(s) ]
        public ApplicationActionDataService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }
        #endregion

    }
}
