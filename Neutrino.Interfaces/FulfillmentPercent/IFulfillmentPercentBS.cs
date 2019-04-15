using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IFulfillmentPercentBS : IBusinessService
    {
        Task<IBusinessResultValue<List<FulfillmentPercent>>> LoadFulfillmentListAsync(int year,int month);
        Task<IBusinessResultValue<List<FulfillmentPercent>>> SubmitDataAsync(List<FulfillmentPercent> lstGeneralGoal);
        
    }
}
