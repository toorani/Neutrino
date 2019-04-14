using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface INeutrinoRoleBS : IBusinessService,IEnabledEntityListLoader<NeutrinoRole>
        ,IEnabledEntityLoader<NeutrinoRole>
        ,IEnabledEntityListByPagingLoader<NeutrinoRole>
    {
        Task<IBusinessResultValue<List<int>>> LoadRoleIdsByUserIdAsync(int userId);
        
    }
}
