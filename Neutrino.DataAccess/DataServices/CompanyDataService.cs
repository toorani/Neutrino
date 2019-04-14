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
    public class CompanyDataService : RepositoryBase<Company>, ICompany
    {
        #region [ Constructor(s) ]
        public CompanyDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion

    }
}
