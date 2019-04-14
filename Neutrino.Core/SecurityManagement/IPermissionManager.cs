using System.Collections.Generic;
using Neutrino.Entities;

namespace Neutrino.Core.SecurityManagement
{
    public interface IPermissionManager
    {
        List<Permission> GetUserAccess(int userId);
        bool HasAccess(string actionUrl, int userId);
        void SignOut(int userId);

    }
}
