using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class GoodsCategoryDataService : RepositoryBase<GoodsCategory>, IGoodsCategory
    {
        #region [ Constructor(s) ]
        public GoodsCategoryDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion
    }
}
