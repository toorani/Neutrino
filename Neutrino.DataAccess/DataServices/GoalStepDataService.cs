using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class GoalStepDataService : NeutrinoRepositoryBase<GoalStep>
    {
        #region [ Constructor(s) ]
        public GoalStepDataService(NeutrinoContext context)
            : base(context)
        {
        }
        #endregion

        public override void Update(GoalStep entityToUpdate)
        {
            dbContext.GoalSteps.Attach(entityToUpdate);
            dbContext.Entry<GoalStep>(entityToUpdate).State = EntityState.Modified;

            var lstGoalStepInfos = dbContext.GoalStepItemInfos
                .Where(x => x.GoalStepId == entityToUpdate.Id)
                .AsNoTracking()
                .ToList();


            entityToUpdate.Items.ToList().ForEach(item =>
            {
                dbContext.GoalStepItemInfos.Attach(item);
                if (lstGoalStepInfos.Any(x => x.Id == item.Id))
                {
                    dbContext.Entry<GoalStepItemInfo>(item).State = EntityState.Modified;
                }
                else
                {
                    dbContext.Entry<GoalStepItemInfo>(item).State = EntityState.Added;
                }

            });

            lstGoalStepInfos.ForEach(inf => {
                if (entityToUpdate.Items.Any(x => x.Id == inf.Id) == false)
                {
                    dbContext.GoalStepItemInfos.Attach(inf);
                    dbContext.Entry<GoalStepItemInfo>(inf).State = EntityState.Deleted;
                }
            });
            
        }

    }
}
