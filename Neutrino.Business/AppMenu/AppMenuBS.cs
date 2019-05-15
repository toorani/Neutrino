using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject.Extensions.Logging;

namespace Neutrino.Business
{
    public class AppMenuBS : NeutrinoBusinessService, IAppMenuBS
    {
        #region [ Constructor(s) ]
        public AppMenuBS(NeutrinoUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        #endregion

        public async Task<IBusinessResultValue<List<AppMenu>>> LoadApplicationMenu(List<string> lstUrlPermission, bool checkAccess)
        {
            var result = new BusinessResultValue<List<AppMenu>>();
            try
            {
                if (checkAccess)
                {
                    result.ResultValue = await unitOfWork.AppMenuDataService
                    .GetAsync(where: x => x.ChildItems.Any(y => lstUrlPermission.Contains(y.Url))
                    , includes: x => x.ChildItems
                    , orderBy: x => x.OrderBy(y => y.OrderId));
                }
                else
                {
                    result.ResultValue = await unitOfWork.AppMenuDataService
                    .GetAsync(where: x => x.ParentId == null
                        , includes: x => x.ChildItems
                        , orderBy: x => x.OrderBy(y => y.OrderId));
                }

                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

    }
}
