using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Espresso.Identity.Models;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class ServiceLogDataService : RepositoryBase<ServiceLog>, IServiceLog
    {
        #region [ Constructor(s) ]
        public ServiceLogDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }


        #endregion

        

    }
}
