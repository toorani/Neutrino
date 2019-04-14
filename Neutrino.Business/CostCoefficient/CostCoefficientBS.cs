
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
using FluentValidation.Results;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class CostCoefficientBS : NeutrinoBusinessService, ICostCoefficientBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<CostCoefficient> validator;
        #endregion

        #region [ Constructor(s) ]
        public CostCoefficientBS(AbstractValidator<CostCoefficient> validator
            , NeutrinoUnitOfWork unitOfWork):base(unitOfWork)
        {
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<List<CostCoefficient>>> LoadCoefficientList()
        {
            var result = new BusinessResultValue<List<CostCoefficient>>();
            try
            {
                result.ResultValue = await unitOfWork.CostCoefficientDataService.GetCoefficientList();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> AddOrUpdateAsync(CostCoefficient entity)
        {
            var result = new BusinessResult();
            try
            {
                ValidationResult validationResult = validator.Validate(entity);
                if (validationResult.IsValid == false)
                {
                    result.PopulateValidationErrors(validationResult.Errors);
                    return result;
                }
                entity.Records.ForEach(x => unitOfWork.CostCoefficientDataService.InsertOrUpdate(x));

                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add("ثبت اطلاعات با موفقیت انجام گردید");
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
