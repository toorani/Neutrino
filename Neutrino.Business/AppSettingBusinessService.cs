
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
using Notrino.Core;

namespace Neutrino.Business
{
    public class AppSettingBusinessService : NeutrinoBSBase<AppSetting,IAppSetting>
    {
        #region [ Constructor(s) ]
        public AppSettingBusinessService(ITransactionalData transactionalData)
            : base(transactionalData)
        {
        }
        #endregion
    }
}
