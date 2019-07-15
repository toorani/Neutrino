using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
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
        private readonly AbstractValidator<List<MemberSharePromotion>> lstValidationRules;


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

                entities.ForEach(memshar =>
                {
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

                entities.ForEach(memshar =>
                {
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
        public async Task<IBusinessResultValue<PromotionReviewStatusEnum>> ProceedMemberSharePromotionAsync(PromotionReviewStatusEnum currentStep, PromotionReviewStatusEnum nextStep, int branchId)
        {
            var result = new BusinessResultValue<PromotionReviewStatusEnum>();
            try
            {
                var entity = await unitOfWork.BranchPromotionDataService.GetQuery()
                    .IncludeFilter(x => x.MemberSharePromotions.Where(y => y.Deleted == false))
                    .IncludeFilter(x => x.MemberSharePromotions.Select(y => y.Details.Where(c => c.Deleted == false)))
                    .FirstOrDefaultAsync(x => x.BranchId == branchId && x.PromotionReviewStatusId == currentStep);
                if (entity != null)
                {
                    var salesAssigned = entity.MemberSharePromotions.Sum(x => x.Details.Sum(c => (decimal?)c.BranchSalesPromotion)) ?? 0;
                    var receiptAssigned = entity.MemberSharePromotions.Sum(x => x.Details.Sum(c => (decimal?)c.ReceiptPromotion)) ?? 0;
                    var compensatoryAssigned = entity.MemberSharePromotions.Sum(x => x.Details.Sum(c => (decimal?)c.CompensatoryPromotion)) ?? 0;

                    if (salesAssigned > entity.TotalSalesPromotion)
                    {
                        result.ReturnMessage.Add("جمع مبلغ پورسانت تامین کننده پرسنل از پورسانت فروش تامین کنندگان مرکز بیشتر میباشد");
                        result.ReturnStatus = false;
                    }
                    if (receiptAssigned > entity.PrivateReceiptPromotion + entity.TotalReceiptPromotion)
                    {
                        result.ReturnMessage.Add("جمع مبلغ پورسانت وصول پرسنل از پورسانت وصول مرکز بیشتر میباشد");
                        result.ReturnStatus = false;
                    }
                    if (compensatoryAssigned > entity.CompensatoryPromotion)
                    {
                        result.ReturnMessage.Add("جمع مبلغ پورسانت ترمیمی پرسنل از پورسانت ترمیمی مرکز بیشتر میباشد");
                        result.ReturnStatus = false;
                    }

                    if (result.ReturnStatus)
                    {
                        entity.PromotionReviewStatusId = nextStep;
                        result.ResultValue = nextStep;
                        unitOfWork.BranchPromotionDataService.Update(entity);
                        await unitOfWork.CommitAsync();
                        result.ReturnMessage.Add("اطلاعات تایید و ارسال شد");
                    }
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
        public async Task<IBusinessResultValue<MemberSharePromotion>> LoadMemberSharePromotionAsync(int memberId, int month, int year, SharePromotionTypeEnum sharePromotionTypeId)
        {
            var result = new BusinessResultValue<MemberSharePromotion>();
            try
            {
                result.ResultValue = await unitOfWork.MemberSharePromotionDataService.FirstOrDefaultAsync(x => x.MemberId == memberId
               && x.BranchPromotion.Month == month && x.BranchPromotion.Year == year
               && x.Details.Any(y => y.SharePromotionTypeId == sharePromotionTypeId)
               , includes: x => new { x.BranchPromotion, x.Details });

                if (result.ResultValue == null && sharePromotionTypeId == SharePromotionTypeEnum.Manager)
                {
                    var sellerTotalPromtion = await (from mep in unitOfWork.MemberPromotionDataService.GetQuery()
                                                     join brp in unitOfWork.BranchPromotionDataService.GetQuery()
                                                     on mep.BranchPromotionId equals brp.Id
                                                     where mep.MemberId == memberId && brp.Month == month && brp.Year == year && mep.Deleted == false
                                                     group mep by mep.MemberId into grp_memp
                                                     select grp_memp.Sum(x => x.Promotion)).FirstOrDefaultAsync();

                    MemberSharePromotion memberSharePromotion = new MemberSharePromotion { MemberId = memberId };
                    memberSharePromotion.Details.Add(new MemberSharePromotionDetail { CompensatoryPromotion = sellerTotalPromtion });
                    result.ResultValue = memberSharePromotion;
                }
                else
                {
                    result.ResultValue = new MemberSharePromotion { MemberId = memberId };
                }

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<List<MemberSharePromotion>>> LoadMemberSharePromotionListAsync(int branchId, int month, int year, SharePromotionTypeEnum sharePromotionTypeId)
        {
            var result = new BusinessResultValue<List<MemberSharePromotion>>();
            try
            {

                var branchPromotion = await unitOfWork.BranchPromotionDataService.FirstOrDefaultAsync(x => x.Month == month && x.Year == year && x.BranchId == branchId);
                int branchPromotionId = branchPromotion.Id;



                var query = await (from me in unitOfWork.MemberDataService.GetQuery()
                                   join post in unitOfWork.PositionTypeDataService.GetQuery()
                                   on me.PositionTypeId equals post.eId into left_join_position
                                   from join_me_pos in left_join_position.DefaultIfEmpty()
                                   join msp in unitOfWork.MemberSharePromotionDataService.GetQuery()
                                   on me.Id equals msp.MemberId into left_join_memShare
                                   from join_memShare in left_join_memShare
                                   .Where(x => x.Deleted == false && x.BranchPromotionId == branchPromotionId)
                                   .DefaultIfEmpty()
                                   join mshde in unitOfWork.MemberSharePromotionDetailDataService.GetQuery()
                                   on join_memShare.Id equals mshde.MemberSharePromotionId into left_join_memberSh_detail
                                   from join_meShDeatil in left_join_memberSh_detail
                                   .Where(x => x.SharePromotionTypeId == SharePromotionTypeEnum.Manager)
                                   .DefaultIfEmpty()


                                   where me.BranchId == branchId && me.Deleted == false
                                   select new
                                   {
                                       MemberId = me.Id,
                                       Member = me,
                                       join_me_pos,
                                       join_memShare,
                                       join_meShDeatil
                                   }).ToListAsync();

                result.ResultValue = query.Select(x =>
                {
                    if (x.join_memShare != null)
                    {
                        return x.join_memShare;
                    }
                    MemberSharePromotion memberSharePromotion = new MemberSharePromotion
                    {
                        MemberId = x.MemberId,
                        Member = x.Member,
                        BranchPromotionId = branchPromotionId
                    };
                    //if (x.sellerTotalPromtion.HasValue)
                    //    memberSharePromotion.Details.Add(new MemberSharePromotionDetail { MemberId = x.MemberId, CompensatoryPromotion = x.sellerTotalPromtion.Value, SharePromotionTypeId = sharePromotionTypeId });
                    return memberSharePromotion;
                }).ToList();

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResult> CreateOrUpdateAsync(List<MemberSharePromotion> entities)
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

                entities.ForEach(x => x.Member = null);

                var branchPromotionId = entities.First().BranchPromotionId;
                var lst_existEntities = await unitOfWork.MemberSharePromotionDataService
                    .GetQuery()
                    .Include(x => x.Details)
                    .Where(x => x.BranchPromotionId == branchPromotionId && x.Deleted == false)
                    .AsNoTracking()
                    .ToListAsync();

                var lst_newEntities = entities.Except(lst_existEntities, x => x.MemberId);
                foreach (var item in lst_newEntities.Where(x => x.ManagerPromotion != 0))
                    unitOfWork.MemberSharePromotionDataService.Insert(item);


                var lst_intersectEntities = entities.Intersect(lst_existEntities, x => x.MemberId);

                foreach (var item in lst_intersectEntities.Where(x => x.ManagerPromotion == 0))
                {
                    var exist_item = lst_existEntities.Single(x => x.MemberId == item.MemberId);
                    var arr_details = new MemberSharePromotionDetail[exist_item.Details.Count];
                    exist_item.Details.CopyTo(arr_details, 0);
                    foreach (var detail in arr_details)
                        unitOfWork.MemberSharePromotionDetailDataService.Delete(detail, false);

                    unitOfWork.MemberSharePromotionDataService.Delete(exist_item, false);

                }

                foreach (var item in lst_intersectEntities.Where(x => x.ManagerPromotion != 0))
                {
                    var exist_item = lst_existEntities.Single(x => x.MemberId == item.MemberId);

                    exist_item.CEOPromotion = item.CEOPromotion;
                    exist_item.FinalPromotion = item.FinalPromotion;
                    exist_item.ManagerPromotion = item.ManagerPromotion;

                    foreach (var detail in item.Details)
                    {
                        var exist_item_detail = exist_item.Details.FirstOrDefault(x => x.MemberId == detail.MemberId
                        && x.Deleted == false && x.SharePromotionTypeId == SharePromotionTypeEnum.Manager);

                        if (exist_item_detail != null)
                        {
                            exist_item_detail.ReceiptPromotion = detail.ReceiptPromotion;
                            exist_item_detail.CompensatoryPromotion = detail.CompensatoryPromotion;
                            exist_item_detail.BranchSalesPromotion = detail.BranchSalesPromotion;
                            unitOfWork.MemberSharePromotionDetailDataService.Update(exist_item_detail);
                        }
                    }
                    unitOfWork.MemberSharePromotionDataService.Update(exist_item);
                }


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
    }

    internal static class Extentions
    {
        public static IEnumerable<T> Intersect<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other,
                                                                            Func<T, TKey> getKey)
        {
            return from item in items
                   join otherItem in other
                   on getKey(item) equals getKey(otherItem)
                   select item;
        }
    }
}
