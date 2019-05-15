using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using FluentValidation;
using FluentValidation.Results;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;

namespace Neutrino.Business
{
    public class ApplicationActionBS : NeutrinoBusinessService, IApplicationActionBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<List<ApplicationAction>> validator;
        #endregion

        #region [ Constructor(s) ]
        public ApplicationActionBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<List<ApplicationAction>> validator) : base(unitOfWork)
        {
            this.validator = validator;
            
            
        }

        [Inject]
        public IEntityListLoader<ApplicationAction> EntityListLoader { get; set ; }

        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResult> CreateOrModify(List<ApplicationAction> applicationActions)
        {
            var result = new BusinessResult();
            try
            {
                ValidationResult validationResult = validator.Validate(applicationActions);
                if (validationResult.IsValid == false)
                {
                    result.PopulateValidationErrors(validationResult.Errors);
                    return result;
                }

                var htmlUrl = applicationActions.FirstOrDefault().HtmlUrl;

                var existData = await unitOfWork.ApplicationActionDataService.GetAsync(x => x.HtmlUrl == htmlUrl);
                var lst_removeActions = existData.Except(applicationActions, x => x.ActionUrl);
                var lst_newActions = applicationActions.Except(existData, x => x.ActionUrl);

                foreach (var item in lst_newActions)
                {
                    unitOfWork.ApplicationActionDataService.Insert(item);
                }
                foreach (var item in lst_removeActions)
                {
                    unitOfWork.ApplicationActionDataService.Delete(item);
                }
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add(" اطلاعات با موفقیت ثبت گردید");
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
