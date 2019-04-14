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
    public class OtherRewardTypeDataService : RepositoryBase<OtherRewardType>, IOtherRewardType
    {
        #region [ Constructor(s) ]
        public OtherRewardTypeDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion

    }
}
