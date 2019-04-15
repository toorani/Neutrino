using System.Collections.Generic;
using Espresso.InjectModules;
using Neutrino.InjectModules;
using Ninject.Modules;

namespace Ninject.Http
{
    // List and Describe Necessary HttpModules
    // This class is optional if you already Have NinjectMvc
    public class NinjectHttpModules
    {
        //Return Lists of Modules in the Application
        public static List<NinjectModule> Modules
        {
            get
            {
                return new List<NinjectModule>() { new PortalModule()
                    , new SharedModule()
                    , new MainModule()
                    , new EspressoModule() };
            }
        }


    }


}