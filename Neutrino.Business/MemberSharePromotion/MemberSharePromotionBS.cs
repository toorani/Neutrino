using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Neutrino.Business
{
    public class MemberSharePromotionBS : NeutrinoBusinessService, IMemberSharePromotionBS
    {
        private readonly AbstractValidator<MemberSharePromotion> validationRules;
        private readonly AbstractValidator<List<MemberSharePromotion>>  lstValidationRules;


        public MemberSharePromotionBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<MemberSharePromotion> memberShareValidator
            , AbstractValidator<List<MemberSharePromotion>> lstValidationRules) : base(unitOfWork)
        {
            validationRules = memberShareValidator;
            this.lstValidationRules = lstValidationRules;
        }

        public async Task<IBusinessResult> AddOrModfiyFinalPromotionAsync(List<MemberSharePromotion> entities)
        {
            var result = new BusinessResult();
            try
            {
                var result_validator = lstValidationRules.Validate(entities);
                if (!result_validator.IsValid)
                {
                    result.PopulateValidationErrors(result_validator.Errors);
                    return result;
                }

                entities.ForEach(memshar => {
                    unitOfWork.MemberSharePromotionDataService.Update(memshar);
                });
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> CreateOrUpdateAsync(MemberSharePromotion entity)
        {
            var result = new BusinessResult();
            try
            {
                var branchPromotion = await unitOfWork.BranchPromotionDataService.FirstOrDefaultAsync(x => x.BranchId == entity.BranchId
                && x.PromotionReviewStatusId == PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview);
                entity.BranchPromotionId = branchPromotion.Id;

                var result_validate = validationRules.Validate(entity);
                if (!result_validate.IsValid)
                {
                    result.PopulateValidationErrors(result_validate.Errors);
                    return result;
                }

                var existEntity = await unitOfWork.MemberSharePromotionDataService.FirstOrDefaultAsync(x => x.MemberId == entity.MemberId && x.BranchPromotionId == branchPromotion.Id);
                if (existEntity != null)
                {
                    existEntity.CEOPromotion = entity.CEOPromotion;
                    existEntity.FinalPromotion = entity.FinalPromotion;
                    existEntity.ManagerPromotion = entity.ManagerPromotion;
                    unitOfWork.MemberSharePromotionDataService.Update(existEntity);
                }
                else
                    unitOfWork.MemberSharePromotionDataService.Insert(entity);

                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);

                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<PromotionReviewStatusEnum>> DeterminedPromotion(List<MemberSharePromotion> entities)
        {
            var result = new BusinessResultValue<PromotionReviewStatusEnum>();
            try
            {
                var result_validator = lstValidationRules.Validate(entities);
                if (!result_validator.IsValid)
                {
                    result.PopulateValidationErrors(result_validator.Errors);
                    return result;
                }

                entities.ForEach(memshar => {
                    unitOfWork.MemberSharePromotionDataService.Update(memshar);
                });

                var branchPromotionId = entities.First().BranchPromotionId;
                var branchPromotion = await unitOfWork.BranchPromotionDataService.GetByIdAsync(branchPromotionId);
                branchPromotion.PromotionReviewStatusId = PromotionReviewStatusEnum.DeterminedPromotion;
                unitOfWork.BranchPromotionDataService.Update(branchPromotion);
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add("پورسانت پرسنل نهایی شد");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<MemberSharePromotion>>> LoadAsync(int branchId, PromotionReviewStatusEnum promotionReviewStatusId)
        {
            var result = new BusinessResultValue<List<MemberSharePromotion>>();
            try
            {
                result.ResultValue = await unitOfWork.MemberSharePromotionDataService.GetAsync(x =>
                x.BranchPromotion.BranchId == branchId &&
                x.BranchPromotion.PromotionReviewStatusId == promotionReviewStatusId
                , includes: x => new { x.BranchPromotion, x.Member });
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> ProceedMemberSharePromotionAsync(PromotionReviewStatusEnum currentStep, PromotionReviewStatusEnum nextStep, int branchId)
        {
            var result = new BusinessResult();
            try
            {
                var entity = await unitOfWork.BranchPromotionDataService.GetQuery()
                    .IncludeFilter(x => x.MemberSharePromotions.Where(y => y.Deleted == false))
                    .FirstOrDefaultAsync(x => x.BranchId == branchId && x.PromotionReviewStatusId == currentStep);
                if (entity != null)
                {
                    var assigendValue = entity.MemberSharePromotions.Sum(x => (decimal?)x.ManagerPromotion) ?? 0;
                    if (assigendValue != entity.PrivateReceiptPromotion.Value + entity.TotalReceiptPromotion.Value + entity.TotalSalesPromotion.Value)
                    {
                        result.ReturnStatus = false;
                        result.ReturnMessage.Add("مبلغ پورسانت مرکز به طور کامل بین پرسنل تقسیم نشده است");
                        return result;
                    }

                    entity.PromotionReviewStatusId = nextStep;

                    unitOfWork.BranchPromotionDataService.Update(entity);
                    await unitOfWork.CommitAsync();
                    result.ReturnMessage.Add("اطلاعات تایید و ارسال شد");
                }
                else
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add("اطلاعات معتبر نمی باشد");
                }
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> RemoveAsync(int branchId, int memberId)
        {
            var result = new BusinessResult();
            try
            {
                var entity = await unitOfWork.MemberSharePromotionDataService.FirstOrDefaultAsync(x => x.MemberId == memberId
                && x.BranchPromotion.BranchId == branchId && x.BranchPromotion.PromotionReviewStatusId == PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview);

                unitOfWork.MemberSharePromotionDataService.Delete(entity);
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_DELETE_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
    }
}
