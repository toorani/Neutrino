using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neutrino.Interfaces
{
    public interface INeutrinoRoleDS 
    {
        Task<List<int>> GetRoleIdsByUserIdAsync(int userId);
    }
}
