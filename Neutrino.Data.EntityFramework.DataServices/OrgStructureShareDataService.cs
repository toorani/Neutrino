using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Data.Entity;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class OrgStructureShareDataService : NeutrinoRepositoryBase<OrgStructureShare>, IOrgStructureShareDS
    {
        #region [ Varibale(s) ]
        private readonly IEntityBaseRepository<OrgStructure> orgStructureDataService;
        #endregion


        #region [ Constructor(s) ]
        public OrgStructureShareDataService(NeutrinoContext context
            , IEntityBaseRepository<OrgStructure> orgStructureDataService)
            : base(context)
        {
            this.orgStructureDataService = orgStructureDataService;
        }

        #endregion

        #region [ Public Method(s) ]

        public async Task<Tuple<List<OrgStructureShareDTO>, int>> GetOrgStructureShareDTOByPagingAsync(
             string brandName
            , Func<IQueryable<OrgStructureShareDTO>, IOrderedQueryable<OrgStructureShareDTO>> orderBy
            , int pageNumber = 0
            , int pageSize = 15)
        {


            var query = dbContext.OrgStructureShare
                   .Where(x => x.Deleted == false
                    && (x.Branch.Name.Contains(brandName) || brandName == null))
                    .GroupBy(x => x.Branch)
                    .Select(x => new OrgStructureShareDTO
                    {
                        Branch = x.Key,
                        Items = x.ToList()
                    });
            int totalRow = await query.CountAsync();
            query = orderBy(query);
            query = query.Skip(pageNumber).Take(pageSize);
            var items = await query.ToListAsync();



            List<int> orgStructIds = new List<int>();
            items.ForEach(x =>
            {
                x.Items.ForEach(y =>
                {
                    orgStructIds.Add(y.OrgStructureId);
                });
            });

            var orgStructs = await orgStructureDataService.GetAsync(where: x => orgStructIds.Distinct().Contains(x.Id));

            items.ForEach(x =>
            {
                x.Items.ForEach(y =>
                {
                    y.OrgStructure = orgStructs.FirstOrDefault(a => a.Id == y.OrgStructureId);
                });
            });

            return new Tuple<List<OrgStructureShareDTO>, int>(items, totalRow);
        }
        public async Task<OrgStructureShareDTO> GetOrgStructureShareDTOAsync(int branchId)
        {
            OrgStructureShareDTO result = new OrgStructureShareDTO();
            result.Branch = await (from br in dbContext.Branches where br.Id == branchId select br).FirstOrDefaultAsync();

            var query = await (from org in dbContext.OrgStructures
                               join perShare in dbContext.OrgStructureShare on
                               org.Id equals perShare.OrgStructureId
                               into persOrg
                               where org.BranchId == branchId && org.Deleted == false
                               from final in persOrg.Where(x => x.Deleted == false).DefaultIfEmpty()
                               join post in dbContext.PositionTypes
                               on org.PositionTypeId  equals post.eId
                               select new
                               {
                                   Org = org,
                                   PositionType = post,
                                   OrgStructureShare = final,
                               })
                               .ToListAsync();
            result.Items = query.Select(x => new OrgStructureShare
            {
                OrgStructure = x.Org,
                Branch = result.Branch,
                BranchId = branchId,
                OrgStructureId = x.Org.Id,
                PrivateReceiptPercent = x.OrgStructureShare != null ? x.OrgStructureShare.PrivateReceiptPercent : null,
                TotalReceiptPercent = x.OrgStructureShare != null ? x.OrgStructureShare.TotalReceiptPercent : null,
                SalesPercent = x.OrgStructureShare != null ? x.OrgStructureShare.SalesPercent : null,
                MinimumPromotion = x.OrgStructureShare != null ? x.OrgStructureShare.MinimumPromotion : null,
                MaximumPromotion = x.OrgStructureShare != null ? x.OrgStructureShare.MaximumPromotion : null,
                Id = x.OrgStructureShare != null ? x.OrgStructureShare.Id : 0,
            }).ToList();

            return result;
        }
        public void InsertOrUpdateOrgStructureShare(OrgStructureShare entity)
        {
            entity.Branch = null;
            entity.OrgStructure = null;
            if (entity.Id == 0)
                Insert(entity);
            else
                Update(entity);
        }
        #endregion

    }


}
