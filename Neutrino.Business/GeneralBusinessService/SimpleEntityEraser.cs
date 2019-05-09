using System;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class SimpleEntityEraser<TEntity> : NeutrinoBusinessService, IEntityEraser<TEntity>
        where TEntity : EntityBase, new()
    {
        #region [ Varibale(s) ]
        private readonly IEntityBaseRepository<TEntity> dataRepository;
        #endregion

        #region [ Constructor(s) ]
        public SimpleEntityEraser(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            dataRepository = unitOfWork.GetRepository<TEntity>();

        }
        #endregion

        #region [ Public Method(s) ]
        public virtual IBusinessResult Delete(TEntity entity)
        {
            var result = new BusinessResult();
            try
            {
                dataRepository.Delete(entity);
                unitOfWork.Commit();
                result.ReturnStatus = true;
                result.ReturnMessage.Add(".حذف اطلاعات با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public virtual async Task<IBusinessResult> DeleteAsync(TEntity entity)
        {
            var result = new BusinessResult();
            try
            {
                dataRepository.Delete(entity);
                await unitOfWork.CommitAsync();
                result.ReturnStatus = true;
                result.ReturnMessage.Add(".حذف اطلاعات با موفقیت انجام شد");

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
