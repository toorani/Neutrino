using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;

namespace Neutrino.Data.EntityFramework
{
    public class NeutrinoRepositoryBase<TEntity> : RepositoryBase<TEntity>
        where TEntity : EntityBase, new()
    {
        protected readonly NeutrinoContext dbContext;

        public NeutrinoRepositoryBase(NeutrinoContext context) : base(context)
        {
            dbContext = context;
        }
    }
}
