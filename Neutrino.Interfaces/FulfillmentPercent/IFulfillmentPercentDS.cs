using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IFulfillmentPercentDS : IEntityRepository<FulfillmentPercent>
    {
        Task<List<FulfillmentPercent>> GetFulfillmentListAsync(int year, int month);
        void InsertFulfillment(List<FulfillmentPercent> lstGoalFulfillment);
    }
}
