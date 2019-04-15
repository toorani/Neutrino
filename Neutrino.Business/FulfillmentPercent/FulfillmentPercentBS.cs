using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class FulfillmentPercentBS : NeutrinoBusinessService, IFulfillmentPercentBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<List<FulfillmentPercent>> validator;
        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPercentBS(NeutrinoUnitOfWork unitOfWork,
            AbstractValidator<List<FulfillmentPercent>> validator):base(unitOfWork)
        {
            this.validator = validator;
        }

        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<List<FulfillmentPercent>>> LoadFulfillmentListAsync(int year, int month)
        {
            var result = new BusinessResultValue<List<FulfillmentPercent>>();
            try
            {
                result.ResultValue = await unitOfWork.FulfillmentPercentDataService.GetFulfillmentListAsync(year,month);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        
        public async Task<IBusinessResultValue<List<FulfillmentPercent>>> SubmitDataAsync(List<FulfillmentPercent> lstGoalFulfillment)
        {
            var result = new BusinessResultValue<List<FulfillmentPercent>>();
            try
            {
                lstGoalFulfillment = lstGoalFulfillment
                    .Where(x => x.ManagerFulfillmentPercent != 0 || x.SellerFulfillmentPercent != 0)
                    .ToList();

                ValidationResult validatorResult = validator.Validate(lstGoalFulfillment);
                if (validatorResult.IsValid == false)
                {
                    result.PopulateValidationErrors(validatorResult.Errors);
                    return result;
                }


                unitOfWork.FulfillmentPercentDataService.InsertFulfillment(lstGoalFulfillment);
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ResultValue = lstGoalFulfillment;
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
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
