using System.Data.Entity;
using Neutrino.Entities;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class GoalNonFulfillmentPercentDataService : NeutrinoRepositoryBase<GoalNonFulfillmentPercent>
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Constructor(s) ]
        public GoalNonFulfillmentPercentDataService(NeutrinoContext context)
            : base(context)
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public override void Update(GoalNonFulfillmentPercent entityToUpdate)
        {
            foreach (var item in entityToUpdate.GoalNonFulfillmentBranches)
            {
                if (item.Deleted)
                    dbContext.Entry(item).State = EntityState.Modified;
                else if (item.Id == 0)
                    dbContext.Entry(item).State = EntityState.Added;
            }

            base.Update(entityToUpdate);
        }
        public override void Insert(GoalNonFulfillmentPercent entity)
        {
            foreach (var item in entity.GoalNonFulfillmentBranches)
            {
                dbContext.Entry(item).State = EntityState.Added;
            }
            base.Insert(entity);
        }
        #endregion
    }
}
