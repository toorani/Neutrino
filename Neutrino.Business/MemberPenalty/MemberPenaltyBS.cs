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



        public async Task<IBusinessResultValue<List<MemberPenaltyDTO>>> LoadPenaltiesForPromotionAsync(int branchId)
        {
            var result = new BusinessResultValue<List<MemberPenaltyDTO>>();
            try
            {
                var branchPromotion = await unitOfWork.BranchPromotionDataService.FirstOrDefaultAsync(x => x.BranchId == branchId && x.PromotionReviewStatusId == PromotionReviewStatusEnum.ReleasedStep1ByBranchManager);


                if (branchPromotion != null)
                {
                    var lst_memberPenalties = await unitOfWork.MemberPenaltyDataService.GetAsync(x => x.BranchPromotionId == branchPromotion.Id
                    && x.BranchPromotion.PromotionReviewStatusId == PromotionReviewStatusEnum.DeterminedPromotion
                    && x.BranchPromotion.Month < branchPromotion.Month && x.BranchPromotion.Year < branchPromotion.Year
                    , includes: x => x.BranchPromotion
                    , orderBy: x => x.OrderByDescending(z => z.BranchPromotion.Year).OrderByDescending(z => z.BranchPromotion.Month));

                    var query = await (from memshar in unitOfWork.MemberPromotionDataService.GetQuery().Include(x => x.Member)
                                       join mempln in unitOfWork.MemberPenaltyDataService.GetQuery()
                                       on memshar.Id equals mempln.MemberPromotionId into leftjoin_share_penalty
                                       from share_penalty in leftjoin_share_penalty.Where(x => x.Deleted == false).DefaultIfEmpty()
                                       where memshar.BranchPromotionId == branchPromotion.Id
                                       select new
                                       {
                                           memshar,
                                           memshar.Member,
                                           penalty = share_penalty ?? null
                                       }).ToListAsync();




                    result.ResultValue = query.Select(x =>
                    {
                        //آخرین رکورد جریمه پرسنل
                        var lastMemberPenalty = lst_memberPenalties.FirstOrDefault(c => c.MemberId == x.Member.Id);

                        return new MemberPenaltyDTO
                        {
                            MemberName = x.Member.Name + " " + x.Member.LastName,
                            MemberId = x.memshar.MemberId,
                            ManagerPromotion = x.memshar.Promotion,
                            BranchPromotionId = branchPromotion.Id,
                            Deduction = x.penalty?.Deduction ?? 0,
                            MemberPromotionId = x.memshar.Id,
                            Penalty = x.penalty?.Penalty ?? 0,
                            Credit = x.penalty?.Credit ?? 0,
                            RemainingPenalty = lastMemberPenalty != null ? lastMemberPenalty.RemainingPenalty + lastMemberPenalty.Penalty : x.penalty?.RemainingPenalty ?? 0,
                            Description = x.penalty?.Description ?? "",
                            Id = x.penalty?.Id ?? 0,
                            HasPerviousData = lastMemberPenalty != null,
                            Saved = x.penalty?.Saved ?? 0
                        };
                    }).ToList();
                }




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
                    .IncludeFilter(x => x.MemberPromotions.Where(c => c.Deleted == false))
                    .IncludeFilter(x => x.MemberPromotions.Select(c => c.Details.Where(cf => cf.Deleted == false)))
                   .SingleOrDefaultAsync(x => x.Id == branchPromotionId);
                if (branchPromotion != null)
                {
                    foreach (var item in branchPromotion.MemberPromotions)
                    {
                        var penalty = entities.SingleOrDefault(x => x.MemberId == item.MemberId);

                        item.Promotion = penalty?.CEOPromotion ?? 0;
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