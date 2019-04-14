using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;
using FluentValidation;
using FluentValidation.Results;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;
using Ninject;
using Ninject.Extensions.Logging;

namespace Neutrino.Business
{
    public class SimpleEntityModifer<TEntity> : NeutrinoBusinessService, IEntityModifer<TEntity>
        where TEntity : EntityBase, new()
    {
        #region [ Varibale(s) ]
        protected readonly IEntityRepository<TEntity> dataRepository;
        protected readonly AbstractValidator<TEntity> validator;
        #endregion

        #region [ Constructor(s) ]
        public SimpleEntityModifer(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<TEntity> validator = null)
            : base(unitOfWork)
        {
            dataRepository = unitOfWork.GetRepository<TEntity>();
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public virtual IBusinessResult Update(TEntity entity)
        {
            var result = new BusinessResult();

            try
            {
                if (validator != null)
                {
                    ValidationResult results = validator.Validate(entity);

                    bool validationSucceeded = results.IsValid;
                    IList<ValidationFailure> failures = results.Errors;

                    if (validationSucceeded == false)
                    {
                        result.PopulateValidationErrors(failures);
                        return result;
                    }
                }

                dataRepository.Update(entity);
                unitOfWork.Commit();
                result.ReturnStatus = true;
                result.ReturnMessage.Add(".ویرایش اطلاعات با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public virtual async Task<IBusinessResult> UpdateAsync(TEntity entity)
        {
            var result = new BusinessResult();

            try
            {
                if (validator != null)
                {
                    ValidationResult results = await validator.ValidateAsync(entity);

                    bool validationSucceeded = results.IsValid;
                    IList<ValidationFailure> failures = results.Errors;

                    if (validationSucceeded == false)
                    {
                        result.PopulateValidationErrors(failures);
                        return result;
                    }
                }
                dataRepository.Update(entity);
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add(".ویرایش اطلاعات با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        #endregion
    }
}
