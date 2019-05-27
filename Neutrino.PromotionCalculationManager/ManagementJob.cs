using Espresso.Core.Ninject;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.PromotionCalculationManager
{
    class ManagementJob : IJob
    {
        private readonly NeutrinoUnitOfWork unitOfWork;

        public ManagementJob()
        {
            unitOfWork = NinjectContainer.Resolve<NeutrinoUnitOfWork>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (unitOfWork.PromotionDataService.Exist(where: x => x.StatusId == PromotionStatusEnum.Initialized))
            {
                unitOfWork.context.DataBase

            }
        }
    }
}
