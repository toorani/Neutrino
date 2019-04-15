using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface INeutrinoUserBS : IEnabledEntityListByPagingLoader<NeutrinoUser>
    {
        Task<IBusinessResultValue<NeutrinoUser>> LoadUserAsync(int userId);
    }
}
