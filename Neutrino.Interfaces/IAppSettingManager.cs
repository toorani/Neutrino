using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IAppSettingManager  
    {
        List<AppSetting> GetAll();
        string GetValue(string keyName);
        TValue GetValue<TValue>(string keyName);
    }
}
