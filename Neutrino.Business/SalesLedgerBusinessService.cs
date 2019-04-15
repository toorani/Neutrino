
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
    public class SalesLedgerBusinessService : NeutrinoBSBase<SalesLedger,ISalesLedger>
    {
        #region [ Constructor(s) ]
        public SalesLedgerBusinessService(ITransactionalData transactionalData)
            :base(transactionalData)
        {

        }
        public SalesLedgerBusinessService()
            : base()
        {

        }

        #endregion
    }
}
