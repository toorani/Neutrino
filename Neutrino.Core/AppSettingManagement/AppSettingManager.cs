using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Neutrino.DependencyResolver;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;
using Neutrino.Core.CashManagement;
using Espresso.Core.Interfaces;
using Espresso.Entites;
using Espresso.Utilities.Interfaces;
using Espresso.DataAccess.Interfaces;

namespace Neutrino.Core.AppSettingManagement
{
    public class AppSettingManager : IAppSettingManager
    {
        #region [ Varibale(s) ]
        private readonly IDataRepository<ApplicationSetting> appSettingDS;
        #endregion

        #region [ Constructor(s) ]
        public AppSettingManager(IDataRepository<ApplicationSetting> settingDataSerivce)
        {
            appSettingDS = settingDataSerivce;
        }
        #endregion

        #region [ Public Method(s) ]
        public List<ApplicationSetting> GetAll()
        {
            InMemoryCache cashProvider = new InMemoryCache();
            return cashProvider.GetOrSet("app.Settings", () => appSettingDS.Get(null).ToList());
        }
        public string GetValue(string keyName)
        {
            var setting = GetAll().FirstOrDefault(x => x.Key == keyName);
            if (setting != null)
            {
                return setting.Value;
            }
            return null;
        }
        public Nullable<TValue> GetValue<TValue>(string keyName)
            where TValue : struct
        {
            var setting = GetAll().FirstOrDefault(x => x.Key == keyName);
            if (setting != null)
            {
                try
                {
                    return Convert.ChangeType(setting.Value, typeof(TValue)) as Nullable<TValue>;
                }
                catch
                {
                    return null;
                }

            }
            return null;
        }

       
        #endregion


    }
}
