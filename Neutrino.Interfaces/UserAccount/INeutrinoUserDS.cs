using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface INeutrinoUserDS :IEntityRepository<NeutrinoUser>
    {
        Task<NeutrinoUser> GetUserAsync(int userId);
    }
}
