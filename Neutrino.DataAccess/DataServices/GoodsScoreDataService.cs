using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class GoodsScoreDataService : RepositoryBase<GoodsScore>, IGoodsScore
    {
        #region [ Constructor(s) ]
        public GoodsScoreDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion

    }
}
