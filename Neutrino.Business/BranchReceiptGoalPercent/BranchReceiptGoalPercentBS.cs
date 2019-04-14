using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class BranchReceiptGoalPercentBS : NeutrinoBusinessService, IBranchReceiptGoalPercentBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<BranchReceiptGoalPercentDTO> validator;
        #endregion

        #region [ Constructor(s) ]
        public BranchReceiptGoalPercentBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<BranchReceiptGoalPercentDTO> validator)
            :base(unitOfWork)
        {
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<BranchReceiptGoalPercentDTO>> CreateOrUpdateAsync(BranchReceiptGoalPercentDTO branchReceiptGoalPercentDTO)
        {
            var result = new BusinessResultValue<BranchReceiptGoalPercentDTO>();
            try
            {
                var result_validator = validator.Validate(branchReceiptGoalPercentDTO);
                if (result_validator.IsValid == false)
                {
                    result.PopulateValidationErrors(result_validator.Errors);
                    return result;
                }

                var lst_BranchIds = new List<int>();
                lst_BranchIds.AddRange(branchReceiptGoalPercentDTO.Branches);
                lst_BranchIds.AddRange(branchReceiptGoalPercentDTO.DeselectedBranches);
                //load exist data
                var existData = await unitOfWork.BranchReceiptGoalPercentDataService.GetAsync(x => x.GoalId == branchReceiptGoalPercentDTO.GoalId &&
                    lst_BranchIds.Contains(x.BranchId));

                //delete deselected branches
                var lst_deselectedBranches = existData.Where(x => branchReceiptGoalPercentDTO.DeselectedBranches.Contains(x.BranchId)).ToList();
                lst_deselectedBranches.ForEach(x =>
                {
                    unitOfWork.BranchReceiptGoalPercentDataService.Delete(x);
                });

                //update branches
                var lst_update_branchePercent = existData.Where(x => branchReceiptGoalPercentDTO.Branches.Contains(x.BranchId)).ToList();
                lst_update_branchePercent.ForEach(x =>
                {
                    x.NotReachedPercent = branchReceiptGoalPercentDTO.NotReachedPercent;
                    x.ReachedPercent = branchReceiptGoalPercentDTO.ReachedPercent;
                    unitOfWork.BranchReceiptGoalPercentDataService.Update(x);
                });

                //add new branch percent
                var lst_insert_branchePercent = branchReceiptGoalPercentDTO.Branches.Where(x => !existData.Any(y => y.BranchId == x)).ToList();
                lst_insert_branchePercent.ForEach(x =>
                {
                    BranchReceiptGoalPercent newEntity = new BranchReceiptGoalPercent();
                    newEntity.BranchId = x;
                    newEntity.GoalId = branchReceiptGoalPercentDTO.GoalId;
                    newEntity.NotReachedPercent = branchReceiptGoalPercentDTO.NotReachedPercent;
                    newEntity.ReachedPercent = branchReceiptGoalPercentDTO.ReachedPercent;
                    unitOfWork.BranchReceiptGoalPercentDataService.Insert(newEntity);
                });

                await unitOfWork.CommitAsync();
                result.ResultValue = branchReceiptGoalPercentDTO;
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> DeleteAsync(BranchReceiptGoalPercentDTO branchReceiptGoalPercentDTO)
        {
            var result = new BusinessResult();
            try
            {
                var lst_itemDeleted = await unitOfWork.BranchReceiptGoalPercentDataService.GetAsync(x => branchReceiptGoalPercentDTO.Branches.Contains(x.BranchId)
                && x.GoalId == branchReceiptGoalPercentDTO.GoalId
                && x.NotReachedPercent == branchReceiptGoalPercentDTO.NotReachedPercent
                && x.ReachedPercent == branchReceiptGoalPercentDTO.ReachedPercent
                );

                lst_itemDeleted.ForEach(x =>
                {
                    unitOfWork.BranchReceiptGoalPercentDataService.Delete(x);
                });
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_DELETE_ENTITY);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<Branch>>> LoadNotSpecifiedBranchesAsync(int goalId)
        {
            var result = new BusinessResultValue<List<Branch>>();
            try
            {
                result.ResultValue = await unitOfWork.BranchReceiptGoalPercentDataService.GetNotSpecifiedBranchesAsync(goalId);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<Goal>> LoadReceiptGoalAsync(int goalId)
        {
            var result = new BusinessResultValue<Goal>();
            try
            {
                result.ResultValue = await unitOfWork.BranchReceiptGoalPercentDataService.GetReceiptGoalAsync(goalId);
                if (result.ResultValue == null)
                {
                    result.ReturnMessage.Add("اطلاعاتی یافت نشد");
                    result.ReturnStatus = false;
                }
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
