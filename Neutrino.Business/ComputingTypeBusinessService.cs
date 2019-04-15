
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
    public class ComputingTypeBusinessService : NeutrinoBSBase<ComputingType,IComputingType>
    {

        #region [ Protected Property(ies) ]
        protected override AbstractValidator<ComputingType> businessRulesService
        {
            get { return null; }
        }
        #endregion

        #region [ Constructor(s) ]

        public ComputingTypeBusinessService(ITransactionalData transactionalData)
            :base(transactionalData)
        {

        }
        
        #endregion
        
    }
}
