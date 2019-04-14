using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;
using Ninject.Extensions.Logging;

namespace Neutrino.Business
{
    public class GeneralEntityCounter<TEntity> : NeutrinoBusinessService,IEntityCounter<TEntity>
        where TEntity : EntityBase, new()
    {
        #region [ Constructor(s) ]
        public GeneralEntityCounter(NeutrinoUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            
        }
        #endregion

        #region [ Public Method(s) ]
        public virtual IBusinessResultValue<int> GetCount(Expression<Func<TEntity, bool>> where = null)
        {
            IBusinessResultValue<int> resultValue = new BusinessResultValue<int>();
            try
            {
                resultValue.ResultValue = unitOfWork.GetRepository<TEntity>().GetCount(where);
                resultValue.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, resultValue, "");
            }
            return resultValue;
        }
        public virtual async Task<IBusinessResultValue<int>> GetCountAsync(Expression<Func<TEntity, bool>> where = null)
        {
            IBusinessResultValue<int> resultValue = new BusinessResultValue<int>();
            try
            {
                resultValue.ResultValue = await unitOfWork.GetRepository<TEntity>().GetCountAsync(where);
                resultValue.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, resultValue, "");
            }
            return resultValue;
        }
        #endregion
        
    }
}
