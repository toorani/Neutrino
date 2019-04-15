using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.Core;
using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using NLog;

namespace Neutrino.Business
{
    public abstract class NeutrinoBSBase<TEntity, IDataService> : BusinessServiceGenericBase<TEntity>
        where TEntity : EntityBase, new()
        where IDataService : IEntityRepository<TEntity>
    {
        #region [ Protected Property(ies) ]
        protected IDataService dataService
        {
            get
            {
                return (IDataService)dataRepository;
            }
        }
        protected NeutrinoContext dbContext;
        
        #endregion

        #region [ Constructor(s) ]
        public NeutrinoBSBase(IEntityRepository<TEntity> dataService, ITransactionalData transactionalData)
            : base(dataService, transactionalData)
        {
        }
        public NeutrinoBSBase(ITransactionalData transactionalData)
            : base(transactionalData)
        {

            //base.dataRepository = DependencyResolver.Context.Instance.GetService<IDataService>();

            TransactionalData = transactionalData;
            if (dataRepository != null)
            {
                dbContext = dataRepository.UnitOfWork.Context as NeutrinoContext;
            }
        }
        public NeutrinoBSBase()
        {
            //base.dataRepository = DependencyResolver.Context.Instance.GetService<IDataService>();
            //TransactionalData = DependencyResolver.Context.Instance.GetService<ITransactionalData>(); ;
            if (dataRepository != null)
            {
                dbContext = dataRepository.UnitOfWork.Context as NeutrinoContext;
            }
        }
        #endregion

        #region [ Protected Method(s) ]
        
        #endregion

    }
}
