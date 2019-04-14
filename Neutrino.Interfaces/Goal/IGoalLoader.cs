using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IGoalLoader : IEntityLoader<Goal>
    {
        Task<IBusinessResultValue<Goal>> LoadGoalAync(int goalId);
    }
}
