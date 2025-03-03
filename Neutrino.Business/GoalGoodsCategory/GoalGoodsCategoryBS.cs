﻿
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using FluentValidation;
using FluentValidation.Results;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Neutrino.Business
{
    public class GoalGoodsCategoryBS : NeutrinoBusinessService, IGoalGoodsCategoryBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<GoalGoodsCategory> validator;
        [Inject]
        public IEntityListLoader<GoalGoodsCategory> EntityListLoader { get; set; }
        [Inject]
        public IEntityLoader<GoalGoodsCategory> EntityLoader { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public GoalGoodsCategoryBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<GoalGoodsCategory> validator)
            :base(unitOfWork)
        {
            this.validator = validator;
            
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<GoalGoodsCategory>> CreateGoalGoodsCategoryAsync(GoalGoodsCategory goalGoodsCategory, int goalCategorySimilarId)
        {
            var result = new BusinessResultValue<GoalGoodsCategory>();
            try
            {
                if (validator != null)
                {
                    ValidationResult results = await validator.ValidateAsync(goalGoodsCategory);
                    if (results.IsValid == false)
                    {
                        result.PopulateValidationErrors(results.Errors);
                        return await Task<GoalGoodsCategory>.Run(() => result);
                    }
                }
                List<Goal> goals = new List<Goal>();
                if (goalCategorySimilarId != 0)
                {
                    var similarEntity = await unitOfWork.GoalGoodsCategoryDataService.GetByIdAsync(goalCategorySimilarId);
                    similarEntity.IsVisible = false;
                    
                    goals = await unitOfWork.GoalDataService
                        .GetQuery()
                        .AsNoTracking()
                        .Where(x => x.IsUsed == false && x.GoalGoodsCategoryId == goalCategorySimilarId)
                        .ToListAsync();
                    unitOfWork.GoalGoodsCategoryDataService.Update(similarEntity);
                }
                unitOfWork.GoalGoodsCategoryDataService.Insert(goalGoodsCategory);
                
                foreach (var item in goals)
                {
                    item.GoalGoodsCategoryId = goalGoodsCategory.Id;
                    unitOfWork.GoalDataService.Update(item);
                }
                await unitOfWork.CommitAsync();

                result.ReturnStatus = true;
                result.ResultValue = await unitOfWork.GoalGoodsCategoryDataService.GetGoalGoodsCategoryAsync(goalGoodsCategory.Id);
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task<IBusinessResultValue<GoalGoodsCategory>> LoadGoalGoodsCategoryAsync(int goalGoodsCategoryId)
        {
            var result = new BusinessResultValue<GoalGoodsCategory>();
            try
            {
                result.ResultValue = await unitOfWork.GoalGoodsCategoryDataService.GetGoalGoodsCategoryAsync(goalGoodsCategoryId);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }

            return result;
        }
        public async Task<IBusinessResultValue<List<GoalGoodsCategory>>> LoadVisibleGoalGoodsCategoryListAsync(Expression<Func<GoalGoodsCategory, bool>> where = null)
        {
            var result = new BusinessResultValue<List<GoalGoodsCategory>>();
            try
            {
                if (where != null)
                {
                    where = where.And(x => x.IsVisible == true);
                }
                else
                {
                    where = x => (x.IsVisible == true);
                }
                result.ResultValue = await unitOfWork.GoalGoodsCategoryDataService.GetAsync(where);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<List<GoalGoodsCategory>>> LoadGoalGoodsCategoryForReportAsync(GoalGoodsCategoryTypeEnum goodsCategoryTypeId, GoalTypeEnum goalTypeId, DateTime startDate, DateTime endDate)
        {
            var result = new BusinessResultValue<List<GoalGoodsCategory>>();
            try
            {
                result.ResultValue = await (from goal in unitOfWork.GoalDataService.GetQuery()
                                            join ggc in unitOfWork.GoalGoodsCategoryDataService.GetQuery()
                                            on goal.GoalGoodsCategoryId equals ggc.Id
                                            where goal.StartDate >= startDate && goal.EndDate <= endDate
                                            && goal.GoalGoodsCategoryTypeId == goodsCategoryTypeId
                                            && goal.GoalTypeId == goalTypeId
                                            select ggc).ToListAsync();
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
