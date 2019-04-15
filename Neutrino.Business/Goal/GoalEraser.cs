using Espresso.BusinessService;
using Neutrino.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Ninject.Extensions.Logging;
using Neutrino.Interfaces;
using Espresso.BusinessService.Interfaces;

namespace Neutrino.Business
{
    public class GoalEraser : SimpleEntityEraser<Goal>
    {
        private IGoalDS dataService;

        public GoalEraser(IGoalDS dataRepository, ILogger logger) 
            : base(dataRepository, logger)
        {
            dataService = dataRepository;
        }

        
        public override IBusinessResult Delete(Goal entity)
        {
            return base.Delete(entity);
        }
    }
}
