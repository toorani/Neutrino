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
        Task<IBusinessResultValue<Promotion>> AddPromotionAsync(Promotion entity);
        Task<IBusinessResult> PutInProcessQueueAsync(int year,int month);
        Task<IBusinessResult> CalculateSalesGoalsAsync(Promotion entity);
        Task<IBusinessResult> CalculateReceiptGoalsAsync(Promotion entity);
    }
}
