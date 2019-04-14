using Espresso.Core;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class GoodsPromotionBusinessService : NeutrinoBSBase<GoodsPromotion, IGoodsPromotion>
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Constructor(s) ]
        public GoodsPromotionBusinessService(ITransactionalData transactionalData)
            : base(transactionalData)
        {
            
        }
        public GoodsPromotionBusinessService()
            : base()
        {
            
        }
        #endregion
    }
}
