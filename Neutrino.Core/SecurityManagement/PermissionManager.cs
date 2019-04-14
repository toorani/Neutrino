using System.Collections.Generic;
using System.Linq;
using Espresso.Utilities.CashManagement;




namespace Neutrino.Core.SecurityManagement
{
    public class PermissionManager : IPermissionManager
    {


        #region [ Varibale(s) ]
        private readonly NeutrinoUnitOfWork unitOfWork;
        private Dictionary<int, List<Permission>> permissions;
        #endregion

        #region [ Constructor(s) ]
        public PermissionManager(NeutrinoUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            permissions = new Dictionary<int, List<Permission>>();
        }
        #endregion

        #region [ Public Method(s) ]
        public List<Permission> GetUserAccess(int userId)
        {
            if (permissions.Keys.Any(x => x == userId) == false)
            {
                permissions.Add(userId, unitOfWork.PermissionDataService.GetUserAccess(userId));
            }

            return permissions[userId];
            //InMemoryCache cashProvider = new InMemoryCache();
            //return cashProvider.GetOrSet<List<Permission>>(userId.ToString(), () => permissionDataSerivce.GetUserAccess(userId));
        }

        public bool HasAccess(string actionUrl, int userId)
        {
            if (permissions.Keys.Any(x => x == userId) == false)
            {
                permissions.Add(userId, unitOfWork.PermissionDataService.GetUserAccess(userId));
            }

            return permissions[userId].Any(x => x.ApplicationAction.ActionUrl != null
                && x.ApplicationAction.ActionUrl.ToLower() == actionUrl.ToLower());
        }

        public void SignOut(int userId)
        {
            InMemoryCache cashProvider = new InMemoryCache();
            cashProvider.Remove(userId.ToString());
        }
        #endregion

    }
}
