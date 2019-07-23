using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Entites;
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
            public decimal FulfillPercent { get; set; }
        }
        class AggregationInfo
        {
            public int BranchId { get; set; }
            public decimal AggregationSalesAmount { get; set; }
            public double AggregationSalesQuantity { get; set; }
        }
        class FulfillmentPromotionOption
        {
            /// <summary>
            ///  مقدار مشخص شده مرکز برای دستیابی به هدف وصول کل
            /// </summary>
            public decimal TotalReceiptSpecifiedAmount { get; set; }
            /// <summary>
            /// درصد دست یافته شده هدف وصول کل
            /// </summary>
            public decimal TotalReceiptReachedPercent { get; set; }
            /// <summary>
            ///  مقدار مشخص شده مرکز برای دستیابی به هدف وصول خصوصی
            /// </summary>
            public decimal PrivateReceiptSpecifiedAmount { get; set; }
            /// <summary>
            /// مبلغ دست یافته شده برای وصول خصوصی 
            /// </summary>
            public decimal PrivateReceiptAmount { get; set; }
            /// <summary>
            /// درصد دست یافته شده هدف وصول خصوصی
            /// </summary>
            public decimal PrivateReceiptReachedPercent { get; set; }
            /// <summary>
            ///  درصد دستیابی هدف کل فروش
            /// </summary>
            public decimal TotalSalesReachedPercent { get; set; }
            /// <summary>
            /// درصد مشخص شده مرکز برای هدف کل فروش
            /// </summary>
            public decimal TotalSalesSpecifiedPercent { get; set; }
            /// <summary>
            /// درصد مشخص شده مرکز برای هدف کل فروش
            /// </summary>
            public decimal TotalSalesSpecifiedAmount { get; set; }

            /// <summary>
            /// مبلغ کل تجمیعی
            /// </summary>
            public decimal AggregationSalesAmount { get; set; }
            /// <summary>
            ///  درصد دستیابی هدف تجمیعی
            /// </summary>
            public decimal AggregationReachedPercent { get; set; }

            /// <summary>
            /// مقدار مشخص شده مرکز برای هدف تجمیعی
            /// </summary>
            public decimal AggregationSpecifiedAmount { get; set; }

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
            , AbstractValidator<Promotion> validator
            )
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
                var result_supplier = await CalculateSupplierGoalsAsync(entity);
                var result_receipt = await CalculateReceiptGoalsAsync(entity);
                var result_branchSales = await CalculateBranchSalesAsync(entity);

                result.ReturnStatus = result_receipt.ReturnStatus & result_supplier.ReturnStatus & result_branchSales.ReturnStatus;
                if (result.ReturnStatus)
                {
                    entity = await unitOfWork.PromotionDataService.FirstOrDefaultAsync(x => x.Id == entity.Id);
                    entity.StatusId = PromotionStatusEnum.Done;
                    await unitOfWork.CommitAsync();
                }
                result.ReturnMessage.AddRange(result_receipt.ReturnMessage);
                result.ReturnMessage.AddRange(result_supplier.ReturnMessage);
                result.ReturnMessage.AddRange(result_branchSales.ReturnMessage);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        /// <summary>
        /// محاسبه پورسانت اهداف تامین کننده
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IBusinessResult> CalculateSupplierGoalsAsync(Promotion entity)
        {
            var result = new BusinessResult();
            try
            {
                if (entity.IsSupplierCalculated || entity.StatusId != PromotionStatusEnum.InProcessQueue)
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add("با توجه به وضعیت پورسانت امکان محاسبه پورسانت تامین کننده وجود ندارد");
                    return result;
                }

                entity = await unitOfWork.PromotionDataService.GetQuery()
                               .IncludeFilter(x => x.BranchPromotions.Where(c => c.Deleted == false))
                               .IncludeFilter(x => x.BranchPromotions.Select(c => c.MemberPromotions.Where(b => b.Deleted == false)))
                               .IncludeFilter(x => x.BranchPromotions.Select(c => c.MemberPromotions.Select(f => f.Details.Where(b => b.Deleted == false))))
                               .FirstOrDefaultAsync(x => x.Id == entity.Id);


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


                        });

                    //هدف فروش ریالی به همراه اهداف تعدادی
                    lst_branchGoals.Where(x => x.ComputingTypeId == ComputingTypeEnum.Amount
                    && x.QuantityConditions.Any(y => y.QuantityConditionTypeId == QuantityConditionTypeEnum.Independent))
                        .ToList()
                        .ForEach(goal =>
                        {
                            var quantityConditions = goal.QuantityConditions.Single();
                            //لیست فروش هر محصول در مرکز به همراه فیلد اینکه محصول به تعداد مورد نظر رسیده است یا خیر؟
                            var lst_branchConditions = (from gdsqc in quantityConditions.GoodsQuantityConditions
                                                        join brsa in (from brsag in unitOfWork.BranchSalesDataService.GetQuery()
                                                                      where brsag.SalesDate >= goal.StartDate && brsag.SalesDate <= goal.EndDate
                                                                      group brsag by new { brsag.GoodsId, brsag.BranchId } into grp_goods
                                                                      select new
                                                                      {
                                                                          grp_goods.Key.GoodsId,
                                                                          grp_goods.Key.BranchId,
                                                                          TotalAmount = grp_goods.Sum(x => x.TotalAmount),
                                                                          TotalNumber = grp_goods.Sum(x => x.TotalNumber)
                                                                      })
                                                        on gdsqc.GoodsId equals brsa.GoodsId
                                                        select new
                                                        {
                                                            brsa.BranchId,
                                                            brsa.GoodsId,
                                                            Amount = brsa.TotalAmount,
                                                            Quantity = brsa.TotalNumber,
                                                            BranchQuantity = gdsqc.BranchQuantityConditions.SingleOrDefault(x => x.BranchId == brsa.BranchId)?.Quantity,
                                                            IsGoodsFulfilled = (gdsqc.BranchQuantityConditions.SingleOrDefault(x => x.BranchId == brsa.BranchId)?.Quantity <= brsa.TotalNumber)
                                                        }).ToList();
                            //درصد دستیابی هر مرکز به هدف تعدادی
                            var lst_branchFulfillInfo = (from br_cd in lst_branchConditions
                                                         group br_cd by br_cd.BranchId into grp
                                                         select new
                                                         {
                                                             BranchId = grp.Key,
                                                             FulfilledCount = grp.Count(x => x.IsGoodsFulfilled),
                                                             IsFulFilled = grp.Count(x => x.IsGoodsFulfilled) >= quantityConditions.Quantity
                                                         }).ToList();

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
                                var computingValue = 0.0M;

                                if (goal.ComputingTypeId == ComputingTypeEnum.Quantities)
                                {
                                    //در صورتیکه معیار هدف تعدادی باشد ،باید تعداد فروش مقایسه شود
                                    computingValue = branchSalesInfo.TotalQuantity;
                                }
                                else if (goal.ComputingTypeId == ComputingTypeEnum.Amount)
                                {
                                    //در صورتیکه معیار هدف تعدادی باشد ،باید مقدار فروش مقایسه شود
                                    computingValue = branchSalesInfo.TotalSales;
                                }

                                goalStep = goal.GoalSteps.OrderByDescending(step => step.ComputingValue)
                                        .Where(step => (step.ComputingValue * branchGoal.Percent * 0.01M) <= computingValue)
                                        .FirstOrDefault();

                                if (goalStep != null)
                                {
                                    branchSalesInfo.FulfillPercent = (computingValue / (goalStep.ComputingValue * branchGoal.Percent.Value * 0.01M)) * 100;
                                }
                                else if (goal.GoalSteps.Count != 0)
                                {
                                    branchSalesInfo.FulfillPercent = (computingValue / (goal.GoalSteps.FirstOrDefault().ComputingValue * branchGoal.Percent.Value * 0.01M)) * 100;
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
                            //لیست فروش هر محصول در مرکز به همراه فیلد اینکه محصول به تعداد مورد نظر رسیده است یا خیر؟
                            var lst_branchConditions = (from gdsqc in quantityConditions.GoodsQuantityConditions
                                                        join brsa in (from brsag in unitOfWork.BranchSalesDataService.GetQuery()
                                                                      where brsag.SalesDate >= goal.StartDate && brsag.SalesDate <= goal.EndDate
                                                                      group brsag by new { brsag.GoodsId, brsag.BranchId } into grp_goods
                                                                      select new
                                                                      {
                                                                          grp_goods.Key.GoodsId,
                                                                          grp_goods.Key.BranchId,
                                                                          TotalAmount = grp_goods.Sum(x => x.TotalAmount),
                                                                          TotalNumber = grp_goods.Sum(x => x.TotalNumber)
                                                                      })
                                                        on gdsqc.GoodsId equals brsa.GoodsId
                                                        select new
                                                        {
                                                            brsa.BranchId,
                                                            brsa.GoodsId,
                                                            Amount = brsa.TotalAmount,
                                                            Quantity = brsa.TotalNumber,
                                                            FulfillPercent = ((decimal)brsa.TotalNumber / (decimal)gdsqc.BranchQuantityConditions.SingleOrDefault(x => x.BranchId == brsa.BranchId).Quantity) * 100M,
                                                            GoalQuantity = gdsqc.BranchQuantityConditions.SingleOrDefault(x => x.BranchId == brsa.BranchId).Quantity,
                                                        });


                            var lst_branchIds = lst_branchConditions.Select(x => x.BranchId).Distinct();
                            foreach (int branchId in lst_branchIds)
                            {
                                int count = 0;
                                goalStep = null;

                                foreach (var step in goal.GoalSteps.OrderByDescending(step => step.ComputingValue))
                                {
                                    count = lst_branchConditions.Count(x => x.FulfillPercent >= step.ComputingValue && x.BranchId == branchId);
                                    if (count >= quantityConditions.Quantity)
                                    {
                                        goalStep = step;
                                        break;
                                    }
                                }

                                BranchSalesInfo branchSalesInfo = new BranchSalesInfo
                                {
                                    //TotalSales = lst_branchConditions.GroupBy(x => new { x.BranchId, x.GoodsId }).Select(x => x.Sum(c => c.Amount)).FirstOrDefault(),
                                    TotalSales = lst_branchConditions.Where(x => x.BranchId == branchId).Sum(x => x.Amount),
                                    TotalQuantity = count,
                                    BranchId = branchId,
                                    FulfillPercent = goalStep != null ? goalStep.ComputingValue : 0M,
                                    SellerFulfillmentPercent = lst_fulfillmentPercents.SingleOrDefault(x => x.BranchId == branchId)?.SellerFulfillmentPercent
                                };
                                addBranchGoalPromotion(entity, goal, goalStep, branchSalesInfo);

                                var branchPromotion = entity.BranchPromotions.Single(x => x.BranchId == branchId);

                                lst_branchConditions
                                .Where(x => x.BranchId == branchId)
                                .Select(x => new QuantityGoalPromotion
                                {
                                    BranchId = branchId,
                                    BranchPromotionId = branchPromotion.Id,
                                    FulfilledPercent = x.FulfillPercent,
                                    GoodsId = x.GoodsId,
                                    GoalQuantity = x.GoalQuantity,
                                    TotalQuantity = x.Quantity,
                                    GoalId = goal.Id,
                                    TotalSales = x.Amount
                                })
                                .ToList()
                                .ForEach(branchPromotion.QuantityGoalPromotions.Add);

                            }

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
                                                   join invc in unitOfWork.InvoiceDataService.GetQuery()
                                                   on ggcg.GoodsId equals invc.GoodsId
                                                   join member in unitOfWork.MemberDataService.GetQuery()
                                                   on invc.SellerId equals member.Id
                                                   where invc.InvoiceDate >= goal.StartDate && invc.InvoiceDate <= goal.EndDate
                                                   && ggcg.GoalGoodsCategoryId == goal.GoalGoodsCategoryId
                                                   group new { invc, member } by new { invc.GoodsId, invc.SellerId, member.BranchId } into grp_expr
                                                   select new
                                                   {
                                                       grp_expr.Key.GoodsId,
                                                       grp_expr.Key.BranchId,
                                                       MemberId = grp_expr.Key.SellerId,
                                                       Quantity = grp_expr.Sum(x => x.invc.TotalCount),
                                                       Amount = grp_expr.Sum(x => x.invc.TotalAmount)
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
                                var branchPromotion = entity.BranchPromotions.Single(x => x.BranchId == memberSales.BranchId);

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
                                            promotion = goalStepItem.Amount.Value * (int)(memberSales.Amount / goalStepItem.EachValue.Value);
                                        }
                                        else if (goalStepItem.ComputingTypeId.Value == ComputingTypeEnum.Quantities)
                                        {
                                            //به ازای فروش این تعداد
                                            promotion = goalStepItem.Amount.Value * (int)(memberSales.Quantity / goalStepItem.EachValue.Value);
                                        }
                                    }


                                    branchPromotion.SellerPromotions.Add(new SellerPromotion
                                    {
                                        GoalId = goal.Id,
                                        Promotion = promotion,
                                        Quantity = memberSales.Quantity,
                                        Amount = memberSales.Amount,
                                        MemberId = memberSales.MemberId
                                    });


                                    //اضافه کردن پورسانت فروشنده
                                    addMemberPromotion(memberId: memberSales.MemberId, promotion: promotion, promotionType: PromotionType.Supplier, branchPromotion: branchPromotion);
                                }
                                else
                                {
                                    branchPromotion.SellerPromotions.Add(new SellerPromotion
                                    {
                                        GoalId = goal.Id,
                                        Promotion = 0,
                                        Quantity = memberSales.Quantity,
                                        Amount = memberSales.Amount,
                                        MemberId = memberSales.MemberId
                                    });

                                    //اضافه کردن پورسانت فروشنده
                                    addMemberPromotion(memberId: memberSales.MemberId, promotion: 0, promotionType: PromotionType.Supplier, branchPromotion: branchPromotion);
                                }

                            });
                        });
                }

                entity.IsSupplierCalculated = true;


                //محاسبه جمع پورسانت فروش هر مرکز
                (from brp in entity.BranchPromotions
                 from brglp in brp.BranchGoalPromotions
                 join gl in lst_goals
                 on brglp.Id equals gl.Id
                 group brglp by brglp.BranchId into grp_branch
                 select new
                 {
                     BranchId = grp_branch.Key,
                     TotalBranchPromotion = grp_branch.Sum(x => x.FinalPromotion)
                 }).ToList()
                .ForEach(total =>
                {
                    var branchPromotions = entity.BranchPromotions.FirstOrDefault(x => x.BranchId == total.BranchId);
                    branchPromotions.TotalSalesPromotion = total.TotalBranchPromotion;
                });

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

                entity = await unitOfWork.PromotionDataService.GetQuery()
                    .IncludeFilter(x => x.BranchPromotions.Where(c => c.Deleted == false))
                    .IncludeFilter(x => x.BranchPromotions.Select(z => z.BranchGoalPromotions.Where(c => c.Deleted == false)))
                    .IncludeFilter(x => x.BranchPromotions.Select(c => c.MemberPromotions.Where(b => b.Deleted == false)))
                    .IncludeFilter(x => x.BranchPromotions.Select(c => c.MemberPromotions.Select(f => f.Details.Where(b => b.Deleted == false))))
                    .FirstOrDefaultAsync(x => x.Id == entity.Id);

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

                    //decimal fulfilledPercent = 0;
                    //محاسبه پورسانت هدف وصول کل
                    //promotion = getReceiptPromotion(braReceipt, receiptTotalGoal, ref fulfilledPercent);
                    promotion = getReceiptPromotion(braReceipt, receiptTotalGoal);

                    //محاسبه سهم پورسانت پست های سازمانی از هدف وصول کل
                    BranchGoalPromotion receiptTotalGoalPromotion = branchPromotion.BranchGoalPromotions.FirstOrDefault(x => x.GoalId == receiptTotalGoal.Id && x.BranchId == braReceipt.BranchId);

                    receiptTotalGoalPromotion.FinalPromotion = promotion;
                    receiptTotalGoalPromotion.PromotionWithOutFulfillmentPercent = promotion;
                    receiptTotalGoalPromotion.PositionReceiptPromotions = lst_orgStructureShare_branch
                        .Where(x => x.TotalReceiptPercent != null)
                        .Select(x => new PositionReceiptPromotion
                        {
                            OrgStructureShareId = x.Id,
                            Promotion = x.TotalReceiptPercent.Value * promotion * 0.01M,
                            PositionTypeId = x.OrgStructure.PositionTypeId
                        }).ToList();

                    //
                    branchPromotion.TotalReceiptPromotion = promotion;


                    //محاسبه پورسانت هدف وصول خصوصی
                    promotion = getReceiptPromotion(braReceipt, receiptPrivateGoal);

                    //محاسبه سهم پورسانت پست های سازمانی از هدف وصول خصوصی
                    BranchGoalPromotion receiptPrivateGoalPromotion = branchPromotion.BranchGoalPromotions.FirstOrDefault(x => x.GoalId == receiptPrivateGoal.Id && x.BranchId == braReceipt.BranchId);

                    receiptPrivateGoalPromotion.FinalPromotion = promotion;
                    receiptPrivateGoalPromotion.PromotionWithOutFulfillmentPercent = promotion;
                    receiptPrivateGoalPromotion.PositionReceiptPromotions = lst_orgStructureShare_branch
                        .Where(x => x.PrivateReceiptPercent != null)
                        .Select(x => new PositionReceiptPromotion
                        {
                            OrgStructureShareId = x.Id,
                            Promotion = x.PrivateReceiptPercent.Value * promotion * 0.01M,
                            PositionTypeId = x.OrgStructure.PositionTypeId

                        }).ToList();

                    branchPromotion.PrivateReceiptPromotion = promotion;


                    //پورسانت اشخاص
                    var branchMembers = unitOfWork.MemberDataService.Get(x => x.BranchId == braReceipt.BranchId);

                    foreach (var item in receiptPrivateGoalPromotion.PositionReceiptPromotions)
                    {
                        if (branchMembers.Count(x => x.PositionTypeId == item.PositionTypeId) == 1)
                        {
                            var member = branchMembers.Single(x => x.PositionTypeId == item.PositionTypeId);
                            addMemberPromotion(member.Id, item.Promotion, PromotionType.Receipt, branchPromotion);
                        }
                    }
                    foreach (var item in receiptTotalGoalPromotion.PositionReceiptPromotions)
                    {
                        if (branchMembers.Count(x => x.PositionTypeId == item.PositionTypeId) == 1)
                        {
                            var member = branchMembers.Single(x => x.PositionTypeId == item.PositionTypeId);
                            addMemberPromotion(member.Id, item.Promotion, PromotionType.Receipt, branchPromotion);
                        }
                    }

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
        /// <summary>
        /// محاسبه پورسانت فروش کل مراکز
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IBusinessResult> CalculateBranchSalesAsync(Promotion entity)
        {
            var result = new BusinessResult();
            try
            {
                if (entity.StatusId != PromotionStatusEnum.InProcessQueue || entity.IsBranchSalesCalculated)
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add("با توجه به وضعیت پورسانت امکان محاسبه پورسانت فروش مرکز وجود ندارد");
                    return result;
                }

                entity = await unitOfWork.PromotionDataService.GetQuery()
                    .IncludeFilter(x => x.BranchPromotions.Where(c => c.Deleted == false))
                    .IncludeFilter(x => x.BranchPromotions.Select(c => c.MemberPromotions.Where(b => b.Deleted == false)))
                    .IncludeFilter(x => x.BranchPromotions.Select(c => c.MemberPromotions.Select(f => f.Details.Where(b => b.Deleted == false))))
                    .FirstOrDefaultAsync(x => x.Id == entity.Id);

                var lst_totalBranchSales = await (from brsal in unitOfWork.BranchSalesDataService.GetQuery()
                                                  where brsal.SalesDate >= entity.StartDate && brsal.SalesDate <= entity.EndDate
                                                  && brsal.Deleted == false
                                                  group brsal by brsal.BranchId into grp_expr
                                                  select new
                                                  {
                                                      BranchId = grp_expr.Key,
                                                      TotalSales = grp_expr.Sum(x => x.TotalAmount)
                                                  }).ToListAsync();




                //لیست سهم پست های سازمانی مراکز
                var lst_orgStructureShare = await unitOfWork.OrgStructureShareDataService.GetAsync(x => x.SalesPercent != null, includes: x => new { x.OrgStructure });



                decimal promotion = 0;
                BranchPromotion branchPromotion = new BranchPromotion();
                List<OrgStructureShare> lst_orgStructureShare_branch = new List<OrgStructureShare>();
                lst_totalBranchSales.ForEach(braSales =>
                {
                    branchPromotion = entity.BranchPromotions.Single(x => x.BranchId == braSales.BranchId);

                    //لیست سهم پست های سازمانی برای مرکز
                    lst_orgStructureShare_branch = lst_orgStructureShare.Where(x => x.BranchId == braSales.BranchId).ToList();
                    var operation_employeePercent = lst_orgStructureShare_branch.FirstOrDefault(x => x.OrgStructure.PositionTypeId == PositionTypeEnum.OperationEmployee);
                    branchPromotion.Budget = braSales.TotalSales * operation_employeePercent?.SalesPercent.Value ?? 0;


                    var lst_operationEmployee = unitOfWork.MemberDataService.Get(mem => mem.IsActive && !mem.Deleted
                                                       && (Member.OPERATION_EMPLOYEE_POSITION.Contains(mem.PositionTypeId.Value)
                                                       || mem.PositionTypeId == PositionTypeEnum.OperationManager)
                                                       && mem.BranchId == braSales.BranchId).ToList();

                    if (branchPromotion.Budget != 0 && lst_operationEmployee.Count != 0)
                        promotion = branchPromotion.Budget / lst_operationEmployee.Count;
                    else
                        promotion = 0;

                    lst_operationEmployee.Select(x => new OperationPromotion
                    {
                        MemberId = x.Id,
                        Promotion = promotion
                    }).ToList()
                    .ForEach(x =>
                    {
                        branchPromotion.OperationPromotions.Add(x);
                        addMemberPromotion(x.MemberId, x.Promotion, PromotionType.BranchTotalSales, branchPromotion);
                    });
                });
                entity.IsBranchSalesCalculated = true;
                await unitOfWork.CommitAsync();


                result.ReturnStatus = true;
                result.ReturnMessage.Add("محاسبه پورسانت فروش مراکز با موفقیت پایان یافت");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        /// <summary>
        /// محاسبه جمع پورسانت فروش هر مرکز 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IBusinessResult> CalculateTotalSalesBranchPromotionValueAsync(Promotion entity)
        {
            var result = new BusinessResult();
            try
            {
                var query = await (from brp in unitOfWork.BranchPromotionDataService.GetQuery()
                                   join brglp in unitOfWork.BranchGoalPromotionDataService.GetQuery()
                                   on brp.Id equals brglp.BranchPromotionId
                                   where brp.Month == entity.Month && brp.Year == entity.Year && brp.Deleted == false
                                   join gl in unitOfWork.GoalDataService.GetQuery()
                                   on brglp.GoalId equals gl.Id
                                   where (gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Group || gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Single)
                                   && gl.ApprovePromotionTypeId == ApprovePromotionTypeEnum.Branch
                                   group brglp by brglp.BranchId into grp_expr
                                   select new
                                   {
                                       BranchId = grp_expr.Key,
                                       TotalPromotion = grp_expr.Sum(x => x.FinalPromotion)
                                   }).ToListAsync();
                var branchPromotions = await unitOfWork.BranchPromotionDataService.GetAsync(x => x.Month == entity.Month && x.Year == entity.Year);
                branchPromotions.ForEach(brp =>
                {
                    brp.TotalSalesPromotion = query.Where(x => x.BranchId == brp.BranchId).Sum(x => x.TotalPromotion);
                    unitOfWork.BranchPromotionDataService.Update(brp);
                });

                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<ReportBranchPromotionOverview>>> LoadReportOverView(int year, int month)
        {
            var result = new BusinessResultValue<List<ReportBranchPromotionOverview>>();
            try
            {
                GoalGoodsCategoryTypeEnum[] goalGoodsCategoryTypes =
                {
                    GoalGoodsCategoryTypeEnum.TotalSalesGoal
                    ,GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal
                    ,GoalGoodsCategoryTypeEnum.ReceiptTotalGoal
                    ,GoalGoodsCategoryTypeEnum.AggregationGoal
                };

                var query = from brp in unitOfWork.BranchPromotionDataService.GetQuery()
                            join brgp in unitOfWork.BranchGoalPromotionDataService.GetQuery()
                            on brp.Id equals brgp.BranchPromotionId
                            where brp.Month == month && brp.Year == year
                            join gl in unitOfWork.GoalDataService.GetQuery()
                            on brgp.GoalId equals gl.Id
                            where goalGoodsCategoryTypes.Contains(gl.GoalGoodsCategoryTypeId)
                            join br in unitOfWork.BranchDataService.GetQuery()
                            on brgp.BranchId equals br.Id
                            orderby br.Order
                            select new
                            {
                                brgp.BranchId,
                                BranchTitle = br.Name,
                                gl.GoalGoodsCategoryTypeId,
                                brgp.TotalSales,
                                brgp.FulfilledPercent
                            };

                result.ResultValue = await (from q in query
                                            group q by new { q.BranchTitle, q.BranchId } into grp_data
                                            select new ReportBranchPromotionOverview
                                            {
                                                BranchId = grp_data.Key.BranchId,
                                                BranchName = grp_data.Key.BranchTitle,
                                                Month = month,
                                                Year = year,
                                                PrivateFulfilledPercent = grp_data.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal).FulfilledPercent,
                                                TotalReceiptFulfilledPercent = grp_data.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptTotalGoal).FulfilledPercent,
                                                TotalSalesFulfilledPercent = grp_data.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.TotalSalesGoal).FulfilledPercent,
                                                AggregationFulfilledPercent = grp_data.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.AggregationGoal) != null ?
                                                grp_data.FirstOrDefault(x => x.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.AggregationGoal).FulfilledPercent : 0
                                            }).ToListAsync();



            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<ReportBranchSalesGoal>>> LoadReport_Amount_Quantities_Goal(DateTime startDate, DateTime endDate, int goalGoodsCategoryId)
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
                                   on new { brgl.GoalId, brgl.BranchId } equals new { brglp.GoalId, brglp.BranchId }
                                   where (gl.ComputingTypeId == ComputingTypeEnum.Amount ||
                                   gl.ComputingTypeId == ComputingTypeEnum.Quantities)
                                   && gl.GoalGoodsCategoryId == goalGoodsCategoryId
                                   && gl.StartDate >= startDate && gl.EndDate <= endDate
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
                                       ApprovePromotionTypeId = gl.ApprovePromotionTypeId.Value,
                                       AmountSpecified = brgl.Percent.HasValue ? brgl.Percent.Value * 0.01M * gls.ComputingValue : brgl.Amount.Value
                                   }).ToListAsync();

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
                    ApprovePromotionTypeId = x.ApprovePromotionTypeId,
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
        public async Task<IBusinessResultValue<List<ReportBranchSalesGoal>>> LoadReport_Percent_Goal(DateTime startDate, DateTime endDate, int goalGoodsCategoryId)
        {
            var result = new BusinessResultValue<List<ReportBranchSalesGoal>>();
            try
            {
                var query = await (from gl in unitOfWork.GoalDataService.GetQuery()
                                   join brglp in unitOfWork.BranchGoalPromotionDataService.GetQuery()
                                   on gl.Id equals brglp.GoalId
                                   join br in unitOfWork.BranchDataService.GetQuery()
                                   on brglp.BranchId equals br.Id
                                   where gl.GoalGoodsCategoryId == goalGoodsCategoryId
                                   && gl.StartDate >= startDate && gl.EndDate <= endDate
                                   select new
                                   {
                                       gl.ComputingTypeId,
                                       ApprovePromotionTypeId = gl.ApprovePromotionTypeId.Value,
                                       BranchName = br.Name,
                                       brglp.TotalQuantity,
                                       brglp.TotalSales,
                                       brglp.FulfilledPercent,
                                       brglp.PromotionWithOutFulfillmentPercent,
                                       brglp.FinalPromotion
                                   }).ToListAsync();

                result.ResultValue = query.Select(x => new ReportBranchSalesGoal
                {
                    BranchName = x.BranchName,
                    PromotionWithOutFulfillmentPercent = x.PromotionWithOutFulfillmentPercent,
                    TotalSales = x.TotalSales,
                    TotalQuantity = x.TotalQuantity,
                    ComputingTypeId = x.ComputingTypeId,
                    ApprovePromotionTypeId = x.ApprovePromotionTypeId,
                    FinalPromotion = x.FinalPromotion,
                    FulfilledPercent = x.FulfilledPercent
                }).ToList();

                result.ReturnStatus = true;

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<ReportSellerGoal>>> LoadReport_Seller_Goal(DateTime startDate, DateTime endDate, int goalGoodsCategoryId)
        {
            var result = new BusinessResultValue<List<ReportSellerGoal>>();
            try
            {
                var query = await (from gl in unitOfWork.GoalDataService.GetQuery()
                                   join glst in unitOfWork.GoalStepDataService.GetQuery()
                                   on gl.Id equals glst.GoalId
                                   join mrp in unitOfWork.SellerPromotionDataService.GetQuery()
                                   on gl.Id equals mrp.GoalId
                                   join mr in unitOfWork.MemberDataService.GetQuery()
                                   on mrp.MemberId equals mr.Id
                                   join br in unitOfWork.BranchDataService.GetQuery()
                                   on mr.BranchId equals br.Id
                                   where gl.GoalGoodsCategoryId == goalGoodsCategoryId
                                   && gl.StartDate >= startDate && gl.EndDate <= endDate
                                   select new
                                   {
                                       gl.ComputingTypeId,
                                       ApprovePromotionTypeId = gl.ApprovePromotionTypeId.Value,
                                       SellerName = mr.Name + " " + mr.LastName,
                                       mrp.Amount,
                                       mrp.Quantity,
                                       mrp.Promotion,
                                       glst.ComputingValue,
                                       BranchName = br.Name,
                                   }).ToListAsync();

                result.ResultValue = query.Distinct().Select(x => new ReportSellerGoal
                {
                    SellerName = x.SellerName,
                    TotalSales = x.Amount,
                    TotalQuantity = x.Quantity,
                    ComputingTypeId = x.ComputingTypeId,
                    ApprovePromotionTypeId = x.ApprovePromotionTypeId,
                    FinalPromotion = x.Promotion,
                    ComputingValue = x.ComputingValue,
                    BranchName = x.BranchName,
                    FulfilledPercent = Math.Round(x.ComputingTypeId == ComputingTypeEnum.Amount ? (x.Amount * 100) / x.ComputingValue : (x.Quantity * 100) / x.ComputingValue, MidpointRounding.AwayFromZero)
                }).ToList();

                result.ReturnStatus = true;

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<ReportBranchPromotionDetail>>> LoadReportBranchPromotionDetail(DateTime startDate, DateTime endDate, params GoalGoodsCategoryTypeEnum[] goalGoodsCategoryTypeIds)
        {
            var result = new BusinessResultValue<List<ReportBranchPromotionDetail>>();
            try
            {
                int len = goalGoodsCategoryTypeIds.Length;

                var query = await (from gl in unitOfWork.GoalDataService.GetQuery()
                                   join brglp in unitOfWork.BranchGoalPromotionDataService.GetQuery()
                                   on gl.Id equals brglp.GoalId
                                   join br in unitOfWork.BranchDataService.GetQuery()
                                   on brglp.BranchId equals br.Id
                                   join ggc in unitOfWork.GoalGoodsCategoryDataService.GetQuery()
                                   on gl.GoalGoodsCategoryId equals ggc.Id
                                   join brp in unitOfWork.BranchPromotionDataService.GetQuery()
                                   on brglp.BranchPromotionId equals brp.Id
                                   join ffp in unitOfWork.FulfillmentPercentDataService.GetQuery()
                                   on new { brglp.BranchId, brp.Month, brp.Year } equals new { ffp.BranchId, ffp.Month, ffp.Year }
                                   where gl.StartDate >= startDate && gl.EndDate <= endDate
                                   && (len == 0 || goalGoodsCategoryTypeIds.Contains(gl.GoalGoodsCategoryTypeId))
                                   && gl.Deleted == false && gl.IsUsed == true
                                   select new
                                   {
                                       BranchId = br.Id,
                                       BranchName = br.Name,
                                       GoalGoodsCategoryName = ggc.Name,
                                       brglp.FinalPromotion,
                                       brglp.PromotionWithOutFulfillmentPercent,
                                       SellerFulfillmentPercent = ffp.SellerFulfillmentPercent.Value,
                                       ManagerFulfillmentPercent = ffp.ManagerFulfillmentPercent.Value
                                   }).ToListAsync();

                result.ResultValue = query.Select(x => new ReportBranchPromotionDetail
                {
                    BranchId = x.BranchId,
                    BranchName = x.BranchName,
                    FinalPromotion = x.FinalPromotion,
                    GoalGoodsCategoryName = x.GoalGoodsCategoryName,
                    ManagerFulfillmentPercent = x.ManagerFulfillmentPercent,
                    PromotionWithOutFulfillmentPercent = x.PromotionWithOutFulfillmentPercent,
                    SellerFulfillmentPercent = x.SellerFulfillmentPercent,
                    TotalFinalPromotion = query.Where(y => y.BranchId == x.BranchId).Sum(y => y.FinalPromotion),
                    TotalPromotionWithOutFulfillmentPercent = query.Where(y => y.BranchId == x.BranchId).Sum(y => y.PromotionWithOutFulfillmentPercent)
                }).ToList();
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<ReportBranchReceiptGoal>>> LoadReportBranchReceipt(int year, int month, GoalGoodsCategoryTypeEnum goalGoodsCategoryTypeId)
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
                                   on new { brgl.GoalId, brgl.BranchId } equals new { brglp.GoalId, brglp.BranchId }
                                   join prp in unitOfWork.PositionReceiptPromotionDataService.GetQuery()
                                   on brglp.Id equals prp.BranchGoalPromotionId
                                   join brr in unitOfWork.BranchReceiptDataService.GetQuery()
                                   on br.Id equals brr.BranchId
                                   where brr.Year == year && brr.Month == month
                                   join pt in unitOfWork.PositionTypeDataService.GetQuery()
                                   on prp.PositionTypeId equals pt.eId
                                   where gl.GoalTypeId == GoalTypeEnum.Distributor &&
                                   gl.GoalGoodsCategoryTypeId == goalGoodsCategoryTypeId
                                   && gl.Month == month && gl.Year == year
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
                    FulfilledPercent = ((goalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal ? x.ReceiptPrivateAmount.Value : x.ReceiptTotalAmount) * 100) / x.AmountSpecified,
                    PositionPromotion = x.Promotion,
                    PositionTitle = x.PositionTitle,
                    ReceiptAmount = goalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.ReceiptPrivateGoal ? x.ReceiptPrivateAmount.Value : x.ReceiptTotalAmount,
                    GoalGoodsCategoryName = goalGoodsCategoryTypeId.GetEnumDescription()
                }).ToList();
                result.ReturnStatus = true;

            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<List<BranchPromotion>>> LoadBranchPromotions(PromotionReviewStatusEnum promotionReviewStatusId)
        {
            var result = new BusinessResultValue<List<BranchPromotion>>();
            try
            {
                result.ResultValue = await unitOfWork.BranchPromotionDataService.GetAsync(x => x.PromotionReviewStatusId == promotionReviewStatusId
                , includes: x => x.Branch);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<BranchPromotion>> LoadBranchPromotion(int branchId, PromotionReviewStatusEnum statusId)
        {
            var result = new BusinessResultValue<BranchPromotion>();
            try
            {
                result.ResultValue = await unitOfWork.BranchPromotionDataService.FirstOrDefaultAsync(x => x.BranchId == branchId && x.PromotionReviewStatusId == statusId
                , includes: x => x.Branch);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<MemberSales_PromotionInfo>> LoadMemberSales_PromotionInfoAsync(int memberId, int month, int year)
        {
            var result = new BusinessResultValue<MemberSales_PromotionInfo>();
            try
            {
                // پورسانت عوامل فروش
                var query_mpromotion = from mep in unitOfWork.SellerPromotionDataService.GetQuery()
                                       join brp in unitOfWork.BranchPromotionDataService.GetQuery()
                                       on mep.BranchPromotionId equals brp.Id
                                       where mep.MemberId == memberId && brp.Month == month && brp.Year == year && mep.Deleted == false
                                       group mep by mep.MemberId into grp_memp
                                       select new
                                       {
                                           MemberId = grp_memp.Key,
                                           Promotion = grp_memp.Sum(x => x.Promotion)
                                       };
                // فروش عوامل فروش
                var query_msales = from mes in unitOfWork.InvoiceDataService.GetQuery()
                                   where mes.SellerId == memberId && mes.Month == month && mes.Year == year
                                   && mes.Deleted == false
                                   group mes by mes.SellerId into grp_mes
                                   select new
                                   {
                                       MemberId = grp_mes.Key,
                                       TotalAmount = grp_mes.Sum(x => x.TotalAmount),
                                       TotalCount = grp_mes.Sum(x => x.TotalCount),
                                   };
                result.ResultValue = await (from mep in query_mpromotion
                                            join mesal in query_msales
                                            on mep.MemberId equals mesal.MemberId
                                            select new MemberSales_PromotionInfo
                                            {
                                                MemberId = memberId,
                                                Promotion = mep.Promotion,
                                                TotalAmount = mesal.TotalAmount,
                                                TotalQauntity = mesal.TotalCount
                                            }).FirstOrDefaultAsync();

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
                                           AggregationSalesQuantity = grp.Sum(x => x.TotalNumber)
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
                                                                  TotalQuantity = grp.Sum(x => x.TotalNumber)
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
                                                   TotalSalesQuantity = brSal.TotalQuantity,
                                               }).ToListAsync();


            var lst_fulfillmentPercent = new List<FulfillmentPercent>();

            BranchPromotion branchPromotion;
            FulfillmentPromotionOption fulfillmentPromotionOption;
            query_branchPromotion.ForEach(queryRecord =>
            {
                branchPromotion = new BranchPromotion { Year = entity.Year, Month = entity.Month, BranchId = queryRecord.BranchId, PromotionReviewStatusId = PromotionReviewStatusEnum.WaitingForCompensatory };
                fulfillmentPromotionOption = new FulfillmentPromotionOption();
                //Aggregation data
                if (aggregationGoal != null)
                {
                    AggregationInfo aggregationInfo = lst_aggregationInfo.SingleOrDefault(x => x.BranchId == queryRecord.BranchId);
                    if (aggregationInfo != null)
                    {
                        fulfillmentPromotionOption.AggregationSpecifiedAmount = aggregationGoal.BranchGoals.Single(bg => bg.BranchId == queryRecord.BranchId).Amount.Value;
                        fulfillmentPromotionOption.AggregationReachedPercent = Math.Round(aggregationInfo.AggregationSalesAmount / fulfillmentPromotionOption.AggregationSpecifiedAmount * 100, MidpointRounding.AwayFromZero);
                        fulfillmentPromotionOption.AggregationSalesAmount = aggregationInfo.AggregationSalesAmount;

                        branchPromotion.BranchGoalPromotions.Add(new BranchGoalPromotion
                        {
                            BranchId = queryRecord.BranchId,
                            GoalId = aggregationGoal.Id,
                            FulfilledPercent = fulfillmentPromotionOption.AggregationReachedPercent,
                            TotalSales = aggregationInfo.AggregationSalesAmount,
                            PromotionWithOutFulfillmentPercent = 0,
                            TotalQuantity = (int)aggregationInfo.AggregationSalesQuantity,
                            FinalPromotion = 0
                        });
                    }



                }

                //Total sales data
                fulfillmentPromotionOption.TotalSalesSpecifiedPercent = totalSalesGoal.BranchGoals.Single(bg => bg.BranchId == queryRecord.BranchId).Percent.Value;
                fulfillmentPromotionOption.TotalSalesSpecifiedAmount = (totalSalesGoalAmount * fulfillmentPromotionOption.TotalSalesSpecifiedPercent) / 100;
                fulfillmentPromotionOption.TotalSalesReachedPercent = Math.Round((queryRecord.TotalSalesAmount / fulfillmentPromotionOption.TotalSalesSpecifiedAmount) * 100, MidpointRounding.AwayFromZero);

                branchPromotion.BranchGoalPromotions.Add(new BranchGoalPromotion
                {
                    BranchId = queryRecord.BranchId,
                    GoalId = totalSalesGoal.Id,
                    FulfilledPercent = fulfillmentPromotionOption.TotalSalesReachedPercent,
                    TotalSales = queryRecord.TotalSalesAmount,
                    PromotionWithOutFulfillmentPercent = 0,
                    TotalQuantity = queryRecord.TotalSalesQuantity,
                    FinalPromotion = 0
                });

                //total receipt
                fulfillmentPromotionOption.TotalReceiptSpecifiedAmount = receiptTotalGoal.BranchGoals.Single(bg => bg.BranchId == queryRecord.BranchId).Amount.Value;
                fulfillmentPromotionOption.TotalReceiptReachedPercent = Math.Round((queryRecord.TotalReceiptAmount / fulfillmentPromotionOption.TotalReceiptSpecifiedAmount) * 100, MidpointRounding.AwayFromZero);

                branchPromotion.BranchGoalPromotions.Add(new BranchGoalPromotion
                {
                    BranchId = queryRecord.BranchId,
                    GoalId = receiptTotalGoal.Id,
                    FulfilledPercent = fulfillmentPromotionOption.TotalReceiptReachedPercent,
                    TotalSales = queryRecord.TotalReceiptAmount,
                    PromotionWithOutFulfillmentPercent = 0,
                    TotalQuantity = 0,
                    FinalPromotion = 0
                });

                //private receipt
                fulfillmentPromotionOption.PrivateReceiptSpecifiedAmount = receiptPrivateGoal.BranchGoals.Single(bg => bg.BranchId == queryRecord.BranchId).Amount.Value;
                fulfillmentPromotionOption.PrivateReceiptReachedPercent = Math.Round(queryRecord.PrivateReceiptAmount / fulfillmentPromotionOption.PrivateReceiptSpecifiedAmount * 100, MidpointRounding.AwayFromZero);

                branchPromotion.BranchGoalPromotions.Add(new BranchGoalPromotion
                {
                    BranchId = queryRecord.BranchId,
                    GoalId = receiptPrivateGoal.Id,
                    FulfilledPercent = fulfillmentPromotionOption.TotalReceiptReachedPercent,
                    TotalSales = queryRecord.PrivateReceiptAmount,
                    PromotionWithOutFulfillmentPercent = 0,
                    TotalQuantity = 0,
                    FinalPromotion = 0
                });

                //maximum value between aggregation and total sales
                var max_totalSales_aggregation = fulfillmentPromotionOption.AggregationReachedPercent > fulfillmentPromotionOption.TotalSalesReachedPercent ? fulfillmentPromotionOption.AggregationReachedPercent : fulfillmentPromotionOption.TotalSalesReachedPercent;


                var fulfillmentPromotion_manager = lst_totalFulfillPromotion
                    .OrderByDescending(x => x.TotalSalesFulfilledPercent)
                    .FirstOrDefault(x => x.TotalSalesFulfilledPercent <= max_totalSales_aggregation
                    && x.TotalReceiptFulfilledPercent <= fulfillmentPromotionOption.TotalReceiptReachedPercent
                    && x.PrivateReceiptFulfilledPercent <= fulfillmentPromotionOption.PrivateReceiptReachedPercent
                    && x.ManagerPromotion != null);

                var fulfillmentPromotion_seller = lst_totalFulfillPromotion
                    .OrderByDescending(x => x.TotalSalesFulfilledPercent)
                    .FirstOrDefault(x => x.TotalSalesFulfilledPercent <= max_totalSales_aggregation
                    && x.TotalReceiptFulfilledPercent <= fulfillmentPromotionOption.TotalReceiptReachedPercent
                    && x.PrivateReceiptFulfilledPercent <= fulfillmentPromotionOption.PrivateReceiptReachedPercent
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
                    TotalReceiptFulfilledPercent = fulfillmentPromotionOption.TotalReceiptReachedPercent,
                    PrivateReceiptFulfilledPercent = fulfillmentPromotionOption.PrivateReceiptReachedPercent,
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
                         where brsa.SalesDate >= goal.StartDate && brsa.SalesDate <= goal.EndDate
                         && ggcg.GoalGoodsCategoryId == goal.GoalGoodsCategoryId
                         group brsa by brsa.BranchId into grp
                         select new
                         {
                             BranchId = grp.Key,
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
            var branchPromotion = entity.BranchPromotions.Single(x => x.BranchId == branchSalesInfo.BranchId);

            var branchGoalPromotion = new BranchGoalPromotion
            {
                GoalId = goal.Id,
                BranchId = branchSalesInfo.BranchId,
                TotalSales = branchSalesInfo.TotalSales,
                TotalQuantity = branchSalesInfo.TotalQuantity,
                FulfilledPercent = branchSalesInfo.FulfillPercent,
            };

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


            }
            else if (goal.GoalNonFulfillmentPercents.Count != 0)
            {
                //در صورت عدم تحقق هدف ،چک کردن اینکه 'سهم مرکز در صورت عدم تحقق' تعریف شده است یا نه
                var goalNonFulfillmentPercent = goal.GoalNonFulfillmentPercents.SingleOrDefault(x => x.GoalNonFulfillmentBranches.Any(y => y.BranchId == branchSalesInfo.BranchId));
                if (goalNonFulfillmentPercent != null)
                {
                    promotion = goalNonFulfillmentPercent.Percent * 0.01M * branchSalesInfo.TotalSales;
                }
            }

            branchGoalPromotion.PromotionWithOutFulfillmentPercent = promotion;
            branchGoalPromotion.FinalPromotion = branchSalesInfo.SellerFulfillmentPercent.HasValue ? promotion * branchSalesInfo.SellerFulfillmentPercent.Value * 0.01M : promotion;


            branchPromotion.BranchGoalPromotions.Add(branchGoalPromotion);
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

            //fulfilledPerecnt = (amount / receiptTotal_branchGoal.Amount.Value) * 100;
            return promotion;
        }
        enum PromotionType
        {
            Supplier,
            Receipt,
            BranchTotalSales
        }
        private void addMemberPromotion(int memberId, decimal promotion, PromotionType promotionType, BranchPromotion branchPromotion)
        {
            var memberPromotion = branchPromotion.MemberPromotions.SingleOrDefault(x => x.MemberId == memberId);

            if (memberPromotion != null)
            {
                memberPromotion.Promotion += promotion;
                var detailPromotion = memberPromotion.Details.FirstOrDefault();
                switch (promotionType)
                {
                    case PromotionType.Supplier:
                        detailPromotion.SupplierPromotion += promotion;
                        break;
                    case PromotionType.Receipt:
                        detailPromotion.ReceiptPromotion += promotion;
                        break;
                    case PromotionType.BranchTotalSales:
                        detailPromotion.BranchSalesPromotion += promotion;
                        break;
                }

            }
            else
            {
                memberPromotion = new MemberPromotion
                {
                    MemberId = memberId,
                    Promotion = promotion
                };

                memberPromotion.Details.Add(new MemberPromotionDetail
                {
                    BranchSalesPromotion = promotionType == PromotionType.BranchTotalSales ? promotion : 0,
                    ReceiptPromotion = promotionType == PromotionType.Receipt ? promotion : 0,
                    SupplierPromotion = promotionType == PromotionType.Supplier ? promotion : 0,
                    CompensatoryPromotion = 0,
                    MemberId = memberId,
                    ReviewPromotionStepId = ReviewPromotionStepEnum.Initial,
                });
                branchPromotion.MemberPromotions.Add(memberPromotion);
            }
        }
        #endregion
    }
}
