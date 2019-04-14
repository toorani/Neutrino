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
    public class BranchGoalBS : NeutrinoBusinessService, IBranchGoalBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<BranchGoalDTO> validator;
        #endregion

        #region [ Constructor(s) ]
        public BranchGoalBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<BranchGoalDTO> validator) : base(unitOfWork)
        {
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<BranchGoalDTO>> LoadBranchGoalDTOAsync(int goalId)
        {
            var result = new BusinessResultValue<BranchGoalDTO>();
            try
            {
                var goalEntity = await unitOfWork.GoalDataService.FirstOrDefaultAsync(where: x => x.Id == goalId
                , includes: x => new { x.GoalSteps, x.GoalGoodsCategory });
                result.ResultValue = new BranchGoalDTO();
                result.ResultValue.Items = await unitOfWork.BranchGoalDataService.GetBranchGoalListAsync(goalId);
                result.ResultValue.Goal = goalEntity;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }

            return result;
        }
        public async Task<IBusinessResultValue<BranchGoalDTO>> BatchUpdateAsync(BranchGoalDTO batchData)
        {
            var result = new BusinessResultValue<BranchGoalDTO>();
            try
            {
                ValidationResult validationResult = validator.Validate(batchData);
                if (validationResult.IsValid == false)
                {
                    result.PopulateValidationErrors(validationResult.Errors);
                    return result;
                }
                BranchGoal entity;
                var lstEntities = new List<BranchGoal>();
                batchData.Items
                    .Where(x => (x.Percent != null && x.Percent != 0) || x.Amount != null)
                    .ToList()
                    .ForEach((item) =>
                    {
                        entity = new BranchGoal();

                        if (item.Percent.HasValue)
                            entity.Percent = item.Percent.Value;
                        if (item.Amount.HasValue)
                            entity.Amount = item.Amount.Value;

                        entity.BranchId = item.BranchId;
                        entity.GoalId = item.GoalId;

                        if (item.BranchGoalId.HasValue) // edit
                        {
                            entity.Id = item.BranchGoalId.Value;
                            unitOfWork.BranchGoalDataService.Update(entity);
                        }
                        else
                        {
                            unitOfWork.BranchGoalDataService.Insert(entity);
                        }
                        lstEntities.Add(entity);
                    });

                await unitOfWork.CommitAsync();
                batchData.Items.Where(x => (x.Percent != null && x.Percent != 0) || x.Amount != null)
               .ToList()
               .ForEach(vm =>
               {
                   vm.BranchGoalId = lstEntities.FirstOrDefault(x => x.BranchId == vm.BranchId).Id;
               });
                result.ResultValue = batchData;
                result.ReturnStatus = true;
                result.ReturnMessage.Add(" اطلاعات با موفقیت ثبت گردید");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> BatchDeleteAsync(BranchGoalDTO batchData)
        {
            var result = new BusinessResult();
            try
            {
                var branchBenefitIds = batchData.Items
                        .Where(x => x.BranchGoalId != null)
                        .Select(x => x.BranchGoalId.Value)
                        .ToList();

                var branchBenefits = await unitOfWork.BranchGoalDataService.GetAsync(where: x => branchBenefitIds.Contains(x.Id));
                branchBenefits.ForEach(x => { x.Deleted = true; });

                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add(" اطلاعات با موفقیت حذف شد");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<BranchGoal>> LoadBranchGoalListAsync(params int[] lstGoalIds)
        {
            var result = new BusinessResultValue<BranchGoal>();
            try
            {
                result.ResultValue = await unitOfWork.BranchGoalDataService.FirstOrDefaultAsync(where: x => x.Deleted == false && (lstGoalIds == null || lstGoalIds.Contains(x.GoalId)));
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
