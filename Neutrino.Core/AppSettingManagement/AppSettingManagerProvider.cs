using Neutrino.Interfaces;
using Ninject;
using Ninject.Activation;

namespace Notrino.Core.AppSettingManagement
{
    public class AppSettingManagerProvider : Provider<AppSettingManager>
    {
        protected override AppSettingManager CreateInstance(IContext context)
        {
            return new AppSettingManager(context.Kernel.Get<IAppSetting>());
        }
    }
}
