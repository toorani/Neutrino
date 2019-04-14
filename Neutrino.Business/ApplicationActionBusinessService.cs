using Espresso.Core;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class ApplicationActionBusinessService : NeutrinoBSBase<ApplicationAction, IApplicationAction>
    {
        #region [ Constructor(s) ]
        public ApplicationActionBusinessService(ITransactionalData transactionalData)
            : base(transactionalData)
        {

        }
        public ApplicationActionBusinessService()
            : base()
        {

        }
        #endregion
        
    }
}
