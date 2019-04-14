using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Core
{
    public enum LogLayers : int
    {
        Api,
        Business,
        Service,
        DataAccess
    }

    [Flags]
    public enum LoggerTypes : int
    {
        None = 0,
        Database = 2,
        XML = 4,
        File = 8,
        EMail = 16,
        SMS = 32,
        NetSend = 64
    }
    public enum ExceptionTypes
    {
        DataBase,
        Application,
        Business,
        Service,
        None,
    }
    

    
}
