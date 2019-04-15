using Espresso.BusinessService;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public abstract class NeutrinoBusinessService : BusinessServiceBase
    {
        protected readonly NeutrinoUnitOfWork unitOfWork;

        #region [ Constructor(s) ]
        public NeutrinoBusinessService(NeutrinoUnitOfWork unitOfWork)
            : base()
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

    }
}
