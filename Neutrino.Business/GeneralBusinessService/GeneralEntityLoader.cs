using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class GeneralEntityLoader<TEntity> : NeutrinoBusinessService,IEntityLoader<TEntity>
        where TEntity : EntityBase, new()
    {
        #region [ Varibale(s) ]
        private readonly IEntityRepository<TEntity> dataRepository;
        #endregion
        
        #region [ Constructor(s) ]
        public GeneralEntityLoader(NeutrinoUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            dataRepository = unitOfWork.GetRepository<TEntity>();
        }
        #endregion

        #region [ Public Method(s) ]
        public virtual IBusinessResultValue<TEntity> Load(int entityId, params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = new BusinessResultValue<TEntity>();
            try
            {
                entity.ResultValue = dataRepository.FirstOrDefault(where: ent => ent.Id == entityId);
                entity.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entity, "");
            }
            return entity;


        }
        public virtual async Task<IBusinessResultValue<TEntity>> LoadAsync(int entityId, params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = new BusinessResultValue<TEntity>();
            try
            {
                entity.ResultValue = await dataRepository.GetByIdAsync(entityId, includes: includes);
                entity.ReturnStatus = true;

            }
            catch (Exception ex)
            {
                CatchException(ex, entity, "");
            }
            return entity;


        }

        public virtual IBusinessResultValue<TEntity> Load(Expression<Func<TEntity, bool>> where
            , params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = new BusinessResultValue<TEntity>();
            try
            {
                entity.ResultValue = dataRepository.FirstOrDefault(where, includes: includes);
                entity.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entity, "");
            }
            return entity;
        }
        public virtual async Task<IBusinessResultValue<TEntity>> LoadAsync(Expression<Func<TEntity, bool>> where = null
            , bool isThrowException = false
            , params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = new BusinessResultValue<TEntity>();
            try
            {
                entity.ResultValue = await dataRepository.FirstOrDefaultAsync(where, includes: includes);
                entity.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                if (!isThrowException)
                {
                    CatchException(ex, entity, "");
                }
                else
                    throw ex;
            }
            return entity;
        }

        public virtual IBusinessResultValue<TEntity> LoadLatestRecord()
        {
            var entity = new BusinessResultValue<TEntity>();
            try
            {
                entity.ResultValue = dataRepository.GetLatestRecord();
                entity.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entity, "");
            }
            return entity;
        }
        public virtual async Task<IBusinessResultValue<TEntity>> LoadLatestRecordAsync()
        {
            var entity = new BusinessResultValue<TEntity>();
            try
            {
                entity.ResultValue = await dataRepository.GetLatestRecordAsync();
                entity.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entity, "");
            }
            return entity;
        }
        #endregion

       

    }
}
