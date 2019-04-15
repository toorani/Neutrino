using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core.Interfaces;
using Espresso.DataAccess;
using Espresso.Identity.Models;


namespace Neutrino.Identity.EntityFramework.DataServices
{
    public class UserDataService : RepositoryBase<User>
    {


        #region [ Constructor(s) ]
        public UserDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion

    }
}
