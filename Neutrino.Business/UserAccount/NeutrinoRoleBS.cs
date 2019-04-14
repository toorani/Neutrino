using Neutrino.Interfaces;
using Neutrino.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Espresso.BusinessService.Interfaces;
using Espresso.BusinessService;
using System;
using Ninject;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class NeutrinoRoleBS : NeutrinoBusinessService, INeutrinoRoleBS
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Public Property(ies) ]
        [Inject]
        public IEntityListByPagingLoader<NeutrinoRole> EntityListByPagingLoader { get; set; }
        [Inject]
        public IEntityListLoader<NeutrinoRole> EntityListLoader { get; set; }
        [Inject]
        public IEntityLoader<NeutrinoRole> EntityLoader { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public NeutrinoRoleBS(NeutrinoUnitOfWork unitOfWork):base(unitOfWork)
        {
            
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<List<int>>> LoadRoleIdsByUserIdAsync(int userId)
        {
            var result = new BusinessResultValue<List<int>>();
            try
            {
                result.ResultValue = await unitOfWork.RoleDataService.GetRoleIdsByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        #endregion



    }
}
