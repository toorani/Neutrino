using System;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IPromotionBS : IBusinessService
        ,IEnabledEntityListByPagingLoader<Promotion>
        ,IEnabledEntityLoader<Promotion>
        
    {
        Task<IBusinessResult> CalculateAsync(Promotion entity);
        Task<IBusinessResultValue<Promotion>> AddPromotionAsync(Promotion entity);
        Task<IBusinessResult> PutInProcessQueueAsync(Promotion entity);
    }
}
