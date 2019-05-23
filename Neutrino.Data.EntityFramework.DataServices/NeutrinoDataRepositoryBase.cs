using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;

namespace Neutrino.Data.EntityFramework
{
    public class NeutrinoDataRepositoryBase<TEntity> : DataRepository<TEntity>
        where TEntity : class
    {
        protected readonly NeutrinoContext dbContext;

        public NeutrinoDataRepositoryBase(NeutrinoContext context) : base(context)
        {
            dbContext = context;
        }
    }
}
