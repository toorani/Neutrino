using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using FluentValidation;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Z.EntityFramework.Plus;

namespace Neutrino.Interfaces
{
    public class GoalNonFulfillmentPercentBS : NeutrinoBusinessService, IGoalNonFulfillmentPercentBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<GoalNonFulfillmentPercent> validator;
        #endregion

        public GoalNonFulfillmentPercentBS(NeutrinoUnitOfWork unitOfWork,
            AbstractValidator<GoalNonFulfillmentPercent> validator
            ) 
            : base(unitOfWork)
        {
            this.validator = validator;
        }

        public async Task<IBusinessResultValue<GoalNonFulfillmentPercent>> CreateOrUpdateAsync(GoalNonFulfillmentPercent entity)
        {
            var result = new BusinessResultValue<GoalNonFulfillmentPercent>();
            try
            {
                if (validator != null)
                {
                    var result_validate = validator.Validate(entity);
                    if (result_validate.IsValid == false)
                    {
                        result.PopulateValidationErrors(result_validate.Errors);
                        return result;
                    }
                }

                if (entity.Id == 0) // insert
                {
                    unitOfWork.GoalNonFulfillmentPercentDataService.Insert(entity);
                }
                else
                {
                    var existBranches = await unitOfWork.GoalNonFulfillmentBranchDataService
                        .GetQuery()
                        .AsNoTracking()
                        .Where(x => x.GoalNonFulfillmentPercentId == entity.Id && x.Deleted == false)
                        .ToListAsync();
                    var deleteBranches = existBranches.Except(entity.GoalNonFulfillmentBranches, x => x.BranchId);
                    foreach (var delBranch in deleteBranches)
                    {
                        delBranch.Deleted = true;
                        entity.GoalNonFulfillmentBranches.Add(delBranch);
                    }
                    unitOfWork.GoalNonFulfillmentPercentDataService.Update(entity);
                }

                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
                result.ResultValue = entity;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<Goal>> LoadGoalAsync(int goalId)
        {
            var result = new BusinessResultValue<Goal>();
            try
            {
                result.ResultValue = await unitOfWork.GoalDataService.GetQuery()
                    .IncludeFilter(x => x.GoalGoodsCategory)
                    .IncludeFilter(x => x.GoalNonFulfillmentPercents.Where(z => z.Deleted == false))
                    .IncludeFilter(x => x.GoalNonFulfillmentPercents.Select(z => z.GoalNonFulfillmentBranches.Where(y => y.Deleted == false)))
                    .IncludeFilter(x => x.GoalNonFulfillmentPercents.Select(z => z.GoalNonFulfillmentBranches.Select(y => y.Branch)))
                    .SingleAsync(x => x.Id == goalId);

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
    }
}
