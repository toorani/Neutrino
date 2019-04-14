
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.Core;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class GoodsScoreBusinessService : NeutrinoBSBase<GoodsScore, IGoodsScore>
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Constructor(s) ]
        public GoodsScoreBusinessService(ITransactionalData transactionalData)
            : base(transactionalData)
        {
            
        }
        public GoodsScoreBusinessService()
            : base()
        {
            
        }
        #endregion
    }
}
