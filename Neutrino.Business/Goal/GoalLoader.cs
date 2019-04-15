using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject.Extensions.Logging;

namespace Neutrino.Business
{
    public class GoalLoader : GeneralEntityLoader<Goal>
    {
        private readonly IGoalDS goalDataService;

        public GoalLoader(IGoalDS dataRepository, ILogger logger) 
            : base(dataRepository, logger)
        {
            goalDataService = dataRepository;
        }

        


    }
}
