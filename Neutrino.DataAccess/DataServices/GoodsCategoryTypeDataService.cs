using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class GoodsCategoryTypeDataService : RepositoryBase<GoodsCategoryType>, IGoodsCategoryType
    {
        #region [ Constructor(s) ]
        public GoodsCategoryTypeDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion
    }
}
