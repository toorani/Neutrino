using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;

namespace Neutrino.Business
{
    public class QuantityConditionBS : NeutrinoBusinessService, IQuantityConditionBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<QuantityCondition> validator;
        #endregion

        #region [ Public Property(ies) ]

        #endregion

        #region [ Constructor(s) ]
        public QuantityConditionBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<QuantityCondition> validator)
            :base(unitOfWork)
        {
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<QuantityCondition>> LoadQuantityConditionAsync(int goalId)
        {
            IBusinessResultValue<QuantityCondition> resultValue = new BusinessResultValue<QuantityCondition>();
            try
            {
                resultValue.ResultValue = await unitOfWork.QuantityConditionDataService.GetQuantityConditionAsync(goalId);
                resultValue.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, resultValue, "");
            }

            return resultValue;
        }
        public async Task<IBusinessResultValue<QuantityCondition>> AddOrUpdateQuantityConditionAsync(QuantityCondition entity)
        {
            var result = new BusinessResultValue<QuantityCondition>();
            try
            {
                var validatorResult = await validator.ValidateAsync(entity);
                if (validatorResult.IsValid == false)
                {
                    result.PopulateValidationErrors(validatorResult.Errors);
                    result.ReturnStatus = false;
                    return result;
                }

                unitOfWork.QuantityConditionDataService.InsertOrUpdate(entity);
                await unitOfWork.CommitAsync();
                result.ResultValue = entity;
                result.ReturnStatus = true;
                result.ReturnMessage.Add("ثبت اطلاعات با موفقیت انجام گردید");
            }
            catch (DbEntityValidationException ex)
            {
                result.PopulateValidationErrors(ex);
                result.ReturnStatus = false;
                Logger.Error(ex, "");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;

        }
        public async Task<IBusinessResultValue<List<QuantityCondition>>> LoadQuantityConditionListAsync(List<int> goalIds)
        {
            IBusinessResultValue<List<QuantityCondition>> resultValue = new BusinessResultValue<List<QuantityCondition>>();
            try
            {
                resultValue.ResultValue = await unitOfWork.QuantityConditionDataService.GetQuantityConditionListAsync(goalIds);
                resultValue.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, resultValue, "");
            }

            return resultValue;
        }

        public async Task<IBusinessResultValue<QuantityConditionTypeEnum?>> LoadQuantityConditionTypeAsync(int goalId)
        {
            IBusinessResultValue<QuantityConditionTypeEnum?> resultValue = new BusinessResultValue<QuantityConditionTypeEnum?>();
            try
            {
                var query = await unitOfWork.QuantityConditionDataService.FirstOrDefaultAsync(where: x => x.GoalId == goalId);
                if (query != null)
                {
                    resultValue.ResultValue = query.QuantityConditionTypeId;
                }
                resultValue.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, resultValue, "");
            }

            return resultValue;
        }
        #endregion

    }
}
