using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Microsoft.AspNet.Identity;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IUserDS  
    {
        void Insert(User entity);

    }
}
