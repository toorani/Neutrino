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
        /// <summary>
        /// اطلاعات فروش کلی مراکز
        /// </summary>
        class TotalBranchSales
        {
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
            public int TotalNumber { get; set; }
        }
        ///// <summary>
        ///// اطلاعات فروش اهداف تعدادی
        ///// </summary>
        //class QuantityConditionSalesInfo
        //{
        //    /// <summary>
        //    //شناسه کالا
        //    /// </summary>
        //    public int GoodsId { get; set; }
        //    /// <summary>
        //    //شناسه مرکز
        //    /// </summary>
        //    public int BranchId { get; set; }
        //    /// <summary>
        //    //مبلغ فروش  محصول در مرکز
        //    /// </summary>
        //    public decimal Amount { get; set; }
        //    /// <summary>
        //    //تعداد فروش  محصول در مرکز
        //    /// </summary>
        //    public int Quantity { get; set; }
        //    /// <summary>
        //    /// آیا به  تعداد مشخص شده فروش داشته است
        //    /// </summary>
        //    public bool IsFulfilled { get; set; }
        //}

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
                && (x.GoalGoodsCategoryTypeId != GoalGoodsCategoryTypeEnum.Group
                && x.GoalGoodsCategoryTypeId != GoalGoodsCategoryTypeEnum.Single)
                , includes: x => new { x.BranchGoals, x.GoalSteps });

                var lst_totalFulfillPromotions = await unitOfWork.TotalFulfillPromotionPercentDataService.GetAllAsync();

                //calculate goals fulfillment
                var lst_GoalFulfillments = await CalculateFulfillmentPercent(entity, lstGoals, lst_totalFulfillPromotions);


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
        public async Task<IBusinessResult> CalculateSalesGoalsAsync(Promotion entity)
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
                //unitOfWork.PromotionDataService.Update(entity);

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
                //TODO : uncomment
                //lst_goals.ForEach(x =>
                //{
                //    x.IsUsed = true;
                //    unitOfWork.GoalDataService.Update(x);
                //});


                //لیست شرط تحقق فروش کل هر مرکز
                //var lst_fulfillmentPercents = await (from gFull in unitOfWork.FulfillmentPercentDataService.GetQuery()
                //                                     where gFull.Year == entity.Year && gFull.Month == entity.Month
                //                                     && gFull.Deleted == false
                //                                     select gFull).ToListAsync();
                GoalStep goalStep = null;

                //فعلا وجود ندارد
                lst_goals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Amount && x.QuantityConditions.Any(y => y.QuantityConditionTypeId == QuantityConditionTypeEnum.DependedOnGoal))
                    .ToList()
                    .ForEach(goal =>
                    {
                        //TODO 
                    });

                //هدف فروش ریالی به همراه اهداف تعدادی
                lst_goals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Amount
                && x.QuantityConditions.Any(y => y.QuantityConditionTypeId == QuantityConditionTypeEnum.Independent))
                    .ToList()
                    .ForEach(goal =>
                    {

                    });

                //اهداف با معیار تعدادی و ریالی بدون داشتن هدف تعدادی
                lst_goals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Quantities
                || (x.ComputingTypeId == ComputingTypeEnum.Amount && x.QuantityConditions.Count == 0))
                    .ToList()
                    .ForEach(goal =>
                    {
                        //به دلیل اینکه ممکن است اهدافی تعریف شود که طول زمان آن بیشتر از یکماه باشد 
                        //باید به ازای هر هدف و تاریخ آن فروش محاسبه شود
                        List<TotalBranchSales> lst_branch_salesInfo = getBranchSalesInfo(goal);


                        foreach (var branchGoal in goal.BranchGoals.Where(x => lst_branch_salesInfo.Any(y => y.BranchId == x.BranchId)))
                        {
                            //اطلاعات فروش مرکز
                            var branchSalesInfo = lst_branch_salesInfo.Single(x => x.BranchId == branchGoal.BranchId);

                            if (goal.ComputingTypeId == ComputingTypeEnum.Quantities)
                            {
                                //در صورتیکه معیار هدف تعدادی باشد ،باید تعداد فروش مقایسه شود
                                goalStep = goal.GoalSteps.OrderByDescending(step => step.ComputingValue)
                                .Where(step => (step.ComputingValue * branchGoal.Percent * 0.01M) <= branchSalesInfo.TotalNumber)
                                .FirstOrDefault();
                            }
                            else if (goal.ComputingTypeId == ComputingTypeEnum.Amount)
                            {
                                //در صورتیکه معیار هدف تعدادی باشد ،باید مقدار فروش مقایسه شود
                                goalStep = goal.GoalSteps.OrderByDescending(step => step.ComputingValue)
                                .Where(step => (step.ComputingValue * branchGoal.Percent * 0.01M) <= branchSalesInfo.TotalSales)
                                .FirstOrDefault();
                            }


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

                                }
                                else if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.Seller) //جایزه عوامل فروش
                                {

                                }

                                entity.BranchPromotions
                                .Single(x => x.BranchId == branchGoal.BranchId)
                                .BranchGoalPromotions
                                .Add(new BranchGoalPromotion { GoalId = goal.Id, PromotionWithOutFulfillmentPercent = promotion });
                            }
                            else if (goal.GoalNonFulfillmentPercents.Count != 0)
                            {
                                //در صورت عدم تحقق هدف ،چک کردن اینکه 'سهم مرکز در صورت عدم تحقق' تعریف شده است یا نه
                                var goalNonFulfillmentPercent = goal.GoalNonFulfillmentPercents.SingleOrDefault(x => x.GoalNonFulfillmentBranches.Any(y => y.BranchId == branchGoal.BranchId));
                                if (goalNonFulfillmentPercent != null)
                                {
                                    promotion = goalNonFulfillmentPercent.Percent * 0.01M * branchSalesInfo.TotalSales;
                                    entity.BranchPromotions
                                    .Single(x => x.BranchId == branchGoal.BranchId)
                                    .BranchGoalPromotions
                                    .Add(new BranchGoalPromotion { GoalId = goal.Id, PromotionWithOutFulfillmentPercent = promotion });
                                }
                            }
                        }
                    });
                lst_goals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Percentage && x.QuantityConditions.Count != 0)
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
                            var totalSales = lst_branchConditions.Sum(x => x.Amount);
                            decimal promotion = 0;
                            // هدف فروش زده است؟
                            if (goalStep != null)
                            {
                                var goalStepItem = goalStep.Items.Where(it => it.ActionTypeId == GoalStepActionTypeEnum.Reward).Single();

                                if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.Percent) //درصد پاداش
                                {
                                    promotion = goalStepItem.Amount.Value * 0.01M * totalSales;
                                }
                                else if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.SingleGoods) //جایزه عوامل فروش هر محصول
                                {

                                }
                                else if (goalStepItem.ItemTypeId == (int)RewardTypeEnum.Seller) //جایزه عوامل فروش
                                {

                                }

                                entity.BranchPromotions
                                .Single(x => x.BranchId == branchFulfillInfo.BranchId)
                                .BranchGoalPromotions
                                .Add(new BranchGoalPromotion { GoalId = goal.Id, PromotionWithOutFulfillmentPercent = promotion });
                            }
                            else if (goal.GoalNonFulfillmentPercents.Count != 0)
                            {
                                //در صورت عدم تحقق هدف ،چک کردن اینکه 'سهم مرکز در صورت عدم تحقق' تعریف شده است یا نه
                                var goalNonFulfillmentPercent = goal.GoalNonFulfillmentPercents.SingleOrDefault(x => x.GoalNonFulfillmentBranches.Any(y => y.BranchId == branchGoal.BranchId));
                                if (goalNonFulfillmentPercent != null)
                                {
                                    promotion = goalNonFulfillmentPercent.Percent * 0.01M * totalSales;
                                    entity.BranchPromotions
                                    .Single(x => x.BranchId == branchFulfillInfo.BranchId)
                                    .BranchGoalPromotions
                                    .Add(new BranchGoalPromotion { GoalId = goal.Id, PromotionWithOutFulfillmentPercent = promotion });
                                }
                            }
                        });



                    });

                await unitOfWork.CommitAsync();

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
                result = await CalculateSalesGoalsAsync(entity) as BusinessResult;
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
        class AggregationInfo
        {
            public int BranchId { get; set; }
            public decimal AggregationSalesAmount { get; set; }
            public double AggregationSalesNumber { get; set; }
            public AggregationInfo()
            {

            }
        }
        private async Task<List<FulfillmentPercent>> CalculateFulfillmentPercent(Promotion entity, List<Goal> lstGoals, List<FulfillmentPromotionCondition> lst_totalFulfillPromotion)
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
        private List<TotalBranchSales> getBranchSalesInfo(Goal goal)
        {
            return (from ggcg in unitOfWork.GoalGoodsCategoryGoodsDataService.GetQuery()
                    join brsa in unitOfWork.BranchSalesDataService.GetQuery()
                    on ggcg.GoodsId equals brsa.GoodsId
                    where brsa.StartDate >= goal.StartDate && brsa.EndDate <= goal.EndDate
                    && ggcg.GoalGoodsCategoryId == goal.GoalGoodsCategoryId
                    group brsa by new { brsa.BranchId } into grp
                    select new TotalBranchSales
                    {
                        BranchId = grp.Key.BranchId,
                        TotalSales = grp.Sum(x => x.TotalAmount),
                        TotalNumber = grp.Sum(x => x.TotalNumber),
                    }).ToList();
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
                    branchPromotion.TotalReceiptReachedPercent = getReceiptReachedPercent(branchGoal.Amount, goalGoodsCategoryTypeId, braReceipt);
                    //branchPromotion.TotalReceiptPromotionPercent = getReceiptPromotion(branchPromotion.TotalReceiptReachedPercent > 100, branchGoal);
                    //branchPromotion.TotalReceiptPromotion = branchPromotion.TotalReceiptAmount * branchPromotion.TotalReceiptPromotionPercent * branchFulfillmentPercent;
                    //branchPromotion.TotalReceiptPromotion = branchPromotion.TotalReceiptAmount * getReceiptPromotion(branchPromotion.TotalReceiptReachedPercent > 100, branchGoal) * branchFulfillmentPercent;
                }

                //وصول خصوصی
                goalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal;
                branchGoal = lst_branchGoals.SingleOrDefault(x => x.Goal.GoalGoodsCategoryTypeId == goalGoodsCategoryTypeId);
                if (branchGoal != null)
                {
                    branchPromotion.PrivateReceiptAmount = braReceipt.PrivateAmount.Value;
                    branchPromotion.PrivateReceiptReachedPercent = getReceiptReachedPercent(branchGoal.Amount, goalGoodsCategoryTypeId, braReceipt);
                    //branchPromotion.PrivateReceiptPromotionPercent = getReceiptPromotion(branchPromotion.PrivateReceiptReachedPercent > 100, branchGoal);
                    //branchPromotion.PrivateReceiptPromotion = branchPromotion.PrivateReceiptAmount * branchPromotion.PrivateReceiptPromotionPercent * branchFulfillmentPercent;
                    //branchPromotion.PrivateReceiptPromotion = branchPromotion.PrivateReceiptAmount * getReceiptPromotion(branchPromotion.PrivateReceiptReachedPercent > 100, branchGoal) * branchFulfillmentPercent;
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
