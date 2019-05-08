using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Entites;
using FluentValidation;
using FluentValidation.Results;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;

namespace Neutrino.Business
{
    public class GoalBS : NeutrinoBusinessService, IGoalBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<Goal> validator;
        #endregion

        #region [ Public Property(ies) ]
        [Inject]
        public IEntityLoader<Goal> EntityLoader { get; set; }
        [Inject]
        public IEntityListByPagingLoader<Goal> EntityListByPagingLoader { get; set; }
        [Inject]
        public IEntityListLoader<Goal> EntityListLoader { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public GoalBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<Goal> validator) : base(unitOfWork)
        {
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<Goal>> LoadGoalAync(int goalId)
        {
            var entity = new BusinessResultValue<Goal>();
            try
            {
                entity.ResultValue = await unitOfWork.GoalDataService.GetGoalAync(goalId);
                if (entity.ResultValue.GoalGoodsCategoryTypeId >= GoalGoodsCategoryTypeEnum.TotalSalesGoal
                    && entity.ResultValue.GoalGoodsCategoryTypeId <= GoalGoodsCategoryTypeEnum.ReceiptGovernGoal)
                {
                    var genralGoalStep = entity.ResultValue.GoalSteps.FirstOrDefault();
                    entity.ResultValue.Amount = genralGoalStep.ComputingValue;
                }

                entity.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entity, "");
            }
            return entity;
        }
        public async Task<IBusinessResult> DeleteGoalAsync(Goal entity)
        {
            var result = new BusinessResult();
            try
            {
                if (entity.IsUsed == true)
                {
                    result.ReturnMessage.Add("هدف برای محاسبه پورسانت استفاده شده است و امکان حذف هدف وجود ندارد");
                    result.ReturnStatus = false;
                    return result;
                }

                Goal dbGaol = await unitOfWork.GoalDataService.GetGoalInclude_GoalGoodsCategory_GoalSteps(entity.Id);
                var lstGoals = await unitOfWork.GoalDataService.GetAsync(x => x.GoalGoodsCategoryId == entity.GoalGoodsCategoryId);
                if (lstGoals.Count == 1)
                {
                    dbGaol.GoalGoodsCategory.Deleted = true;
                }

                dbGaol.GoalSteps.ToList().ForEach(x =>
                {
                    x.Deleted = true;
                    x.Items.ToList().ForEach(i =>
                    {
                        i.Deleted = true;
                    });
                });

                dbGaol.Deleted = true;
                unitOfWork.GoalDataService.Delete(dbGaol);
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_DELETE_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }


            return result;
        }
        public async Task<IBusinessResultValue<Goal>> CreateGoalAsync(Goal goalEntity)
        {
            var result = new BusinessResultValue<Goal>();
            try
            {
                if (goalEntity.GoalTypeId == GoalTypeEnum.Supplier)
                {
                    goalEntity.GoalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.Group;
                    goalEntity.GoalGoodsCategory = new GoalGoodsCategory();
                    goalEntity.GoalGoodsCategory.GoalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.Group;
                    goalEntity.GoalGoodsCategory.GoalTypeId = GoalTypeEnum.Supplier;
                    //TODO : it should be reviewed at supplier goal developing time   
                    //goalEntity.GoalGoodsCategory.GoodsCollection = goalEntity.GoodsSelectionList;
                    if (goalEntity.Company != null)
                    {
                        goalEntity.GoalGoodsCategory.Name = goalEntity.Company.FaName;
                    }
                }
                else if (goalEntity.GoalGoodsCategoryTypeId >= GoalGoodsCategoryTypeEnum.TotalSalesGoal &&
                    goalEntity.GoalGoodsCategoryTypeId <= GoalGoodsCategoryTypeEnum.AggregationGoal)
                {
                    if (goalEntity.StartDate == DateTime.MinValue)
                    {
                        goalEntity.StartDate = Utilities.ToDateTime(goalEntity.Year, goalEntity.Month).Value;
                        goalEntity.EndDate = Utilities.ToDateTime(goalEntity.Year, goalEntity.Month, ToDateTimeOptions.EndMonth).Value;
                    }

                    var goalGoodsCategory = await unitOfWork.GoalGoodsCategoryDataService.FirstOrDefaultAsync(where: x => x.GoalGoodsCategoryTypeId == goalEntity.GoalGoodsCategoryTypeId);

                    if (goalGoodsCategory == null)
                    {
                        goalEntity.GoalGoodsCategory = new GoalGoodsCategory();
                        goalEntity.GoalGoodsCategory.GoalGoodsCategoryTypeId = goalEntity.GoalGoodsCategoryTypeId;
                        goalEntity.GoalGoodsCategory.GoalTypeId = GoalTypeEnum.Distributor;
                        goalEntity.GoalGoodsCategory.Name = goalEntity.GoalGoodsCategoryTypeId.GetEnumDescription<GoalGoodsCategoryTypeEnum>();
                    }
                    else
                    {
                        goalEntity.GoalGoodsCategoryId = goalGoodsCategory.Id;
                    }

                    goalEntity.GoalSteps.Add(new GoalStep()
                    {
                        ComputingTypeId = ComputingTypeEnum.Amount,
                        ComputingValue = goalEntity.Amount.Value,
                        GoalTypeId = GoalTypeEnum.Distributor,
                    });
                }

                if (validator != null)
                {
                    ValidationResult results = await validator.ValidateAsync(goalEntity);
                    if (results.IsValid == false)
                    {
                        result.PopulateValidationErrors(results.Errors);
                        return result;
                    }
                }
                unitOfWork.GoalDataService.Insert(goalEntity);
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ResultValue = goalEntity;
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResult> UpdateGoalAsync(Goal goalEntity)
        {
            var result = new BusinessResult();
            try
            {
                if (goalEntity.IsUsed == true)
                {
                    result.ReturnMessage.Add("هدف برای محاسبه پورسانت استفاده شده است و امکان تغییر اطلاعات هدف وجود ندارد");
                    result.ReturnStatus = false;
                    return result;
                }

                if (goalEntity.GoalGoodsCategoryTypeId >= GoalGoodsCategoryTypeEnum.TotalSalesGoal &&
                    goalEntity.GoalGoodsCategoryTypeId <= GoalGoodsCategoryTypeEnum.ReceiptGovernGoal)
                {
                    var generalGoalStep = await unitOfWork.GoalStepDataService.FirstOrDefaultAsync(x => x.GoalId == goalEntity.Id);
                    generalGoalStep.ComputingValue = goalEntity.Amount.Value;
                    goalEntity.GoalSteps.Add(generalGoalStep);
                }
                ValidationResult results = await validator.ValidateAsync(goalEntity);
                if (results.IsValid == false)
                {
                    result.PopulateValidationErrors(results.Errors);
                    return result;
                }
                unitOfWork.GoalDataService.Update(goalEntity);
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add(MESSAGE_MODIFY_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<decimal>> LoadPreviousAggregationValueAync(int month, int year)
        {
            var result = new BusinessResultValue<decimal>();
            try
            {
                result.ResultValue = await unitOfWork.GoalDataService.GetPreviousAggregationValueAsync(month, year);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessLoadByPagingResult<GroupByStartEndDate>> LoadGroupByStartEndDateGoalsAync(bool? isUsed, int pageNumber = 0, int pageSize = 15)
        {
            var result = new BusinessLoadByPagingResult<GroupByStartEndDate>();
            try
            {
                var query = (from gl in unitOfWork.GoalDataService.GetQuery()
                             where (isUsed == null || gl.IsUsed == isUsed) &&
                             (gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Single || gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.Group)
                             && gl.Deleted == false
                             group gl by new { gl.StartDate, gl.EndDate } into grpgl
                             select new
                             {
                                 grpgl.Key.StartDate,
                                 grpgl.Key.EndDate
                             });
                result.TotalRows = await query.CountAsync();
                
                var takePaging = await query.OrderBy(x => x.StartDate).Skip(pageNumber).Take(pageSize).ToListAsync();
                result.ResultValue = takePaging.Select(x => new GroupByStartEndDate
                {
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    IsUsed = isUsed
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
    }
}
