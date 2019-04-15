using FluentValidation;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class NeutrinoValidator<TEntity> : AbstractValidator<TEntity>
    {
        #region [ Protected Property(ies) ]
        protected readonly NeutrinoUnitOfWork unitOfWork;
        #endregion

        #region [ Constructor(s) ]
        public NeutrinoValidator(NeutrinoUnitOfWork unitOfWork)
            : base()
        {
            this.unitOfWork = unitOfWork;
        }
        public NeutrinoValidator()
            :base()
        {

        }
        #endregion

       
    }
}
