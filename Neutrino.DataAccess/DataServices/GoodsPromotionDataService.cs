using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class GoodsPromotionDataService : RepositoryBase<GoodsPromotion>, IGoodsPromotion
    {
        #region [ Constructor(s) ]
        public GoodsPromotionDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion

    }
}
