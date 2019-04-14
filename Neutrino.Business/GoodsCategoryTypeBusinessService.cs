
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
    public class GoodsCategoryTypeBusinessService : NeutrinoBSBase<GoodsCategoryType, IGoodsCategoryType>
    {
        
        #region [ Constructor(s) ]
        public GoodsCategoryTypeBusinessService(ITransactionalData transactionalData)
            :base(transactionalData)
        {
        }
        public GoodsCategoryTypeBusinessService()
            : base()
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<int> AddGoodsCategoryTypesAsync(List<GoodsCategoryType> lstGoodsCategoryTypes)
        {
            return await dataRepository.InsertBulkAsync(lstGoodsCategoryTypes);
        }
        #endregion


    }
}
