using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class GeneralEntityListLoader<TEntity> : NeutrinoBusinessService, IEntityListLoader<TEntity>
        where TEntity : EntityBase, new()
    {
        #region [ Varibale(s) ]
        private readonly IEntityRepository<TEntity> dataRepository;
        #endregion

        #region [ Constructor(s) ]
        public GeneralEntityListLoader(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            dataRepository = unitOfWork.GetRepository<TEntity>();
        }
        #endregion

        #region [ Public Method(s) ]
        public virtual IBusinessResultValue<List<TEntity>> LoadAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            , params Expression<Func<TEntity, object>>[] include)
        {

            IBusinessResultValue<List<TEntity>> entites = new BusinessResultValue<List<TEntity>>();

            try
            {
                entites.ResultValue = dataRepository.Get(where: null, includes: include, orderBy: orderBy).ToList();
                entites.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entites, "");
            }
            return entites;

        }
        public virtual async Task<IBusinessResultValue<List<TEntity>>> LoadAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            , params Expression<Func<TEntity, object>>[] include
            )
        {

            IBusinessResultValue<List<TEntity>> entites = new BusinessResultValue<List<TEntity>>();
            try
            {
                entites.ResultValue = await dataRepository.GetAsync(where: null, includes: include, orderBy: orderBy);
                entites.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entites, "");
            }
            return entites;

        }
        public virtual IBusinessResultValue<List<TEntity>> LoadList(Expression<Func<TEntity, bool>> where = null
           , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
           , params Expression<Func<TEntity, object>>[] includes)
        {
            var entites = new BusinessResultValue<List<TEntity>>();
            try
            {
                entites.ResultValue = dataRepository.Get(where, orderBy, includes: includes).AsEnumerable<TEntity>().ToList();
                entites.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entites, "");
            }

            return entites;
        }
        public virtual async Task<IBusinessResultValue<List<TEntity>>> LoadListAsync(Expression<Func<TEntity, bool>> where = null
           , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
           , params Expression<Func<TEntity, object>>[] includes)
        {
            var entites = new BusinessResultValue<List<TEntity>>();
            try
            {
                entites.ResultValue = await dataRepository.GetAsync(where, orderBy, includes: includes);
                entites.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entites, "");
            }

            return entites;
        }
        #endregion

    }
}
