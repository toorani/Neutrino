
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Data.EntityFramework.DataServices;
using System.Linq.Expressions;

namespace Neutrino.Business
{
    public class DataSyncStatusBusinessService : NeutrinoBSBase<DataSyncStatus, IDataSyncStatus>
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Constructor(s) ]
        public DataSyncStatusBusinessService(ITransactionalData transactionalData)
            : base(transactionalData)
        {
           
        }
        public DataSyncStatusBusinessService()
            : base()
        {
        }
        #endregion

        #region [ Public Method(s) ]
        
        #endregion



    }
}
