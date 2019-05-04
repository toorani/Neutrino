using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;
using Z.EntityFramework.Plus;

namespace Neutrino.Business
{
    public class PromotionBS : NeutrinoBusinessService, IPromotionBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<Promotion> validator;

        /// <summary>
        /// اطلاعات فروش کلی مراکز
        /// </summary>
        class BranchSalesInfo
        {
            public int BranchGoalId { get; set; }
            /// <summary>
            //شناسه مرکز
            /// </summary>
            public int BranchId { get; set; }
            /// <summary>
            //مبلغ کل فروش مرکز
            /// </summary>
            public decimal TotalSales { get; set; }
            /// <summary>
            //تعداد کل فروش مرکز
            /// </summary>
            public int TotalQuantity { get; set; }
            public decimal? SellerFulfillmentPercent { get; set; }
        }
        class AggregationInfo
        {
            public int BranchId { get; set; }
            public decimal AggregationSalesAmount { get; set; }
            public double AggregationSalesNumber { get; set; }
        }
        #endregion

        #region [ Public Property(ies) ]
        [Inject]
        public IEntityListByPagingLoader<Promotion> EntityListByPagingLoader { get; set; }

        [Inject]
        public IEntityLoader<Promotion> EntityLoader { get; set; }

        #endregion

        #region [ Constructor(s) ]
        public PromotionBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<Promotion> validator)
            : base(unitOfWork)
        {
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<Promotion>> AddPromotionAsync(Promotion entity)
        {
            var result = new BusinessResultValue<Promotion>();
            try
            {
                entity.StatusId = PromotionStatusEnum.WaitingForGoalFulfillment;
                var perCal = new PersianCalendar();
                entity.StartDate = perCal.ToDateTime(entity.Year, entity.Month, 1, 0, 0, 0, 0);

                var daysInMonth = perCal.GetDaysInMonth(entity.Year, entity.Month);
                entity.EndDate = perCal.ToDateTime(entity.Year, entity.Month, daysInMonth, 0, 0, 0, 0);

                var valResult = await validator.ValidateAsync(entity);
                if (valResult.IsValid == false)
                {
                    result.PopulateValidationErrors(valResult.Errors);
                    return result;
                }

                var lstGoals = await unitOfWork.GoalDataService.GetAsync(x => x.Month == entity.Month && x.Year == entity.Year
                && x.IsUsed == false
                && (x.GoalGoodsCategoryTypeId != GoalGoodsCategoryTypeEnum.Group
                && x.GoalGoodsCategoryTypeId != GoalGoodsCategoryTypeEnum.Single)
                , includes: x => new { x.BranchGoals, x.GoalSteps });

                var lst_totalFulfillPromotions = await unitOfWork.TotalFulfillPromotionPercentDataService.GetAllAsync();

                //calculate goals fulfillment
                var lst_GoalFulfillments = await calculateFulfillmentPercent(entity, lstGoals, lst_totalFulfillPromotions);


                //update goals
                lstGoals.ForEach(x =>
                {
                    x.IsUsed = true;
                    unitOfWork.GoalDataService.Update(x);
                });

                unitOfWork.FulfillmentPercentDataService.InsertFulfillment(lst_GoalFulfillments);
                unitOfWork.PromotionDataService.Insert(entity);


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
        public async Task<IBusinessResult> PutInProcessQueueAsync(int year, int month)
        {
            var result = new BusinessResult();
            try
            {
                var entity = await unitOfWork.PromotionDataService.FirstOrDefaultAsync(x => x.Month == month && x.Year == year);
                var monthName = Utilities.PersianMonthNames().FirstOrDefault(x => x.Key == month).Value;
                if (entity != null)
                {
                    if (entity.StatusId == PromotionStatusEnum.WaitingForGoalFulfillment)
                    {
                        entity.StatusId = PromotionStatusEnum.InProcessQueue;
                        //لود شروط تحقق 
                        var lstGoalFulfillments = await unitOfWork.FulfillmentPercentDataService.GetAsync(x => x.Year == entity.Year && x.Month == entity.Month && x.IsUsed == false);

                        lstGoalFulfillments.ForEach(x =>
                        {
                            x.IsUsed = true;
                            unitOfWork.FulfillmentPercentDataService.Update(x);
                        });

                        unitOfWork.PromotionDataService.Update(entity);

                        await unitOfWork.CommitAsync();
                        result.ReturnMessage.Add($"پورسانت {monthName} ماه سال {year} در صف محاسبه قرار گرفت");
                    }
                    else
                    {
                        result.ReturnStatus = false;
                        result.ReturnMessage.Add("با توجه به وضعیت پورسانت امکان انجام درخواست وجود ندارد");
                    }
                }
                else
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add($"یافت نشد {year} و سال {monthName} اطلاعات پورسانت برای ماه ");
                }
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> CalculateGoalsAsync(Promotion entity)
        {
            var result = new BusinessResult();
            try
            {
                var result_sales = await CalculateSalesGoalsAsync(entity);
                var result_receipt = await CalculateReceiptGoalsAsync(entity);

                result.ReturnStatus = result_receipt.ReturnStatus & result_sales.ReturnStatus;
                if (result.ReturnStatus)
                {
                    entity = await unitOfWork.PromotionDataService.FirstOrDefaultAsync(x => x.Id == entity.Id);
                    entity.StatusId = PromotionStatusEnum.GoalCalculated;
                    await unitOfWork.CommitAsync();
                }
                result.ReturnMessage.AddRange(result_receipt.ReturnMessage);
                result.ReturnMessage.AddRange(result_sales.ReturnMessage);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        /// <summary>
        /// محاسبه پورسانت اهداف ریالی و تعدادی
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IBusinessResult> CalculateSalesGoalsAsync(Promotion entity)
        {
            var result = new BusinessResult();
            try
            {
                if (entity.StatusId != PromotionStatusEnum.InProcessQueue || entity.IsSalesCalculated)
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add("با توجه به وضعیت پورسانت امکان محاسبه پورسانت اهداف فروش وجود ندارد");
                    return result;
                }

                entity = await unitOfWork.PromotionDataService.FirstOrDefaultAsync(x => x.Id == entity.Id, includes: x => new { x.BranchPromotions });



                //به دلیل اینکه ممکن است اهدافی تعریف شود که طول زمان آن بیشتر از یکماه باشد 

                //به دلیل اختلاف روز های ماه های یک تا 6 و تاریخ میلادی اولین روز ماه تاریخ پورسانت بدست آمده است
                PersianCalendar persianCalendar = new PersianCalendar();
                var monthEndDate = persianCalendar.GetMonth(entity.EndDate);
                var yearEndDate = persianCalendar.GetYear(entity.EndDate);

                var dateRange = new { Start = persianCalendar.ToDateTime(yearEndDate, monthEndDate, 1, 0, 0, 0, 0), End = entity.EndDate };

                //لود اطلاعات اهدافی که تاریخ پایان آنها تا یکماه گذشته تاریخ پورسانت باشد و محاسبه نشده باشد 
                var lst_goals = await unitOfWork.GoalDataService.GetQuery()
                                   .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false))
                                   .IncludeFilter(x => x.BranchGoals.Where(y => y.Deleted == false))
                                   .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false).Select(i => i.Items))
                                   .IncludeFilter(x => x.QuantityConditions.Where(y => y.Deleted == false))
                                   .IncludeFilter(x => x.QuantityConditions.Select(y => y.GoodsQuantityConditions.Where(z => z.Deleted == false)))
                                   .IncludeFilter(x => x.QuantityConditions.Select(y => y.GoodsQuantityConditions.Select(u => u.BranchQuantityConditions.Where(z => z.Deleted == false))))
                                   .IncludeFilter(x => x.GoalNonFulfillmentPercents.Where(y => y.Deleted == false))
                                   .IncludeFilter(x => x.GoalNonFulfillmentPercents.Select(z => z.GoalNonFulfillmentBranches.Where(y => y.Deleted == false)))
                                   .Where(g => g.IsUsed == false && g.Deleted == false
                                   && (g.EndDate >= dateRange.Start && g.EndDate <= dateRange.End)
                                   && (g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Group ||
                                   g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Single))
                                   .ToListAsync();

                lst_goals.ForEach(x =>
                {
                    x.IsUsed = true;
                    unitOfWork.GoalDataService.Update(x);
                });


                //لیست ضریب تحقق ویزیتور هر مرکز
                List<FulfillmentPercent> lst_fulfillmentPercents = await (from gFull in unitOfWork.FulfillmentPercentDataService.GetQuery()
                                                                          where gFull.Year == entity.Year && gFull.Month == entity.Month
                                                                          && gFull.Deleted == false
                                                                          select gFull).ToListAsync();

                GoalStep goalStep = null;



                //اهداف مرتبط به مرکز
                {
                    //لیست اهداف مرتبط به مرکز
                    var lst_branchGoals = lst_goals.Where(x => x.ApprovePromotionTypeId == ApprovePromotionTypeEnum.Branch);

                    List<BranchSalesInfo> lst_branch_salesInfo = new List<BranchSalesInfo>();

                    //فعلا وجود ندارد
                    // هدف فروش ریالی به همراه اهداف تعدادی مشروط
                    lst_branchGoals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Amount
                    && x.QuantityConditions.Any(y => y.QuantityConditionTypeId == QuantityConditionTypeEnum.DependedOnGoal))
                        .ToList()
                        .ForEach(goal =>
                        {
                            //TODO 
                        });

                    //فعلا وجود ندارد
                    //هدف فروش ریالی به همراه اهداف تعدادی
                    lst_branchGoals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Amount
                    && x.QuantityConditions.Any(y => y.QuantityConditionTypeId == QuantityConditionTypeEnum.Independent))
                        .ToList()
                        .ForEach(goal =>
                        {
                            //TODO 
                        });

                    //اهداف با معیار تعدادی و ریالی بدون داشتن هدف تعدادی
                    lst_branchGoals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Quantities
                    || (x.ComputingTypeId == ComputingTypeEnum.Amount && x.QuantityConditions.Count == 0))
                        .ToList()
                        .ForEach(goal =>
                        {
                            //به دلیل اینکه ممکن است اهدافی تعریف شود که طول زمان آن بیشتر از یکماه باشد 
                            //باید به ازای هر هدف و تاریخ آن فروش محاسبه شود
                            lst_branch_salesInfo = getBranchSalesInfo(goal, lst_fulfillmentPercents);


                            foreach (var branchGoal in goal.BranchGoals.Where(x => lst_branch_salesInfo.Any(y => y.BranchId == x.BranchId)))
                            {
                                //اطلاعات فروش مرکز
                                var branchSalesInfo = lst_branch_salesInfo.Single(x => x.BranchId == branchGoal.BranchId);
                                branchSalesInfo.BranchGoalId = branchGoal.Id;
                                if (goal.ComputingTypeId == ComputingTypeEnum.Quantities)
                                {
                                    //در صورتیکه معیار هدف تعدادی باشد ،باید تعداد فروش مقایسه شود
                                    goalStep = goal.GoalSteps.OrderByDescending(step => step.ComputingValue)
                                        .Where(step => (step.ComputingValue * branchGoal.Percent * 0.01M) <= branchSalesInfo.TotalQuantity)
                                        .FirstOrDefault();
                                }
                                else if (goal.ComputingTypeId == ComputingTypeEnum.Amount)
                                {
                                    //در صورتیکه معیار هدف تعدادی باشد ،باید مقدار فروش مقایسه شود
                                    goalStep = goal.GoalSteps.OrderByDescending(step => step.ComputingValue)
                                        .Where(step => (step.ComputingValue * branchGoal.Percent * 0.01M) <= branchSalesInfo.TotalSales)
                                        .FirstOrDefault();
                                }

                                addBranchGoalPromotion(entity, goal, goalStep, branchSalesInfo);
                            }
                        });

                    //اهداف با معیار درصد که حتما باید اهداف تعدادی داشته باشند
                    lst_branchGoals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Percentage && x.QuantityConditions.Count != 0)
                        .ToList()
                        .ForEach(goal =>
                        {
                            var quantityConditions = goal.QuantityConditions.Single();

                            var lst_branchConditions = (from gdsqc in quantityConditions.GoodsQuantityConditions
                                                        join brsa in unitOfWork.BranchSalesDataService.GetQuery()
                                                        on gdsqc.GoodsId equals brsa.GoodsId
                                                        where brsa.StartDate >= goal.StartDate && brsa.EndDate <= goal.EndDate
                                                        select new
                                                        {
                                                            brsa.BranchId,
                                                            brsa.GoodsId,
                                                            Amount = brsa.TotalAmount,
                                                            Quantity = brsa.TotalNumber,
                                                            IsGoodsFulfilled = gdsqc.BranchQuantityConditions.SingleOrDefault(x => x.BranchId == brsa.BranchId) != null ? gdsqc.BranchQuantityConditions.SingleOrDefault(x => x.BranchId == brsa.BranchId).Quantity > brsa.TotalNumber : false
                                                        }).ToList();

                            var lst_branchFulfillInfo = (from br_cd in lst_branchConditions
                                                         select new
                                                         {
                                                             br_cd.BranchId,
                                                             FulFillPercent = (lst_branchConditions.Count(x => x.IsGoodsFulfilled) / quantityConditions.Quantity) * 100
                                                         }).ToList();

                            lst_branchFulfillInfo.ForEach(branchFulfillInfo =>
                            {
                                goalStep = goal.GoalSteps.OrderByDescending(step => step.ComputingValue)
                                .Where(step => step.ComputingValue <= branchFulfillInfo.FulFillPercent)
                                .FirstOrDefault();
                                //اطلاعات فروش مرکز 
                                BranchSalesInfo branchSalesInfo = new BranchSalesInfo
                                {
                                    TotalSales = lst_branchConditions.Sum(x => x.Amount),
                                    TotalQuantity = lst_branchConditions.Sum(x => x.Quantity),
                                    BranchId = branchFulfillInfo.BranchId,
                                    SellerFulfillmentPercent = lst_fulfillmentPercents.Any(x => x.BranchId == branchFulfillInfo.BranchId) ?
                                            lst_fulfillmentPercents.Single(x => x.BranchId == branchFulfillInfo.BranchId).SellerFulfillmentPercent : null
                                };

                                addBranchGoalPromotion(entity, goal, goalStep, branchSalesInfo);
                            });


                        });
                }


                //اهداف مرتبط به عوامل فروش
                {
                    //
                    lst_goals.Where(x => x.ApprovePromotionTypeId == ApprovePromotionTypeEnum.Seller)
                        .ToList()
                        .ForEach(goal =>
                        {
                            //به دلیل اینکه ممکن است اهدافی تعریف شود که طول زمان آن بیشتر از یکماه باشد 
                            //باید به ازای هر هدف و تاریخ آن فروش محاسبه شود
                            var lst_MemebrSales = (from ggcg in unitOfWork.GoalGoodsCategoryGoodsDataService.GetQuery()
                                                   join brsa in unitOfWork.MemberSalesDataService.GetQuery()
                                                   on ggcg.GoodsId equals brsa.GoodsId
                                                   join member in unitOfWork.MemberDataService.GetQuery()
                                                   on brsa.MemberId equals member.Id
                                                   where brsa.StartDate >= goal.StartDate && brsa.EndDate <= goal.EndDate
                                                   && ggcg.GoalGoodsCategoryId == goal.GoalGoodsCategoryId
                                                   select new
                                                   {
                                                       brsa.GoodsId,
                                                       brsa.Quantity,
                                                       brsa.Amount,
                                                       member.BranchId,
                                                       brsa.MemberId
                                                   }).ToList();
                            lst_MemebrSales.ForEach(memberSales =>
                            {
                                if (goal.ComputingTypeId == ComputingTypeEnum.Quantities)
                                {
                                    //در صورتیکه معیار هدف تعدادی باشد ،باید تعداد فروش مقایسه شود
                                    goalStep = goal.GoalSteps.OrderByDescending(step => step.ComputingValue)
                                        .Where(step => step.ComputingValue <= memberSales.Quantity)
                                        .FirstOrDefault();
                                }
                                else if (goal.ComputingTypeId == ComputingTypeEnum.Amount)
                                {
                                    //در صورتیکه معیار هدف تعدادی باشد ،باید مقدار فروش مقایسه شود
                                    goalStep = goal.GoalSteps.OrderByDescending(step => step.ComputingValue)
                                        .Where(step => step.ComputingValue <= memberSales.Amount)
                                        .FirstOrDefault();
                                }

                                decimal promotion = 0;
                                // هدف فروش زده است؟
                                if (goalStep != null)
                                {
                                    var goalStepItem = goalStep.Items.Where(it => it.ActionTypeId == GoalStepActionTypeEnum.Reward).Single();

                                    if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.Percent) //درصد پاداش
                                    {
                                        promotion = goalStepItem.Amount.Value * 0.01M * memberSales.Amount;
                                    }
                                    else if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.SingleGoods) //جایزه عوامل فروش هر محصول
                                    {
                                        promotion = goalStepItem.Amount.Value * memberSales.Quantity;
                                    }
                                    else if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.Seller) //جایزه عوامل فروش
                                    {
                                        if (goalStepItem.ComputingTypeId.Value == ComputingTypeEnum.Amount)
                                        {
                                            //به ازای فروش این مبلغ
                                            promotion = goalStepItem.Amount.Value * (memberSales.Amount / goalStepItem.EachValue.Value);
                                        }
                                        else if (goalStepItem.ComputingTypeId.Value == ComputingTypeEnum.Quantities)
                                        {
                                            //به ازای فروش این تعداد
                                            promotion = goalStepItem.Amount.Value * (memberSales.Quantity / goalStepItem.EachValue.Value);
                                        }
                                    }

                                    entity.BranchPromotions.Single(x => x.BranchId == memberSales.BranchId)
                                    .MemberPromotions
                                    .Add(new MemberPromotion
                                    {
                                        GoalId = goal.Id,
                                        Promotion = promotion,
                                        MemberId = memberSales.MemberId
                                    });
                                };

                            });
                        });
                }

                entity.IsSalesCalculated = true;
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add("محاسبه پورسانت اهداف فروش با موفقیت پایان یافت");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        /// <summary>
        /// محاسبه پورسانت اهداف وصول خصوصی و کل
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IBusinessResult> CalculateReceiptGoalsAsync(Promotion entity)
        {
            var result = new BusinessResult();
            try
            {
                if (entity.StatusId != PromotionStatusEnum.InProcessQueue && entity.IsReceiptCalculated == false)
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add("با توجه به وضعیت پورسانت امکان محاسبه پورسانت اهداف وصول وجود ندارد");
                    return result;
                }

                entity = await unitOfWork.PromotionDataService.FirstOrDefaultAsync(x => x.Id == entity.Id, includes: x => new { x.BranchPromotions });
                //دریافت اطلاعات اهداف وصول خصوصی / کلی
                var lst_goals = await (from g in unitOfWork.GoalDataService.GetQuery()
                                       .IncludeFilter(x => x.BranchGoals.Where(y => y.Deleted == false))
                                       .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false))
                                       .IncludeFilter(x => x.BranchReceiptGoalPercent.Where(y => y.Deleted == false))
                                       where g.Deleted == false
                                       && g.Month == entity.Month && g.Year == entity.Year
                                       && (g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal
                                       || g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal
                                       || g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptGovernGoal)
                                       select g).ToListAsync();

                lst_goals.ForEach(x =>
                {
                    x.IsUsed = true;
                    unitOfWork.GoalDataService.Update(x);
                });


                //وصول هر مرکز
                var lst_branchReceipts = await unitOfWork.BranchReceiptDataService.GetAsync(where: x => x.Year == entity.Year && x.Month == entity.Month);

                var receiptTotalGoal = lst_goals.Single(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal);
                var receiptPrivateGoal = lst_goals.Single(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal);

                //لیست سهم پست های سازمانی مراکز
                var lst_orgStructureShare = await unitOfWork.OrgStructureShareDataService.GetAsync(x => x.PrivateReceiptPercent != null || x.TotalReceiptPercent != null
                , includes: x => new { x.OrgStructure });


                decimal promotion = 0;
                BranchPromotion branchPromotion = new BranchPromotion();
                List<OrgStructureShare> lst_orgStructureShare_branch = new List<OrgStructureShare>();
                lst_branchReceipts.ForEach(braReceipt =>
                {
                    branchPromotion = entity.BranchPromotions.Single(x => x.BranchId == braReceipt.BranchId);

                    //لیست سهم پست های سازمانی برای مرکز
                    lst_orgStructureShare_branch = lst_orgStructureShare.Where(x => x.BranchId == braReceipt.BranchId).ToList();

                    //محاسبه پورسانت هدف وصول کل
                    promotion = getReceiptPromotion(braReceipt, receiptTotalGoal);
                    //محاسبه سهم پورسانت پست های سازمانی از هدف وصول کل
                    branchPromotion.BranchGoalPromotions.Add(new BranchGoalPromotion
                    {
                        GoalId = receiptTotalGoal.Id,
                        FinalPromotion = promotion,
                        BranchGoalId = receiptTotalGoal.BranchGoals.Single(x => x.BranchId == braReceipt.BranchId).Id,
                        PromotionWithOutFulfillmentPercent = promotion,
                        PositionReceiptPromotions = lst_orgStructureShare_branch
                        .Where(x => x.TotalReceiptPercent != null)
                        .Select(x => new PositionReceiptPromotion
                        {
                            OrgStructureShareId = x.Id,
                            Promotion = x.TotalReceiptPercent.Value * promotion * 0.01M,
                            PositionTypeId = x.OrgStructure.PositionTypeId
                        }).ToList()
                    });

                    //محاسبه پورسانت هدف وصول خصوصی
                    promotion = getReceiptPromotion(braReceipt, receiptPrivateGoal);

                    //محاسبه سهم پورسانت پست های سازمانی از هدف وصول خصوصی
                    branchPromotion.BranchGoalPromotions.Add(new BranchGoalPromotion
                    {
                        GoalId = receiptPrivateGoal.Id,
                        FinalPromotion = promotion,
                        PromotionWithOutFulfillmentPercent = promotion,
                        BranchGoalId = receiptPrivateGoal.BranchGoals.Single(x => x.BranchId == braReceipt.BranchId).Id,
                        PositionReceiptPromotions = lst_orgStructureShare_branch
                        .Where(x => x.PrivateReceiptPercent != null)
                        .Select(x => new PositionReceiptPromotion
                        {
                            OrgStructureShareId = x.Id,
                            Promotion = x.PrivateReceiptPercent.Value * promotion * 0.01M,
                            PositionTypeId = x.OrgStructure.PositionTypeId

                        }).ToList()
                    });

                });
                entity.IsReceiptCalculated = true;
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add("محاسبه پورسانت اهداف وصول با موفقیت پایان یافت");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<ReportBranchSalesGoal>>> LoadReportBranchSalesGoal(DateTime startDate, DateTime endDate
            , int goalGoodsCategoryId
            )
        {
            var result = new BusinessResultValue<List<ReportBranchSalesGoal>>();
            try
            {

                var query = await (from gl in unitOfWork.GoalDataService.GetQuery()
                                   join ggc in unitOfWork.GoalGoodsCategoryDataService.GetQuery()
                                   on gl.GoalGoodsCategoryId equals ggc.Id
                                   join gls in unitOfWork.GoalStepDataService.GetQuery()
                                   on gl.Id equals gls.GoalId
                                   join brgl in unitOfWork.BranchGoalDataService.GetQuery()
                                   on gl.Id equals brgl.GoalId
                                   join br in unitOfWork.BranchDataService.GetQuery()
                                   on brgl.BranchId equals br.Id
                                   join brglp in unitOfWork.BranchGoalPromotionDataService.GetQuery()
                                   on brgl.Id equals brglp.BranchGoalId
                                   where (gl.ComputingTypeId == ComputingTypeEnum.Amount ||
                                   gl.ComputingTypeId == ComputingTypeEnum.Quantities)
                                   &&
                                   gl.GoalTypeId == GoalTypeEnum.Distributor &&
                                   gl.GoalGoodsCategoryId == goalGoodsCategoryId
                                   && gl.StartDate >= startDate && gl.EndDate <= gl.EndDate
                                   && gl.GoalGoodsCategoryId == goalGoodsCategoryId
                                   select new
                                   {
                                       gl.ComputingTypeId,
                                       brglp.TotalSales,
                                       brglp.TotalQuantity,
                                       BranchName = br.Name,
                                       brglp.FinalPromotion,
                                       brglp.PromotionWithOutFulfillmentPercent,
                                       GoalAmount = gls.ComputingValue,
                                       GoalGoodsCategoryName = ggc.Name,
                                       AmountSpecified = brgl.Percent.HasValue ? brgl.Percent.Value * 0.01M * gls.ComputingValue : brgl.Amount.Value
                                   })
                                   .ToListAsync();

                result.ResultValue = query.Select(x => new ReportBranchSalesGoal
                {
                    AmountSpecified = x.AmountSpecified,
                    BranchName = x.BranchName,
                    GoalAmount = x.GoalAmount,
                    PromotionWithOutFulfillmentPercent = x.PromotionWithOutFulfillmentPercent,
                    TotalSales = x.TotalSales,
                    TotalQuantity = x.TotalQuantity,
                    ComputingTypeId = x.ComputingTypeId,
                    FinalPromotion = x.FinalPromotion,
                    GoalGoodsCategoryName = x.GoalGoodsCategoryName,
                    FulfilledPercent = x.ComputingTypeId == ComputingTypeEnum.Amount ? (x.TotalSales * 100) / x.AmountSpecified : (x.TotalQuantity * 100) / x.AmountSpecified
                }).ToList();
                result.ReturnStatus = true;

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<List<ReportBranchReceiptGoal>>> LoadReportBranchReceipt(DateTime startDate, DateTime endDate)
        {
            var result = new BusinessResultValue<List<ReportBranchReceiptGoal>>();
            try
            {

                var query = await (from gl in unitOfWork.GoalDataService.GetQuery()
                                   join brgl in unitOfWork.BranchGoalDataService.GetQuery()
                                   on gl.Id equals brgl.GoalId
                                   join br in unitOfWork.BranchDataService.GetQuery()
                                   on brgl.BranchId equals br.Id
                                   join brglp in unitOfWork.BranchGoalPromotionDataService.GetQuery()
                                   on brgl.Id equals brglp.BranchGoalId
                                   join prp in unitOfWork.PositionReceiptPromotionDataService.GetQuery()
                                   on brglp.Id equals prp.BranchGoalPromotionId
                                   join brr in unitOfWork.BranchReceiptDataService.GetQuery()
                                   on br.Id equals brr.BranchId
                                   where brr.StartDate >= startDate && brr.EndDate <= gl.EndDate
                                   join pt in unitOfWork.PositionTypeDataService.GetQuery()
                                   on prp.PositionTypeId equals pt.eId
                                   where gl.GoalTypeId == GoalTypeEnum.Distributor &&
                                   (gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal
                                   || gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.TotalSalesGoal)
                                   && gl.StartDate >= startDate && gl.EndDate <= gl.EndDate
                                   select new
                                   {
                                       AmountSpecified = brgl.Amount.Value,
                                       brglp.TotalSales,
                                       brglp.TotalQuantity,
                                       BranchName = br.Name,
                                       brglp.FinalPromotion,
                                       prp.Promotion,
                                       PositionTitle = pt.Description,
                                       ReceiptPrivateAmount = brr.PrivateAmount,
                                       ReceiptTotalAmount = brr.TotalAmount
                                   })
                                   .ToListAsync();

                result.ResultValue = query.Select(x => new ReportBranchReceiptGoal
                {
                    AmountSpecified = x.AmountSpecified,
                    BranchName = x.BranchName,
                    TotalSales = x.TotalSales,
                    TotalQuantity = x.TotalQuantity,
                    FinalPromotion = x.FinalPromotion,
                    FulfilledPercent = (x.TotalSales * 100) / x.AmountSpecified, 
                    PositionPromotion = x.Promotion,
                    PositionTitle = x.PositionTitle,
                    ReceiptPrivateAmount = x.ReceiptPrivateAmount.Value,
                    ReceiptTotalAmount = x.ReceiptTotalAmount
                }).ToList();
                result.ReturnStatus = true;

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        #endregion

        #region [ Private Method(s) ]

        private async Task<List<FulfillmentPercent>> calculateFulfillmentPercent(Promotion entity, List<Goal> lstGoals, List<FulfillmentPromotionCondition> lst_totalFulfillPromotion)
        {
            //  هدف کل و وصول و سهم مراکز از اهداف تعریف شده
            Goal totalSalesGoal = lstGoals.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.TotalSalesGoal);
            Goal receiptTotalGoal = lstGoals.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal);
            Goal receiptPrivateGoal = lstGoals.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal);
            Goal aggregationGoal = lstGoals.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.AggregationGoal);

            var totalSalesGoalAmount = totalSalesGoal.GoalSteps.FirstOrDefault().ComputingValue;

            var lst_aggregationInfo = new List<AggregationInfo>();
            if (aggregationGoal != null)
            {
                lst_aggregationInfo = (from brs in unitOfWork.BranchSalesDataService.GetQuery()
                                       where brs.Month > 1 && brs.Year == entity.Year
                                       group brs by brs.BranchId into grp
                                       select new AggregationInfo
                                       {
                                           BranchId = grp.Key,
                                           AggregationSalesAmount = grp.Sum(x => x.TotalAmount),
                                           AggregationSalesNumber = grp.Sum(x => x.TotalNumber)
                                       }).ToList();
            }

            var query_branchPromotion = await (from brSal in (from br in unitOfWork.BranchSalesDataService.GetQuery()
                                                              where br.Month == entity.Month
                                                              && br.Year == entity.Year
                                                              && br.Deleted == false
                                                              group br by br.BranchId into grp
                                                              select new
                                                              {
                                                                  BranchId = grp.Key,
                                                                  TotalAmount = grp.Sum(x => x.TotalAmount),
                                                              })
                                               join brRec in unitOfWork.BranchReceiptDataService.GetQuery()
                                               on brSal.BranchId equals brRec.BranchId into branch_receipt_leftjoin
                                               from br_sales_receipt in branch_receipt_leftjoin.Where(x =>
                                                  x.Month == entity.Month && x.Year == entity.Year
                                                  && x.Deleted == false).DefaultIfEmpty()
                                               select new
                                               {
                                                   brSal.BranchId,
                                                   TotalReceiptAmount = br_sales_receipt != null ? br_sales_receipt.TotalAmount : 0,
                                                   PrivateReceiptAmount = br_sales_receipt != null ? br_sales_receipt.PrivateAmount.Value : 0,
                                                   TotalSalesAmount = brSal.TotalAmount,
                                               }).ToListAsync();


            var lst_fulfillmentPercent = new List<FulfillmentPercent>();

            BranchPromotion branchPromotion;
            query_branchPromotion.ForEach(queryRecord =>
            {
                branchPromotion = new BranchPromotion { Year = entity.Year, Month = entity.Month, BranchId = queryRecord.BranchId };
                //Aggregation data
                if (aggregationGoal != null)
                {
                    AggregationInfo aggregationInfo = lst_aggregationInfo.SingleOrDefault(x => x.BranchId == queryRecord.BranchId);
                    if (aggregationInfo != null)
                    {
                        branchPromotion.AggregationSpecifiedAmount = aggregationGoal.BranchGoals.Single(bg => bg.BranchId == queryRecord.BranchId).Amount.Value;
                        branchPromotion.AggregationReachedPercent = Math.Round(aggregationInfo.AggregationSalesAmount / branchPromotion.AggregationSpecifiedAmount * 100, MidpointRounding.AwayFromZero);
                        branchPromotion.AggregationSalesAmount = aggregationInfo.AggregationSalesAmount;
                    }

                }

                //Total sales data
                branchPromotion.TotalSalesSpecifiedPercent = totalSalesGoal.BranchGoals.Single(bg => bg.BranchId == queryRecord.BranchId).Percent.Value;
                branchPromotion.TotalSalesSpecifiedAmount = (totalSalesGoalAmount * branchPromotion.TotalSalesSpecifiedPercent) / 100;
                branchPromotion.TotalSalesReachedPercent = Math.Round((queryRecord.TotalSalesAmount / branchPromotion.TotalSalesSpecifiedAmount) * 100, MidpointRounding.AwayFromZero);


                //total receipt
                branchPromotion.TotalReceiptSpecifiedAmount = receiptTotalGoal.BranchGoals.Single(bg => bg.BranchId == queryRecord.BranchId).Amount.Value;
                branchPromotion.TotalReceiptReachedPercent = Math.Round((queryRecord.TotalReceiptAmount / branchPromotion.TotalReceiptSpecifiedAmount) * 100, MidpointRounding.AwayFromZero);

                //private receipt
                branchPromotion.PrivateReceiptSpecifiedAmount = receiptPrivateGoal.BranchGoals.Single(bg => bg.BranchId == queryRecord.BranchId).Amount.Value;
                branchPromotion.PrivateReceiptReachedPercent = Math.Round(queryRecord.PrivateReceiptAmount / branchPromotion.PrivateReceiptSpecifiedAmount * 100, MidpointRounding.AwayFromZero);

                //maximum value between aggregation and total sales
                var max_totalSales_aggregation = branchPromotion.AggregationReachedPercent > branchPromotion.TotalReceiptReachedPercent ? branchPromotion.AggregationReachedPercent : branchPromotion.TotalReceiptReachedPercent;


                var fulfillmentPromotion_manager = lst_totalFulfillPromotion
                    .OrderByDescending(x => x.TotalSalesFulfilledPercent)
                    .FirstOrDefault(x => x.TotalSalesFulfilledPercent <= max_totalSales_aggregation
                    && x.TotalReceiptFulfilledPercent <= branchPromotion.TotalReceiptReachedPercent
                    && x.PrivateReceiptFulfilledPercent <= branchPromotion.PrivateReceiptReachedPercent
                    && x.ManagerPromotion != null);

                var fulfillmentPromotion_seller = lst_totalFulfillPromotion
                    .OrderByDescending(x => x.TotalSalesFulfilledPercent)
                    .FirstOrDefault(x => x.TotalSalesFulfilledPercent <= max_totalSales_aggregation
                    && x.TotalReceiptFulfilledPercent <= branchPromotion.TotalReceiptReachedPercent
                    && x.PrivateReceiptFulfilledPercent <= branchPromotion.PrivateReceiptReachedPercent
                    && x.SellerPromotion != null);

                lst_fulfillmentPercent.Add(new FulfillmentPercent
                {
                    BranchId = queryRecord.BranchId,

                    Year = totalSalesGoal.Year,
                    Month = totalSalesGoal.Month,
                    ManagerReachedPercent = fulfillmentPromotion_manager.ManagerPromotion.Value,
                    ManagerFulfillmentPercent = fulfillmentPromotion_manager.ManagerPromotion,
                    SellerReachedPercent = fulfillmentPromotion_seller.SellerPromotion.Value,
                    SellerFulfillmentPercent = fulfillmentPromotion_seller.SellerPromotion,
                    TotalSalesFulfilledPercent = max_totalSales_aggregation,
                    TotalReceiptFulfilledPercent = branchPromotion.TotalReceiptReachedPercent,
                    PrivateReceiptFulfilledPercent = branchPromotion.PrivateReceiptReachedPercent,
                });

                entity.BranchPromotions.Add(branchPromotion);
            });

            return lst_fulfillmentPercent;
        }
        private List<BranchSalesInfo> getBranchSalesInfo(Goal goal, List<FulfillmentPercent> lst_fulfillmentPercents)
        {
            var query = (from ggcg in unitOfWork.GoalGoodsCategoryGoodsDataService.GetQuery()
                         join brsa in unitOfWork.BranchSalesDataService.GetQuery()
                         on ggcg.GoodsId equals brsa.GoodsId
                         where brsa.StartDate >= goal.StartDate && brsa.EndDate <= goal.EndDate
                         && ggcg.GoalGoodsCategoryId == goal.GoalGoodsCategoryId
                         group brsa by new { brsa.BranchId } into grp
                         select new
                         {
                             grp.Key.BranchId,
                             TotalSales = grp.Sum(x => x.TotalAmount),
                             TotalQuantity = grp.Sum(x => x.TotalNumber),
                         }).ToList();

            var result = (from q in query
                          join fulfill in lst_fulfillmentPercents
                          on q.BranchId equals fulfill.BranchId
                          select new BranchSalesInfo
                          {
                              BranchId = q.BranchId,
                              TotalSales = q.TotalSales,
                              TotalQuantity = q.TotalQuantity,
                              SellerFulfillmentPercent = fulfill.SellerFulfillmentPercent
                          }).ToList();
            return result;
        }
        /// <summary>
        /// محاسبه مبلغ پورسانت و ثبت اطلاعات پورسانت بدست آمده برای هدف و مرکز 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="goal"></param>
        /// <param name="goalStep"></param>
        /// <param name="branchSalesInfo"></param>
        private void addBranchGoalPromotion(Promotion entity, Goal goal, GoalStep goalStep, BranchSalesInfo branchSalesInfo)
        {
            decimal promotion = 0;
            // هدف فروش زده است؟
            if (goalStep != null)
            {
                var goalStepItem = goalStep.Items.Where(it => it.ActionTypeId == GoalStepActionTypeEnum.Reward).Single();

                if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.Percent) //درصد پاداش
                {
                    promotion = goalStepItem.Amount.Value * 0.01M * branchSalesInfo.TotalSales;
                }
                else if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.SingleGoods) //جایزه عوامل فروش هر محصول
                {
                    promotion = goalStepItem.Amount.Value * branchSalesInfo.TotalQuantity;
                }
                else if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.Seller) //جایزه عوامل فروش
                {
                    if (goalStepItem.ComputingTypeId.Value == ComputingTypeEnum.Amount)
                    {
                        //به ازای فروش این مبلغ
                        promotion = goalStepItem.Amount.Value * (branchSalesInfo.TotalSales / goalStepItem.EachValue.Value);
                    }
                    else if (goalStepItem.ComputingTypeId.Value == ComputingTypeEnum.Quantities)
                    {
                        //به ازای فروش این تعداد
                        promotion = goalStepItem.Amount.Value * (branchSalesInfo.TotalQuantity / goalStepItem.EachValue.Value);
                    }
                }

                entity.BranchPromotions
                .Single(x => x.BranchId == branchSalesInfo.BranchId)
                .BranchGoalPromotions
                .Add(new BranchGoalPromotion
                {
                    GoalId = goal.Id,
                    PromotionWithOutFulfillmentPercent = promotion,
                    BranchGoalId = branchSalesInfo.BranchGoalId,
                    TotalSales = branchSalesInfo.TotalSales,
                    TotalQuantity = branchSalesInfo.TotalQuantity,
                    FinalPromotion = branchSalesInfo.SellerFulfillmentPercent.HasValue ? promotion * branchSalesInfo.SellerFulfillmentPercent.Value : promotion
                });
            }
            else if (goal.GoalNonFulfillmentPercents.Count != 0)
            {
                //در صورت عدم تحقق هدف ،چک کردن اینکه 'سهم مرکز در صورت عدم تحقق' تعریف شده است یا نه
                var goalNonFulfillmentPercent = goal.GoalNonFulfillmentPercents.SingleOrDefault(x => x.GoalNonFulfillmentBranches.Any(y => y.BranchId == branchSalesInfo.BranchId));
                if (goalNonFulfillmentPercent != null)
                {
                    promotion = goalNonFulfillmentPercent.Percent * 0.01M * branchSalesInfo.TotalSales;
                    entity.BranchPromotions
                    .Single(x => x.BranchId == branchSalesInfo.BranchId)
                    .BranchGoalPromotions
                   .Add(new BranchGoalPromotion
                   {
                       GoalId = goal.Id,
                       PromotionWithOutFulfillmentPercent = promotion,
                       BranchGoalId = branchSalesInfo.BranchGoalId,
                       TotalSales = branchSalesInfo.TotalSales,
                       TotalQuantity = branchSalesInfo.TotalQuantity,
                       FinalPromotion = branchSalesInfo.SellerFulfillmentPercent.HasValue ? promotion * branchSalesInfo.SellerFulfillmentPercent.Value : promotion
                   });
                }
            }
            else
            {
                entity.BranchPromotions
                    .Single(x => x.BranchId == branchSalesInfo.BranchId)
                    .BranchGoalPromotions
                   .Add(new BranchGoalPromotion
                   {
                       GoalId = goal.Id,
                       BranchGoalId = branchSalesInfo.BranchGoalId,
                       TotalSales = branchSalesInfo.TotalSales,
                       TotalQuantity = branchSalesInfo.TotalQuantity,
                       FinalPromotion = 0,
                       PromotionWithOutFulfillmentPercent = 0
                   });
            }
        }
        private decimal getReceiptPromotion(BranchReceipt braReceipt, Goal receiptGoal)
        {
            var receiptTotal_branchGoal = receiptGoal.BranchGoals.Single(x => x.BranchId == braReceipt.BranchId);
            var receiptTotal_branchReceiptGoalPercent = receiptGoal.BranchReceiptGoalPercent.Single(x => x.BranchId == braReceipt.BranchId);


            decimal promotion = 0;
            decimal amount = braReceipt.TotalAmount;
            if (receiptGoal.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal)
                amount = braReceipt.PrivateAmount.Value;

            if (receiptTotal_branchGoal.Amount <= amount)
            {
                //مرکز به هدف وصول دست یافته  
                promotion = amount * receiptTotal_branchReceiptGoalPercent.ReachedPercent * 0.01M;
            }
            else
            {
                promotion = amount * receiptTotal_branchReceiptGoalPercent.NotReachedPercent * 0.01M;
            }

            return promotion;
        }
        #endregion
    }
}
