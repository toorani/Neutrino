using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IOrgStructureBS : IBusinessService
        ,IEnabledEntityListByPagingLoader<OrgStructure>
        ,IEnabledEntityListLoader<OrgStructure>
    {
        Task<IBusinessResult> CreateOrEditAsync(List<OrgStructure> lstOrgStructure);
        Task<IBusinessResult> DeleteAsync(int positionTypeId);
    }
}
