using Neutrino.Interfaces;
using Ninject;
using Ninject.Activation;

namespace Notrino.Core.SecurityManagement
{
    public class PermissionManagerProvider : Provider<PermissionManager>
    {
        protected override PermissionManager CreateInstance(IContext context)
        {
            return new PermissionManager(context.Kernel.Get<IPermission>());
        }
    }
}
