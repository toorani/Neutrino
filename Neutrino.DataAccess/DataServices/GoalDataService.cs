using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Z.EntityFramework.Plus;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class GoalDataService : NeutrinoRepositoryBase<Goal>, IGoalDS
    {
        #region [ Constructor(s) ]
        public GoalDataService(NeutrinoContext context)
            : base(context)
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public override void Update(Goal entityToUpdate)
        {
            if (entityToUpdate.GoalTypeId == GoalTypeEnum.Supplier)
            {

                var existingEntity = GetById(entityToUpdate.Id, includes: x => new { x.GoalGoodsCategory.GoodsCollection });

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Goal, Goal>()
                    .Ignore(x => x.Company)
                    .Ignore(x => x.GoalGoodsCategory);
                });
                var mapper = config.CreateMapper();
                mapper.Map<Goal, Goal>(entityToUpdate, existingEntity);

                var existGoods = existingEntity.GoalGoodsCategory.GoodsCollection.Select(x => new Goods { Id = x.GoodsId }).ToList();
                GoalGoodsCategoryGoods goalGoodsCategoryGoods = new GoalGoodsCategoryGoods();

                // added goods
                //TODO : it should be reviewed at supplier goal developing time   
                //entityToUpdate.GoodsSelectionList.Except(existGoods, x => x.Id)
                //   .ToList<Goods>()
                //   .ForEach(x =>
                //   {
                //       goalGoodsCategoryGoods.GoodsId = x.Id;
                //       goalGoodsCategoryGoods.GoalGoodsCategoryId = 
                //       NeutrinoContext.Goods.Add(x);
                //       NeutrinoContext.Goods.Attach(x);
                //       NeutrinoContext.Entry(x).State = EntityState.Modified;
                //       existingEntity.GoalGoodsCategory.GoodsCollection.Add(x);
                //   });

                //deleted goodsCategories
                //TODO : it should be reviewed at supplier goal developing time   
                //existingEntity.GoalGoodsCategory.GoodsCollection.Except(entityToUpdate.GoodsSelectionList,
                //    x => x.Id)
                //    .ToList<Goods>()
                //    .ForEach(x =>
                //    {
                //        existingEntity.GoalGoodsCategory.GoodsCollection.Remove(x);
                //    });
                dbContext.Entry(existingEntity.GoalGoodsCategory).State = EntityState.Modified;

            }
            else
            {
                if (entityToUpdate.GoalGoodsCategoryTypeId >= GoalGoodsCategoryTypeEnum.TotalSalesGoal &&
                    entityToUpdate.GoalGoodsCategoryTypeId <= GoalGoodsCategoryTypeEnum.ReceiptGovernGoal)
                {
                    var generalGoalStep = entityToUpdate.GoalSteps.FirstOrDefault();
                    dbContext.Entry(generalGoalStep).State = EntityState.Modified;
                }
            }
            //CheckPerviousGoal(entityToUpdate);

            base.Update(entityToUpdate);

        }
        public override void Insert(Goal entity)
        {
            if (entity.GoalGoodsCategory != null)
            {
                if (entity.GoalTypeId == GoalTypeEnum.Distributor)
                {
                    if (entity.GoalGoodsCategoryTypeId <= GoalGoodsCategoryTypeEnum.Group)
                        dbContext.Entry(entity.GoalGoodsCategory).State = EntityState.Modified;
                    else
                    {
                        dbContext.Set<GoalGoodsCategory>().Attach(entity.GoalGoodsCategory);
                        dbContext.Entry(entity.GoalGoodsCategory).State = EntityState.Added;
                    }
                }

                else
                {
                    //TODO : it should be reviewed at supplier goal developing time   
                    //NeutrinoContext.Set<GoalGoodsCategory>().Attach(entity.GoalGoodsCategory);
                    //NeutrinoContext.Entry(entity.GoalGoodsCategory).State = EntityState.Added;
                    //entity.GoalGoodsCategory.GoodsCollection.ToList().ForEach(x =>
                    //    NeutrinoContext.Entry<Goods>(x).State = EntityState.Unchanged);
                    //NeutrinoContext.Entry<Company>(entity.Company).State = EntityState.Unchanged;
                }
            }
            //CheckPerviousGoal(entity);
            base.Insert(entity);
        }
        public async Task<Goal> GetGoalAync(int goalId)
        {
            var query = await dbContext.Goals
                .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false))
                .IncludeFilter(x => x.GoalSteps.Where(y => y.Deleted == false).Select(st => st.Items))
                .IncludeFilter(x => x.GoalGoodsCategory)
                .IncludeFilter(x => x.GoalGoodsCategory.GoodsCollection.Where(y => y.Deleted == false))
                .IncludeFilter(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == goalId);

            return query;
        }
        public async Task<Goal> GetGoalInclude_GoalGoodsCategory_GoalSteps(int goalId)
        {
            return await dbContext.Goals
                        .Include(x => x.GoalGoodsCategory)
                        .Include(x => x.GoalSteps.Select(y => y.Items))
                        .Where(x => x.Id == goalId)
                        .FirstOrDefaultAsync();
        }
        public async Task<decimal> GetPreviousAggregationValueAsync(int month, int year)
        {
            decimal result = 0;
            if (month != 1)
            {
                result = await (from gl in dbContext.Goals
                                .AsNoTracking()
                                .Include(x => x.GoalSteps)
                                where gl.Month == month - 1 && gl.Year == year
                                && gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.AggregationGoal
                                && gl.Deleted == false
                                select gl.GoalSteps.FirstOrDefault().ComputingValue)
                                .FirstOrDefaultAsync();
            }
            return result;

        }
        public decimal GetPreviousAggregationValue(int month, int year)
        {
            decimal result = 0;
            if (month != 1)
            {
                result = (from gl in dbContext.Goals
                        .AsNoTracking()
                        .Include(x => x.GoalSteps)
                          where gl.Month == month - 1 && gl.Year == year
                          && gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.AggregationGoal
                          && gl.Deleted == false
                          select gl.GoalSteps.FirstOrDefault().ComputingValue)
                        .FirstOrDefault();
            }
            return result;

        }
        public decimal GetNextAggregationValue(int month, int year, decimal thisMonthAmount)
        {
            decimal result = 0;
            if (month != 12)
            {
                result = (from gl in dbContext.Goals
                        .AsNoTracking()
                        .Include(x => x.GoalSteps)
                          where gl.Month == month + 1 && gl.Year == year
                          && gl.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.AggregationGoal
                          && gl.Deleted == false
                          select gl.GoalSteps.FirstOrDefault().ComputingValue)
                        .FirstOrDefault();

            }
            return result == 0 ? thisMonthAmount : result;

        }
        
        #endregion

        #region [ Private Method(s) ]
        private void CheckPerviousGoal(Goal entity)
        {
            //if (entity.GoalGoodsCategory == null)
            //{
            //    IBusinessResultValue<GoalGoodsCategory> result = await goalGoodsCategoryBS.EntityLoader.LoadAsync(where: x => x.Id == entity.GoalGoodsCategoryId);
            //    entity.GoalGoodsCategory = result.ResultValue;
            //}


            Goal perviousGoal = FirstOrDefault(where: x => !x.Deleted
                && (x.EndDate == null || x.EndDate < entity.EndDate)
                && x.StartDate < entity.StartDate
                && x.GoalGoodsCategoryId == entity.GoalGoodsCategoryId
                && x.GoalGoodsCategoryTypeId == entity.GoalGoodsCategoryTypeId
                && x.Id != entity.Id
                && x.GoalTypeId == entity.GoalTypeId
                , includes: x => new { x.GoalGoodsCategory, x.GoalSteps }
                );

            if (perviousGoal != null)
            {
                perviousGoal.EndDate = entity.StartDate.AddDays(-1);
                //perviousGoal.GoalGoodsCategory.IsActive = false;
                perviousGoal.GoalSteps
                    .ToList()
                    .ForEach(x =>
                    {
                        x.IsActive = false;
                        dbContext.Entry(x).State = EntityState.Modified;
                    });


                dbContext.Entry(perviousGoal).State = EntityState.Modified;
            }
        }
        #endregion
    }


}
