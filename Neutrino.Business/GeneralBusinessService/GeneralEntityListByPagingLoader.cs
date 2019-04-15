using System;
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
    public class GeneralEntityListByPagingLoader<TEntity> : NeutrinoBusinessService,IEntityListByPagingLoader<TEntity>
        where TEntity : EntityBase, new()
    {
        #region [ Varibale(s) ]
        private readonly IEntityRepository<TEntity> dataRepository;
        #endregion


        #region [ Constructor(s) ]
        public GeneralEntityListByPagingLoader(NeutrinoUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            dataRepository = unitOfWork.GetRepository<TEntity>();
        }
        #endregion

        #region [ Public Method(s) ]
        public virtual async Task<IBusinessLoadByPagingResult<TEntity>> LoadAsync(Expression<Func<TEntity, bool>> where = null
           , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
           , int pageNumber = 0
           , int pageSize = 15
           , params Expression<Func<TEntity, object>>[] includes)
        {
            IBusinessLoadByPagingResult<TEntity> entites = new BusinessLoadByPagingResult<TEntity>();
            
            try
            {
                entites.ResultValue= await dataRepository.GetByPagingAsync(where, orderBy, pageNumber, pageSize, includes);
                entites.TotalRows = await dataRepository.GetCountAsync(where: where);
                entites.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entites, "");
            }

            return entites;
        }

        public virtual IBusinessLoadByPagingResult<TEntity> Load(Expression<Func<TEntity, bool>> where = null
           , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
           , int pageNumber = 0
           , int pageSize = 15
           , params Expression<Func<TEntity, object>>[] includes)
        {
            
            IBusinessLoadByPagingResult<TEntity> entites = new BusinessLoadByPagingResult<TEntity>();
            try
            {
                int totalRows;
                entites.ResultValue = dataRepository.GetByPaging(out totalRows, where, orderBy, pageNumber, pageSize, includes).AsEnumerable<TEntity>().ToList();
                entites.TotalRows = totalRows;
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
