
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class GoodsBusinessService : NeutrinoBSBase<Goods,IGoods>
    {
        #region [ Protected Property(ies) ]
        protected override AbstractValidator<Goods> businessRulesService
        {
            get { return null; }
        }
        #endregion

        #region [ Constructor(s) ]
        public GoodsBusinessService(ITransactionalData transactionalData)
            :base(transactionalData)
        {
            
        }
        public GoodsBusinessService()
            : base()
        {

        }
        #endregion

        #region [ Public Method(s) ]
        
        #endregion

    }
}
