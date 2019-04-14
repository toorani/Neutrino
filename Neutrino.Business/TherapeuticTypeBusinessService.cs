
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
    public class TherapeuticTypeBusinessService : NeutrinoBSBase<TherapeuticType, ITherapeuticType>
    {
        #region [ Constructor(s) ]
        public TherapeuticTypeBusinessService()
            :base()
        {

        }
        public TherapeuticTypeBusinessService(ITransactionalData transactionalData)
            : base(transactionalData)
        {

        }
        #endregion

    }
}
