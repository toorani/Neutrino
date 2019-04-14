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
        private readonly IBranchSalesBS branchSalesBS;
        private readonly IQuantityConditionBS quantityConditionBS;
        private readonly IBranchReceiptBS branchReceiptBS;
        class TouchingGoal
        {
            public int GoalId { get; set; }
            public int BranchId { get; set; }
            /// <summary>
            /// جمع مبلغ فروش هدف
            /// </summary>
            public decimal TotalSales { get; set; }
            /// <summary>
            /// جمع تعدادی فروش
            /// </summary>
            public double TotalNumber { get; set; }
            public TouchingGoalStatus StatusId { get; set; }
            /// <summary>
            /// درصد پورسانت 
            /// </summary>
            public decimal PromotionReachedPercent { get; set; }
            /// <summary>
            /// درصد مشمول
            /// </summary>
            public decimal Fulfillment_EncouragePercent { get; set; }
            /// <summary>
            /// مقدار ردیفی که مرکز به هدف تعدادی آن رسیده است
            /// </summary>
            public int TouchedQuntity { get; set; }
            /// <summary>
            /// جمع مبلغ فروش تعدادی 
            /// </summary>
            public decimal TotalQuntitySales { get; set; }
            /// <summary>
            /// مقدار پورسانت
            /// </summary>
            public decimal PromotionValue { get; set; }
            /// <summary>
            /// مقدار پورسانت هدف نزده
            /// </summary>
            public decimal ForthCasePromotionValue { get; set; }
            /// <summary>
            /// درصد تحقق هدف اصلی
            /// </summary>
            public decimal FulfillmentGoalPercent { get; set; }
            /// <summary>
            /// درصد تحقق تعدادی
            /// </summary>
            public decimal FulfillmentQuantityPercent { get; set; }
            public TouchingGoal()
            {
                StatusId = TouchingGoalStatus.Nothing;
                ForthCasePromotionValue = 0;
            }
        }
        enum TouchingGoalStatus : int
        {
            Nothing = 0,
            Amount = 1,
            Qunatity = 2,
            Both = 3
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
            , IBranchSalesBS branchSalesBS
            , IQuantityConditionBS quantityConditionBS
            , IBranchReceiptBS branchReceiptBS
            , AbstractValidator<Promotion> validator)
            : base(unitOfWork)
        {
            this.validator = validator;
            this.branchSalesBS = branchSalesBS;
            this.quantityConditionBS = quantityConditionBS;
            this.branchReceiptBS = branchReceiptBS;
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
                && (x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal || x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.TotalSalesGoal)
                , includes: x => new { x.BranchGoals, x.GoalSteps });

                var lst_totalFulfillPromotions = await unitOfWork.TotalFulfillPromotionPercentDataService.GetAllAsync();

                //calculate goals fulfillment
                var lst_GoalFulfillments = await Calculate_Fulfillment_BranchPromotion(entity, lstGoals, lst_totalFulfillPromotions);


                //update goals
                lstGoals.ForEach(x =>
                {
                    x.IsUsed = true;
                    unitOfWork.GoalDataService.Update(x);
                });

                unitOfWork.FulfillmentPercentDataService.InsertFulfillment(lst_GoalFulfillments);
                unitOfWork.PromotionDataService.Insert(entity);

                //foreach (var entry in dataService.UnitOfWork.Context.ChangeTracker.Entries())
                //{
                //    Debug.WriteLine($"Entity Name: {entry.Entity.GetType().Name} with Status: {entry.State}");
                //}

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
        public async Task<IBusinessResult> PutInProcessQueueAsync(Promotion entity)
        {
            var result = new BusinessResult();
            try
            {
                if (entity.StatusId == PromotionStatusEnum.WaitingForGoalFulfillment)
                {
                    entity = unitOfWork.PromotionDataService.GetById(entity.Id);
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

                    var monthName = Utilities.PersianMonthNames().FirstOrDefault(x => x.Key == entity.Month).Value;

                    result.ReturnMessage.Add($"پورسانت {monthName} ماه سال {entity.Year} در صف محاسبه قرار گرفت");
                }
                else
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add("با توجه به وضعیت پورسانت امکان انجام درخواست وجود ندارد");
                }
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> CalculateAsync(Promotion entity)
        {
            var result = new BusinessResult();
            try
            {
                if (entity.StatusId != PromotionStatusEnum.InProcessQueue)
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add("با توجه به وضعیت پورسانت امکان انجام درخواست وجود ندارد");
                    return result;
                }

                entity = await unitOfWork.PromotionDataService.FirstOrDefaultAsync(x => x.Id == entity.Id, includes: x => new { x.BranchPromotions });

                //Update commission's status 
                entity.StatusId = PromotionStatusEnum.InProcessing;
                unitOfWork.PromotionDataService.Update(entity);

                //محاسبه اهداف فروش
                result = await CalculateSalesGoals(entity) as BusinessResult;
                if (!result.ReturnStatus)
                    return result;

                //محاسبه اهداف وصول
                result = await CalculateReceiptGoals(entity) as BusinessResult;
                if (!result.ReturnStatus)
                    return result;

                await unitOfWork.CommitAsync();

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }

            return result;
        }
        #endregion

        #region [ Private Method(s) ]
        private async Task<List<FulfillmentPercent>> Calculate_Fulfillment_BranchPromotion(Promotion entity, List<Goal> lstGoals, List<FulfillmentPromotionCondition> lst_totalFulfillPromotion)
        {
            //  هدف کل و وصول و سهم مراکز از اهداف تعریف شده
            var totalSalesGoal = lstGoals.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.TotalSalesGoal);
            var receiptGoal = lstGoals.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal);

            var totalGeneralGoalAmount = totalSalesGoal.GoalSteps.FirstOrDefault().ComputingValue;
            var totalReceiptGoalAmount = receiptGoal.GoalSteps.FirstOrDefault().ComputingValue;


            var query_branchPromotion = await (from brSal in (from br in unitOfWork.BranchSalesDataService.GetQuery()
                                                              where br.Month == totalSalesGoal.Month
                                                              && br.Year == totalSalesGoal.Year
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
                                                  x.Month == totalSalesGoal.Month && x.Year == totalSalesGoal.Year
                                                  && x.Deleted == false).DefaultIfEmpty()
                                               select new
                                               {
                                                   BranchId = brSal.BranchId,
                                                   TotalReceiptAmount = br_sales_receipt != null ? br_sales_receipt.TotalAmount : 0,
                                                   TotalSalesAmount = brSal.TotalAmount,
                                               }).ToListAsync();


            var lst_BranchPromotion = query_branchPromotion
                .Select(x => new BranchPromotion()
                {
                    BranchId = x.BranchId,
                    TotalReceiptAmount = x.TotalReceiptAmount,
                    TotalSalesAmount = x.TotalSalesAmount,
                    Year = entity.Year,
                    Month = entity.Month
                }).ToList();

            var lst_Goalfulfillment = new List<FulfillmentPercent>();

            lst_BranchPromotion.ForEach(branchPromotion =>
            {
                branchPromotion.TotalSalesSpecifiedPercent = totalSalesGoal.BranchGoals.Single(bg => bg.BranchId == branchPromotion.BranchId).Percent.Value;
                branchPromotion.TotalSalesSpecifiedAmount = (totalGeneralGoalAmount * branchPromotion.TotalSalesSpecifiedPercent) / 100;
                branchPromotion.TotalSalesReachedPercent = Math.Round((branchPromotion.TotalSalesAmount / branchPromotion.TotalSalesSpecifiedAmount) * 100, MidpointRounding.AwayFromZero);

                branchPromotion.TotalReceiptSpecifiedAmount = receiptGoal.BranchGoals.Single(bg => bg.BranchId == branchPromotion.BranchId).Amount.Value;
                branchPromotion.TotalReceiptReachedPercent = Math.Round((branchPromotion.TotalReceiptAmount / branchPromotion.TotalReceiptSpecifiedAmount) * 100, MidpointRounding.AwayFromZero);

                var range = new FulfillmentPromotionCondition();


                if (branchPromotion.TotalSalesReachedPercent < branchPromotion.TotalReceiptReachedPercent)
                {
                    // درصد فروش کل از درصد وصول کل کمتر میباشد

                    //نزدیکترین محدوده که کوچکتر از درصد دست یافته شده هدف کل میباشد
                    range = lst_totalFulfillPromotion
                    .OrderByDescending(x => x.TotalSalesFulfilledPercent)
                    .FirstOrDefault(x => x.TotalSalesFulfilledPercent <= branchPromotion.TotalSalesReachedPercent);

                    if (range == null)
                    {
                        range = lst_totalFulfillPromotion
                        .OrderBy(x => x.TotalSalesFulfilledPercent).FirstOrDefault();
                    }
                }
                else
                {
                    // درصد فروش کل از درصد وصول کل بیشتر میباشد

                    //نزدیکترین محدوده که کوچکتر از درصد دست یافته شده هدف وصول میباشد
                    range = lst_totalFulfillPromotion
                    .OrderByDescending(x => x.TotalReceiptFulfilledPercent)
                    .FirstOrDefault(x => x.TotalReceiptFulfilledPercent <= branchPromotion.TotalReceiptReachedPercent);

                    if (range == null)
                    {
                        range = lst_totalFulfillPromotion
                        .OrderBy(x => x.TotalReceiptFulfilledPercent).FirstOrDefault();
                    }
                }

                //Add total sales goal fulfillment
                lst_Goalfulfillment.Add(new FulfillmentPercent
                {
                    BranchId = branchPromotion.BranchId,
                    ManagerReachedPercent = range.ManagerPromotion.Value,
                    Year = totalSalesGoal.Year,
                    Month = totalSalesGoal.Month,
                    ManagerFulfillmentPercent = range.SellerPromotion
                });

                //Add receipt goal fulfillment
                lst_Goalfulfillment.Add(new FulfillmentPercent
                {
                    BranchId = branchPromotion.BranchId,
                    SellerReachedPercent = range.SellerPromotion.Value,
                    Year = receiptGoal.Year,
                    Month = receiptGoal.Month,
                    SellerFulfillmentPercent = range.SellerPromotion
                });
                entity.BranchPromotions.Add(branchPromotion);
            });

            return lst_Goalfulfillment;
        }
        private async Task<IBusinessResult> CalculateSalesGoals(Promotion entity)
        {
            var result = new BusinessResult();
            // جمع فروش هر شعبه به تفکیک دارو برای اهداف گروهی و تکی
            var lst_goal_branch_totalSales = await (from g in unitOfWork.GoalDataService.GetQuery()
                                                    join ggc in unitOfWork.GoalGoodsCategoryDataService.GetQuery()
                                                    on g.GoalGoodsCategoryId equals ggc.Id
                                                    from ggcg in ggc.GoodsCollection
                                                    join brsa in unitOfWork.BranchSalesDataService.GetQuery()
                                                    on ggcg.GoodsId equals brsa.GoodsId
                                                    where g.IsUsed == false && g.Deleted == false
                                                    && g.StartDate >= entity.StartDate && g.EndDate <= entity.EndDate
                                                    && (g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Group ||
                                                    g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Single)
                                                    && brsa.Month == entity.Month && brsa.Year == entity.Year
                                                    group brsa by new { brsa.BranchId, g.Id } into grp
                                                    select new TouchingGoal
                                                    {
                                                        GoalId = grp.Key.Id,
                                                        BranchId = grp.Key.BranchId,
                                                        TotalSales = grp.Sum(x => x.TotalAmount),
                                                        TotalNumber = grp.Sum(x => x.TotalNumber),
                                                    }).ToListAsync();

            //لیست شناسه اهداف لود شده  
            var lst_goalIds = lst_goal_branch_totalSales.Select(x => x.GoalId).Distinct();

            //لیست شناسه شعبات لود شده
            var lst_branchIds = lst_goal_branch_totalSales.Select(x => x.BranchId).Distinct();

            //لود اطلاعات اهداف 
            var lst_goals = await (from g in unitOfWork.GoalDataService.GetQuery()
                               .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false))
                               .IncludeFilter(x => x.BranchGoals.Where(y => y.Deleted == false))
                               .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false).Select(i => i.Items))
                                   where lst_goalIds.Contains(g.Id)
                                   select g).ToListAsync();
            lst_goals.ForEach(x =>
            {
                x.IsUsed = true;
                unitOfWork.GoalDataService.Update(x);
            });


            //لیست شرط تحقق فروش کل هر مرکز
            var lst_totalsalesGoal_fulfillments = await (from gFull in unitOfWork.FulfillmentPercentDataService.GetQuery()
                                                         where gFull.Year == entity.Year && gFull.Month == entity.Month
                                                         && gFull.Deleted == false
                                                         select gFull).ToListAsync();

            //جمع فروش هر محصول در هر مرکز
            var result_lst_branchSales = await branchSalesBS.LoadListAsync(entity.Year, entity.Month);
            if (result_lst_branchSales.ReturnStatus == false)
            {
                result.ReturnStatus = false;
                result.ReturnMessage.AddRange(result_lst_branchSales.ReturnMessage);
                return result;
            }

            var lst_branchSales = result_lst_branchSales.ResultValue;

            //لیست اهداف تعدادی تعریف شده
            var resut_load_quantityConditionList = await quantityConditionBS.LoadQuantityConditionListAsync(lst_goalIds.ToList());
            if (resut_load_quantityConditionList.ReturnStatus == false)
            {
                result.ReturnStatus = false;
                result.ReturnMessage.AddRange(resut_load_quantityConditionList.ReturnMessage);
                return result;
            }


            var lstQuantityConditions = resut_load_quantityConditionList.ResultValue;

            //TODO : remove
            var lst_temp_branch = unitOfWork.BranchDataService.GetQuery().ToList();

            lst_goal_branch_totalSales.ForEach(touchingGoal =>
            {
                //سهم مرکز 
                var branchGoal = lst_goals
                .Single(x => x.Id == touchingGoal.GoalId)
                .BranchGoals
                .Single(x => x.BranchId == touchingGoal.BranchId);
                var computingTypeId = lst_goals.Find(x => x.Id == touchingGoal.GoalId).ComputingTypeId;
                var comparsionValue = touchingGoal.TotalSales;

                if (computingTypeId == ComputingTypeEnum.Quantities)
                    comparsionValue = (decimal)touchingGoal.TotalNumber;


                //پیدا کردن پله هدف زده شده
                var goalStep = lst_goals.Single(g => g.Id == touchingGoal.GoalId)
                    .GoalSteps
                    .OrderByDescending(step => step.ComputingValue)
                    .Where(step => (step.ComputingValue * branchGoal.Percent * 0.01M) <= comparsionValue)
                    .FirstOrDefault();

                //درصد مشمول
                var goalFulfillment = lst_totalsalesGoal_fulfillments.FirstOrDefault(x => x.BranchId == touchingGoal.BranchId);
                //touchingGoal.Fulfillment_EncouragePercent = goalFulfillment != null ? goalFulfillment.EncouragePercent.Value : 0;

                // هدف فروش زده است؟
                if (goalStep != null)
                {

                    //درصد پاداش
                    var goalStepItem = goalStep.Items.Where(it => it.ActionTypeId == GoalStepActionTypeEnum.Reward
                    && it.ItemTypeId == (int)RewardTypeEnum.Percent)
                    .FirstOrDefault();


                    touchingGoal.StatusId = TouchingGoalStatus.Amount;
                    touchingGoal.PromotionReachedPercent = goalStepItem != null ? goalStepItem.Amount.Value / 100 : 0;

                    touchingGoal.FulfillmentGoalPercent = Math.Round(comparsionValue * 100 / (goalStep.ComputingValue * branchGoal.Percent.Value * 0.01M), MidpointRounding.AwayFromZero);

                }

                //محاسبه هدف تعدادی
                var goal_quantityCondition = lstQuantityConditions.SingleOrDefault(x => x.GoalId == touchingGoal.GoalId);
                //بررسی اینکه برای هدف ریالی ،هدف تعدادی مشخص شده است
                if (goal_quantityCondition != null)
                {
                    //لیست محصولات به تفکیک شعبه و فیلد رسیدن به هدف تعدادی
                    var lst_goods_branch_touch_quntityGoal = (from qtotalNo in lst_branchSales
                                                              join goodsQu in goal_quantityCondition.GoodsQuantityConditions
                                                              on qtotalNo.GoodsId equals goodsQu.GoodsId
                                                              where goodsQu.Quantity > 0 && qtotalNo.BranchId == touchingGoal.BranchId
                                                              select new BranchQuntityGoal
                                                              {
                                                                  GoalId = touchingGoal.GoalId,
                                                                  TotalNumber = (from d in goodsQu.BranchQuantityConditions
                                                                                 where d.BranchId == touchingGoal.BranchId
                                                                                 select qtotalNo.TotalNumber).SingleOrDefault(),
                                                                  Quntity = (from d in goodsQu.BranchQuantityConditions
                                                                             where d.BranchId == touchingGoal.BranchId
                                                                             select d.Quantity).SingleOrDefault(),
                                                                  GoodsId = goodsQu.GoodsId,
                                                                  BranchId = touchingGoal.BranchId,
                                                                  IsTouchTarget = (from d in goodsQu.BranchQuantityConditions
                                                                                   where d.BranchId == touchingGoal.BranchId
                                                                                   && d.Quantity <= qtotalNo.TotalNumber
                                                                                   select d).Any(),
                                                                  TouchingPercent = (from d in goodsQu.BranchQuantityConditions
                                                                                     where d.BranchId == touchingGoal.BranchId
                                                                                     select (int)(qtotalNo.TotalNumber / d.Quantity * 100)).SingleOrDefault()
                                                              }).ToList();
                    unitOfWork.BranchQuntityGoalDataService.InsertBulkAsync(lst_goods_branch_touch_quntityGoal);


                    touchingGoal.TouchedQuntity = lst_goods_branch_touch_quntityGoal.Count(x => x.IsTouchTarget);

                    if (touchingGoal.TouchedQuntity >= goal_quantityCondition.Quantity)
                        touchingGoal.StatusId += 2;

                    touchingGoal.TotalQuntitySales = lst_branchSales.Where(x => x.BranchId == touchingGoal.BranchId
                            && goal_quantityCondition.GoodsQuantityConditions.Any(y => y.GoodsId == x.GoodsId && y.Quantity > 0))
                            .Sum(x => x.TotalAmount);

                    switch (touchingGoal.StatusId)
                    {
                        case TouchingGoalStatus.Nothing:
                            touchingGoal.PromotionReachedPercent += goal_quantityCondition.NotReachedPercent;

                            break;
                        case TouchingGoalStatus.Qunatity:
                            touchingGoal.PromotionReachedPercent += goal_quantityCondition.NotReachedPercent;
                            touchingGoal.ForthCasePromotionValue = goal_quantityCondition.ForthCasePercent * touchingGoal.TotalQuntitySales;
                            break;
                        case TouchingGoalStatus.Both:
                            touchingGoal.PromotionReachedPercent += goal_quantityCondition.ExtraEncouragePercent;
                            break;
                    }
                }

                var promotionValue = touchingGoal.TotalSales * touchingGoal.PromotionReachedPercent;
                //محاسبه پورسانت مبلغی
                touchingGoal.PromotionValue = Math.Round(((promotionValue + touchingGoal.ForthCasePromotionValue) * touchingGoal.Fulfillment_EncouragePercent) / 100, MidpointRounding.AwayFromZero);

            });

            //بدست پورسانت نهایی مرکز
            var lst_totalSalesPromotions = (from touch in lst_goal_branch_totalSales
                                            group touch by touch.BranchId into grpSale
                                            select new
                                            {
                                                BranchId = grpSale.Key,
                                                TotalValue = grpSale.Sum(x => x.PromotionValue)
                                            }).ToList();

            foreach (var brPromo in entity.BranchPromotions)
                brPromo.SalesPromotion = lst_totalSalesPromotions.FirstOrDefault(x => x.BranchId == brPromo.BranchId).TotalValue;


            return result;
        }
        private async Task<IBusinessResult> CalculateReceiptGoals(Promotion entity)
        {
            var result = new BusinessResult();

            // لیست اهداف وصول خصوصی / دولتی هر مرکز
            var lst_goal_branch_Goals = await (from g in unitOfWork.GoalDataService.GetQuery()
                                               .IncludeFilter(x => x.BranchGoals.Where(y => y.Deleted == false))
                                               .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false))
                                               .IncludeFilter(x => x.BranchReceiptGoalPercent.Where(y => y.Deleted == false))
                                               where g.IsUsed == false && g.Deleted == false
                                               && g.StartDate >= entity.StartDate && g.EndDate <= entity.EndDate
                                               && (g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal
                                               || g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal
                                               || g.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptGovernGoal)
                                               select g).ToListAsync();

            lst_goal_branch_Goals.ForEach(x =>
            {
                x.IsUsed = true;
                unitOfWork.GoalDataService.Update(x);
            });


            //لیست اهداف محقق شده وصول کل هر مرکز
            var lst_totalReceiptGoal_fulfillments = await (from gFull in unitOfWork.FulfillmentPercentDataService.GetQuery()
                                                           where gFull.Year == entity.Year && gFull.Month == entity.Month
                                                           && gFull.Deleted == false
                                                           select gFull).ToListAsync();

            //وصول هر مرکز
            var result_lst_branchReceipt = await branchReceiptBS.LoadBranchReceiptListAsync(entity.Year, entity.Month);
            if (result_lst_branchReceipt.ReturnStatus == false)
            {
                result.ReturnStatus = false;
                result.ReturnMessage.AddRange(result_lst_branchReceipt.ReturnMessage);
                return result;
            }

            var lst_branchReceipt = result_lst_branchReceipt.ResultValue;

            lst_branchReceipt.ForEach(braReceipt =>
            {
                var branchPromotion = entity.BranchPromotions.FirstOrDefault(x => x.BranchId == braReceipt.BranchId);
                
                var lst_branchGoals = (from goal in lst_goal_branch_Goals
                                       from branchG in goal.BranchGoals
                                       where branchG.BranchId == branchPromotion.BranchId
                                       select branchG).ToList();

                //درصد مشمول شرط تحقق
                var branchFulfillment = lst_totalReceiptGoal_fulfillments.SingleOrDefault(x => x.BranchId == branchPromotion.BranchId);
                decimal branchFulfillmentPercent = 0;
                //if (branchFulfillment != null)
                //    branchFulfillmentPercent = branchFulfillment.EncouragePercent.Value;

                //وصول کل
                var goalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.ReceiptTotalGoal;
                var branchGoal = lst_branchGoals.SingleOrDefault(x => x.Goal.GoalGoodsCategoryTypeId == goalGoodsCategoryTypeId);
                if (branchGoal != null)
                {
                    branchPromotion.TotalReceiptAmount = braReceipt.TotalAmount;
                    branchPromotion.TotalReceiptReachedPercent = getReceiptReachedPercent(branchGoal.Amount, goalGoodsCategoryTypeId, braReceipt);
                    branchPromotion.TotalReceiptPromotionPercent = getReceiptPromotion(branchPromotion.TotalReceiptReachedPercent > 100, branchGoal);
                    branchPromotion.TotalReceiptPromotion = branchPromotion.TotalReceiptAmount * branchPromotion.TotalReceiptPromotionPercent * branchFulfillmentPercent;
                }

                //وصول خصوصی
                goalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal;
                branchGoal = lst_branchGoals.SingleOrDefault(x => x.Goal.GoalGoodsCategoryTypeId == goalGoodsCategoryTypeId);
                if (branchGoal != null)
                {
                    branchPromotion.PrivateReceiptAmount = braReceipt.PrivateAmount.Value;
                    branchPromotion.PrivateReceiptReachedPercent = getReceiptReachedPercent(branchGoal.Amount, goalGoodsCategoryTypeId, braReceipt);
                    branchPromotion.PrivateReceiptPromotionPercent = getReceiptPromotion(branchPromotion.PrivateReceiptReachedPercent > 100, branchGoal);
                    branchPromotion.PrivateReceiptPromotion = branchPromotion.PrivateReceiptAmount * branchPromotion.PrivateReceiptPromotionPercent * branchFulfillmentPercent;
                }
                //Debug.WriteLine($"BranchId={receiptPromotion.BranchId},BranchName={lst_temp_branch.Find(x => x.Id == receiptPromotion.BranchId).Name}"
                //            + $",PrivateReceiptPercent={receiptPromotion.PrivateReceiptPercent},IsTouchedPrivate={receiptPromotion.IsTouchedPrivate},"
                //            + $",PrivatePromotionPercent={receiptPromotion.PrivatePromotionPercent},PrivatePromotion={receiptPromotion.PrivatePromotion},"
                //            + $"TotalReceiptPercent={receiptPromotion.TotalReceiptPercent},IsTouchedTotal={receiptPromotion.IsTouchedTotal}"
                //            + $",TotalPromotionPercent={receiptPromotion.TotalPromotionPercent},TotalPromotion={receiptPromotion.TotalPromotion},"
                //            + $"");
                
            });

            return result;

        }
        private decimal getReceiptPromotion(bool isReached, BranchGoal branchGoal)
        {
            var branchId = branchGoal.BranchId;
            var branchReceiptGoalPercent = branchGoal.Goal.BranchReceiptGoalPercent.SingleOrDefault(x => x.BranchId == branchId);
            if (branchReceiptGoalPercent != null)
            {
                if (isReached)
                    return branchReceiptGoalPercent.ReachedPercent;
                return branchReceiptGoalPercent.NotReachedPercent;
            }
            return 0;
        }
        private decimal getReceiptReachedPercent(decimal? amount, GoalGoodsCategoryTypeEnum goalGoodsCategoryTypeId, BranchReceipt braReceipt)
        {
            if (!amount.HasValue)
                return 0;
            decimal? receiptAmount = 0;
            switch (goalGoodsCategoryTypeId)
            {
                case GoalGoodsCategoryTypeEnum.ReceiptTotalGoal:
                    receiptAmount = braReceipt.TotalAmount;
                    break;
                case GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal:
                case GoalGoodsCategoryTypeEnum.ReceiptGovernGoal:
                    receiptAmount = braReceipt.PrivateAmount;
                    break;
            }
            return receiptAmount.HasValue ? Math.Round((receiptAmount.Value * 100) / amount.Value, 3) : 0;

        }

        #endregion
    }
}
