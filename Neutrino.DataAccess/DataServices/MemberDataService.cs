using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class MemberDataService : RepositoryBase<Member>, IMember
    {
        #region [ Constructor(s) ]
        public MemberDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion
    }
}
