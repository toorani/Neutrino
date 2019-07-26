using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IMemberBS : IBusinessService
    {
        Task<IBusinessResult> ToggleActivationAsync(int memberId);
        Task<IBusinessResultValue<List<Member>>> LoadMembersAsync(int brandId, bool? isActive = null);
        
    }
}
