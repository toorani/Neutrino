using System;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IOrgStructureShareBS : IBusinessService
    {
        Task<IBusinessLoadByPagingResult<OrgStructureShareDTO>> LoadOrgStructureShareDTOByPagingAsync(string brandName
            , Func<IQueryable<OrgStructureShareDTO>, IOrderedQueryable<OrgStructureShareDTO>> orderBy
            , int pageNumber = 0
            , int pageSize = 15);
        Task<IBusinessResultValue<OrgStructureShareDTO>> LoadOrgStructureShareDTOAsync(int branchId);
        Task<IBusinessResultValue<OrgStructureShareDTO>> CreateOrgStructureShareDTOAsync(OrgStructureShareDTO orgStructureShareDTO);
        Task<IBusinessResult> DeleteOrgStructureShareDTOAsync(OrgStructureShareDTO orgStructureShareDTO);
    }
}
