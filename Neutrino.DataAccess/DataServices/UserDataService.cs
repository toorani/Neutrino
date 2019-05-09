using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class UserDataService : DataRepository<User>
    {
        #region [ Constructor(s) ]
        public UserDataService(NeutrinoContext context)
            : base(context)
        {
        }

        #endregion
    }
}
