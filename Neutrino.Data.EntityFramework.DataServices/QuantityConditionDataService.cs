using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class QuantityConditionDataService : NeutrinoRepositoryBase<QuantityCondition>, IQuantityConditionDS
    {
        #region [ Varibale(s) ]
        private readonly IEntityBaseRepository<GoodsQuantityCondition> goodsQuantityConditionDS;
        private readonly IEntityBaseRepository<BranchQuantityCondition> branchQuantityConditionDS;
        #endregion

        #region [ Constructor(s) ]
        public QuantityConditionDataService(NeutrinoContext context
            , IEntityBaseRepository<GoodsQuantityCondition> goodsQuantityConditionDS
            , IEntityBaseRepository<BranchQuantityCondition> branchQuantityConditionDS) : base(context)
        {
            this.branchQuantityConditionDS = branchQuantityConditionDS;
            this.goodsQuantityConditionDS = goodsQuantityConditionDS;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<QuantityCondition> GetQuantityConditionAsync(int goalId)
        {
            var query_result = await (from goal in dbContext.Goals.Include(x => x.GoalGoodsCategory)
                                      where !goal.Deleted && goal.Id == goalId
                                      join qc in dbContext.QuantityConditions
                                      on goal.Id equals qc.GoalId into join_goal_quantityCondition
                                      from goal_qc in join_goal_quantityCondition.DefaultIfEmpty()
                                      select new
                                      {
                                          GoalId = goal.Id,
                                          ExtraEncouragePercent = goal_qc != null ? goal_qc.ExtraEncouragePercent : 0,
                                          ForthCasePercent = goal_qc != null ? goal_qc.ForthCasePercent : 0,
                                          NotReachedPercent = goal_qc != null ? goal_qc.NotReachedPercent : 0,
                                          Quantity = goal_qc != null ? goal_qc.Quantity : 0,
                                          QuantityConditionTypeId = goal_qc != null ? goal_qc.QuantityConditionTypeId : null,
                                          Id = goal_qc != null ? goal_qc.Id : 0,
                                          Goal = goal,
                                          GoalGoodsCategory = goal.GoalGoodsCategory,
                                          GoodsQuantityConditions = (from gds in goal.GoalGoodsCategory.GoodsCollection
                                                                     join gdsQuan in dbContext.GoodsQuantityConditions
                                                                     on gds.GoodsId equals gdsQuan.GoodsId into join_gds_Quna
                                                                     from gds_Quan in join_gds_Quna.DefaultIfEmpty()
                                                                     select new
                                                                     {
                                                                         Goods = gds.Goods,
                                                                         GoodsId = gds.GoodsId,
                                                                         Id = gds_Quan != null ? gds_Quan.Id : 0,
                                                                         Quantity = gds_Quan != null ? gds_Quan.Quantity : 0,
                                                                         BranchQuantityConditions = (from br in dbContext.Branches
                                                                                                     join brQua in gds_Quan.BranchQuantityConditions
                                                                                                     on br.Id equals brQua.BranchId into join_brach_brQua
                                                                                                     where br.Deleted == false
                                                                                                     from bra_qua in join_brach_brQua.DefaultIfEmpty()
                                                                                                     select new
                                                                                                     {
                                                                                                         BranchId = br.Id,
                                                                                                         Branch = br,
                                                                                                         Id = bra_qua != null ? bra_qua.Id : 0,
                                                                                                         Quantity = bra_qua != null ? bra_qua.Quantity : 0,
                                                                                                     })

                                                                     }),
                                      })
                                      .FirstOrDefaultAsync();




            QuantityCondition result = new QuantityCondition
            {
                Id = query_result.Id,
                ExtraEncouragePercent = query_result.ExtraEncouragePercent,
                Goal = query_result.Goal,
                GoalId = query_result.GoalId,
                Quantity = query_result.Quantity,
                ForthCasePercent = query_result.ForthCasePercent,
                NotReachedPercent = query_result.NotReachedPercent,
                QuantityConditionTypeId = query_result.QuantityConditionTypeId,
                GoodsQuantityConditions = query_result.GoodsQuantityConditions
                .ToList()
                .OrderBy(x => x.Goods.GoodsCode)
                .Select(y => new GoodsQuantityCondition()
                {
                    Id = y.Id,
                    QuantityConditionId = query_result.Id,
                    Goods = y.Goods,
                    GoodsId = y.GoodsId,
                    Quantity = y.Quantity,
                    BranchQuantityConditions = y.BranchQuantityConditions.ToList()
                    .OrderBy(x => x.Branch.Name)
                    .Select(z => new BranchQuantityCondition()
                    {
                        Id = z.Id,
                        Branch = z.Branch,
                        BranchId = z.BranchId,
                        GoodsQuantityConditionId = y.Id,
                        Quantity = z.Quantity
                    }).ToList()
                }).ToList(),
            };
            return result;


        }
        public override void InsertOrUpdate(QuantityCondition entity)
        {
            entity.Goal = null;
            foreach (var goodsQC in entity.GoodsQuantityConditions.Where(x => x.Quantity != 0))
            {
                goodsQC.Goods = null;
                goodsQC.QuantityConditionId = entity.Id;
                foreach (var branchQC in goodsQC.BranchQuantityConditions)
                {
                    branchQC.Branch = null;
                    branchQC.GoodsQuantityCondition = null;
                    branchQC.GoodsQuantityConditionId = goodsQC.Id;

                    if (branchQC.Id == 0)
                    {
                        dbContext.Entry(branchQC).State = EntityState.Added;
                    }
                    else
                    {
                        dbContext.Entry(branchQC).State = EntityState.Modified;
                    }
                }
                if (goodsQC.Id == 0)
                {
                    dbContext.Entry(goodsQC).State = EntityState.Added;
                }
                else
                {
                    dbContext.Entry(goodsQC).State = EntityState.Modified;
                }
            }
            if (entity.Id == 0)
            {
                Insert(entity);
            }
            else
            {
                Update(entity);
            }
            
        }
        public async Task<List<QuantityCondition>> GetQuantityConditionListAsync(List<int> goalIds)
        {
            return await dbContext.QuantityConditions
                .Include(d => d.GoodsQuantityConditions)
                .Include(d => d.GoodsQuantityConditions.Select(x => x.BranchQuantityConditions))
                .Where(x => goalIds.Contains(x.GoalId) && x.Deleted == false
                && x.GoodsQuantityConditions.Any(y=>y.Deleted == false))
                .ToListAsync();


        }
        #endregion


    }
}
