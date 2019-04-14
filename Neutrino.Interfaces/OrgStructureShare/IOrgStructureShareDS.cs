using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IOrgStructureShareDS : IEntityRepository<OrgStructureShare>
    {
        Task<Tuple<List<OrgStructureShareDTO>, int>> GetOrgStructureShareDTOByPagingAsync(string brandName
            , Func<IQueryable<OrgStructureShareDTO>, IOrderedQueryable<OrgStructureShareDTO>> orderBy
            , int pageNumber = 0
            , int pageSize = 15);
        Task<OrgStructureShareDTO> GetOrgStructureShareDTOAsync(int branchId);
        void InsertOrUpdateOrgStructureShare(OrgStructureShare entity);
        
    }


}
