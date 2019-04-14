using System;
using System.Threading.Tasks;
using Espresso.DataAccess;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class DataSyncStatusDataService : RepositoryBase<DataSyncStatus>, IDataSyncStatus
    {
        #region [ Varibale(s) ]
        NeutrinoContext dbContext;
        #endregion

        #region [ Constructor(s) ]
        public DataSyncStatusDataService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            dbContext = (NeutrinoContext)unitOfWork.Context;
        }
        #endregion

       
    }
}
