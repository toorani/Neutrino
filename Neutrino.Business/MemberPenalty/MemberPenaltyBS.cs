using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Linq;
using Z.EntityFramework.Plus;
using System.Data.Entity;
using FluentValidation;

namespace Neutrino.Business
{
    public class MemberPenaltyBS : NeutrinoBusinessService, IMemberPenaltyBS
    {

        private readonly AbstractValidator<List<MemberPenalty>> validationRules;

        public MemberPenaltyBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<List<MemberPenalty>> validationRules) : base(unitOfWork)
        {
            this.validationRules = validationRules;
        }



        public async Task<IBusinessResultValue<List<MemberPenalty>>> LoadPenaltiesForPromotionAsync(int branchId)
        {
            var result = new BusinessResultValue<List<MemberPenalty>>();
            try
            {
                var query = await (from brp in unitOfWork.BranchPromotionDataService.GetQuery()
                                   join memshar in unitOfWork.MemberSharePromotionDataService.GetQuery().Include(x => x.Member)
                                   on brp.Id equals memshar.BranchPromotionId
                                   where memshar.Deleted == false
                                   join mempln in unitOfWork.MemberPenaltyDataService.GetQuery()
                                   on memshar.Id equals mempln.MemberSharePromotionId into leftjoin_share_penalty
                                   from share_penalty in leftjoin_share_penalty.Where(x => x.Deleted == false).DefaultIfEmpty()
                                   where brp.BranchId == branchId && brp.PromotionReviewStatusId == PromotionReviewStatusEnum.ReleadedStep1ByBranchManager
                                   select new
                                   {
                                       memshar,
                                       memshar.Member,
                                       brp,
                                       penalty = share_penalty ?? null
                                   }).ToListAsync();




                result.ResultValue = query.Select(x => new MemberPenalty
                {
                    Member = x.memshar.Member,
                    MemberId = x.memshar.MemberId,
                    BranchPromotion = x.brp,
                    BranchPromotionId = x.brp.Id,
                    Deduction = x.penalty?.Deduction ?? 0,
                    MemberSharePromotion = x.memshar,
                    MemberSharePromotionId = x.memshar.Id,
                    Penalty = x.penalty?.Penalty ?? 0,
                    Credit = x.penalty?.Credit ?? 0,
                    RemainingPenalty = x.penalty?.RemainingPenalty ?? 0,
                    Description = x.penalty?.Description ?? "",
                    Id = x.penalty?.Id ?? 0
                }).ToList();

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<MemberPenalty>>> CreateOrModifyAsync(List<MemberPenalty> entities)
        {
            var result = new BusinessResultValue<List<MemberPenalty>>();
            try
            {
                var result_validation = validationRules.Validate(entities);
                if (result_validation.IsValid == false)
                {
                    result.PopulateValidationErrors(result_validation.Errors);
                    return result;
                }

                entities.ForEach(x =>
                {
                    if (x.Id == 0)
                        unitOfWork.MemberPenaltyDataService.Insert(x);
                    else
                        unitOfWork.MemberPenaltyDataService.Update(x);
                });
                await unitOfWork.CommitAsync();
                result.ResultValue = entities;
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<PromotionReviewStatusEnum>> ReleaseCEOPromotion(List<MemberPenalty> entities)
        {
            var result = new BusinessResultValue<PromotionReviewStatusEnum>();
            try
            {
                var result_validation = validationRules.Validate(entities);
                if (result_validation.IsValid == false)
                {
                    result.PopulateValidationErrors(result_validation.Errors);
                    return result;
                }
                var branchPromotionId = entities.FirstOrDefault().BranchPromotionId;

                var branchPromotion = await unitOfWork.BranchPromotionDataService.GetQuery()
                    .IncludeFilter(x=>x.MemberSharePromotions.Where(c=>c.Deleted == false))
                   .SingleOrDefaultAsync(x => x.Id == branchPromotionId );
                if (branchPromotion != null)
                {
                    var assigendValue = entities.Sum(x => x.CEOPromotion);
                    if (assigendValue != branchPromotion.PrivateReceiptPromotion.Value + branchPromotion.TotalReceiptPromotion.Value + branchPromotion.TotalSalesPromotion.Value)
                    {
                        result.ReturnStatus = false;
                        result.ReturnMessage.Add("مبلغ پورسانت مرکز به طور کامل بین پرسنل تقسیم نشده است");
                        return result;
                    }

                    
                    foreach (var item in branchPromotion.MemberSharePromotions)
                    {
                        var penalty = entities.SingleOrDefault(x => x.MemberId == item.MemberId);
                        item.CEOPromotion = penalty?.CEOPromotion;
                    }
                    branchPromotion.PromotionReviewStatusId = PromotionReviewStatusEnum.ReleasedByCEO;

                    unitOfWork.BranchPromotionDataService.Update(branchPromotion);

                    entities.ForEach(x =>
                    {
                        if (x.Id == 0)
                            unitOfWork.MemberPenaltyDataService.Insert(x);
                        else
                            unitOfWork.MemberPenaltyDataService.Update(x);
                    });

                    await unitOfWork.CommitAsync();
                    result.ResultValue = PromotionReviewStatusEnum.ReleasedByCEO;
                    result.ReturnMessage.Add("اطلاعات تایید و جهت تایید نهایی به رییس مرکز ارسال شد");
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
    }
}