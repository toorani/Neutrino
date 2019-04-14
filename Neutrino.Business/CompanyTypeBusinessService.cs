
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
    public class CompanyTypeBusinessService : NeutrinoBSBase<CompanyType,ICompanyType>
    {
        #region [ Constructor(s) ]
        public CompanyTypeBusinessService(ITransactionalData transactionalData)
            :base(transactionalData)
        {

        }
        public CompanyTypeBusinessService()
            : base()
        {

        }

        #endregion
    }
}
