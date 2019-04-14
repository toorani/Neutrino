using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;
using FluentValidation;
using FluentValidation.Results;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class SimpleEntityCreator<TEntity> : NeutrinoBusinessService,IEntityCreator<TEntity>
        where TEntity : EntityBase, new()
    {
        #region [ Varibale(s) ]
        protected readonly IEntityRepository<TEntity> dataRepository;
        protected readonly AbstractValidator<TEntity> validator;
        #endregion

        #region [ Constructor(s) ]
        public SimpleEntityCreator(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<TEntity> validator = null):base(unitOfWork)
        {
            dataRepository = unitOfWork.GetRepository<TEntity>();
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public virtual IBusinessResultValue<TEntity> Create(TEntity entity)
        {
            var result = new BusinessResultValue<TEntity>();
            try
            {
                if (validator != null)
                {
                    ValidationResult results = validator.Validate(entity);
                    if (results.IsValid == false)
                    {
                        result.PopulateValidationErrors(results.Errors);
                        return result;
                    }
                }
                dataRepository.Insert(entity);
                unitOfWork.Commit();
                result.ReturnStatus = true;
                result.ResultValue = entity;
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }

            return result;
        }
        public virtual async Task<IBusinessResultValue<TEntity>> CreateAsync(TEntity entity)
        {
            var result = new BusinessResultValue<TEntity>();
            try
            {
                if (validator != null)
                {
                    ValidationResult results = await validator.ValidateAsync(entity);
                    if (results.IsValid == false)
                    {
                        result.PopulateValidationErrors(results.Errors);
                        return await Task<TEntity>.Run(() => result);
                    }
                }
                dataRepository.Insert(entity);
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ResultValue = entity;
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }

            return result;

        }
        public virtual IBusinessResultValue<int> CreateBatchEntities(List<TEntity> lstEntities)
        {
            var result = new BusinessResultValue<int>();
            result.ResultValue = -1;
            try
            {
                result.ResultValue = dataRepository.InsertBulk(lstEntities);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public virtual async Task<IBusinessResultValue<int>> CreateBatchEntitiesAsync(List<TEntity> lstEntities)
        {
            var result = new BusinessResultValue<int>();
            result.ResultValue = -1;
            try
            {
                result.ResultValue = await dataRepository.InsertBulkAsync(lstEntities);
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
