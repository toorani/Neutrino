
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
    public class OtherRewardTypeBusinessService : NeutrinoBSBase<OtherRewardType,IOtherRewardType>
    {

        #region [ Protected Property(ies) ]
        protected override AbstractValidator<OtherRewardType> businessRulesService
        {
            get { return null; }
        }
        #endregion

        #region [ Constructor(s) ]
        public OtherRewardTypeBusinessService(ITransactionalData transactionalData)
            :base(transactionalData)
        {

        }
        
        #endregion





    }
}
